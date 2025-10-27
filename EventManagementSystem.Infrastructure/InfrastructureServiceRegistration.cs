using EventManagementSystem.Application.Contracts.Repositories;
using EventManagementSystem.Application.Contracts.Services;
using EventManagementSystem.Application.Models.Authentication;
using EventManagementSystem.Domain.Entities;
using EventManagementSystem.Infrastructure.Repositories;
using EventManagementSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EventManagementSystem.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("EventManagementApplicationDB")));

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IUserService, UserService>();

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(configureOptions =>
        {
            configureOptions.RequireHttpsMetadata = false;
            configureOptions.SaveToken = false;
            configureOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!))
            };
        });

        return services;
    }
}
