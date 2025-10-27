using EventManagementSystem.Application.Models.Events;
using FluentValidation;

namespace EventManagementSystem.Application.Validators;

public class EventForUpdateDtoValidator : AbstractValidator<EventForUpdateDto>
{
    public EventForUpdateDtoValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();

        RuleFor(e => e.Description)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MaximumLength(1500).WithMessage("{PropertyName} must not exceed 1500 characters.");

        RuleFor(e => e.Date)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();

        RuleFor(e => e.Location)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();

        RuleFor(e => e.Capacity)
            .GreaterThan(0).When(e => e.Capacity.HasValue)
            .WithMessage("Capacity must be greater than 0 when specified.");
    }
}
