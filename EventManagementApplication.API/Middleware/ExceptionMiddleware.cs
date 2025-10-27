using EventManagementSystem.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace EventManagementApplication.API.Middleware;

public static class ExceptionMiddleware
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                context.Response.ContentType = "application/problem+json";

                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                if (exception is CustomValidationException validationException)
                {
                    context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        title = "One or more validation errors occurred.",
                        errors = validationException.Errors,
                    });
                    return;
                }

                if (exception is NotFoundException)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        title = "Resource not found",
                        detail = exception.Message
                    });
                    return;
                }

                if (exception is BadRequestException)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        title = "Bad Request",
                        detail = exception.Message
                    });
                    return;
                }

                if (exception is UnauthorizedException)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        title = "Unauthorized",
                        detail = exception.Message
                    });
                    return;
                }

                if (exception is ForbiddenException)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        title = "Forbidden",
                        detail = exception.Message
                    });
                    return;
                }

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new
                {
                    title = "Internal Server Error",
                    detail = exception?.Message
                });
            });
        });
    }
}
