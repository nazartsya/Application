using EventManagementSystem.Application.Exceptions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagementSystem.Application.Helpers;

public static class ValidationHelper
{
    public static async Task ThrowIfInvalidAsync<T>(T model, IServiceProvider serviceProvider)
    {
        var validator = serviceProvider.GetService<IValidator<T>>();
        if (validator == null) return;

        var result = await validator.ValidateAsync(model);
        if (result.IsValid) return;

        var errors = result.Errors
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(g => g.Key, g => g.ToArray());

        throw new CustomValidationException(errors);
    }
}
