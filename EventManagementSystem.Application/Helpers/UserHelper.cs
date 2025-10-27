using EventManagementSystem.Application.Exceptions;
using System.Security.Claims;

namespace EventManagementSystem.Application.Helpers;

public static class UserHelper
{
    public static Guid GetUserId(ClaimsPrincipal user)
    {
        var userIdString = user.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
        if (string.IsNullOrWhiteSpace(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            throw new UnauthorizedException("User is unauthorized.");
        }

        return userId;
    }
}
