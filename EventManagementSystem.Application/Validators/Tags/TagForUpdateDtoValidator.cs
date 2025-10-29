using EventManagementSystem.Application.Models.Tags;
using FluentValidation;

namespace EventManagementSystem.Application.Validators.Tags;

public class TagForUpdateDtoValidator : AbstractValidator<TagForUpdateDto>
{
    public TagForUpdateDtoValidator()
    {
        RuleFor(t => t.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.");
    }
}
