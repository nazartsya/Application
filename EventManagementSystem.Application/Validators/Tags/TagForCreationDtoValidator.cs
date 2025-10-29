using EventManagementSystem.Application.Models.Tags;
using FluentValidation;

namespace EventManagementSystem.Application.Validators.Tags;

public class TagForCreationDtoValidator : AbstractValidator<TagForCreationDto>
{
    public TagForCreationDtoValidator()
    {
        RuleFor(t => t.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.");
    }
}
