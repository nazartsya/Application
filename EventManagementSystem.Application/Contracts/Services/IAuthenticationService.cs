using EventManagementSystem.Application.Models.Authentication;

namespace EventManagementSystem.Application.Contracts.Services;

public interface IAuthenticationService
{
    Task<LoginUserResponse> LoginUserAsync(LoginUserRequest request);
    Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request);
}
