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

        RuleFor(e => e.Tags)
            .NotNull().WithMessage("Tags are required.")
            .Must(tags => tags.Count >= 1).WithMessage("At least 1 tag is required.")
            .Must(tags => tags.Count <= 5).WithMessage("Maximum 5 tags allowed per event.");

        RuleFor(e => e.Tags)
            .Must(tags => tags.Select(t => t.Name.ToLower()).Distinct().Count() == tags.Count)
            .WithMessage("Duplicate tags are not allowed.");
    }
}
