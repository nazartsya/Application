using EventManagementSystem.Application.Contracts.Services;
using EventManagementSystem.Application.Exceptions;
using EventManagementSystem.Application.Models.Authentication;
using EventManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventManagementSystem.Infrastructure.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JwtSettings _jwtSettings;

    public AuthenticationService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _signInManager = signInManager;
    }

    public async Task<LoginUserResponse> LoginUserAsync(LoginUserRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new Exception($"User with {request.Email} not found.");

        var result = await _signInManager.PasswordSignInAsync(user.UserName!, request.Password, false, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            throw new BadRequestException($"Credentials for {request.Email} aren't valid.");
        }

        JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

        LoginUserResponse response = new()
        {
            Id = user.Id,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            Email = user.Email!,
            UserName = user.UserName!
        };

        return response;
    }

    public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request)
    {
        var existingUser = await _userManager.FindByNameAsync(request.UserName);
        if (existingUser != null)
        {
            throw new BadRequestException($"Username {request.UserName} already exists.");
        }

        var existingEmail = await _userManager.FindByEmailAsync(request.Email);
        if (existingEmail != null)
        {
            throw new BadRequestException($"Email {request.Email} already exists.");
        }

        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors
                .GroupBy(e => e.Code)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.Description).ToArray()
                );

            throw new CustomValidationException(errors);
        }

        JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

        RegisterUserResponse response = new()
        {
            Id = user.Id,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            Email = user.Email,
            UserName = user.UserName
        };

        return response;
    }

    private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();

        for (int i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim("roles", roles[i]));
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim("uid", user.Id.ToString())
        }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }
}
