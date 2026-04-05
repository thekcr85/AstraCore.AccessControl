using AstraCore.AccessControl.Application.DTOs.AccessPoint;
using FluentValidation;

namespace AstraCore.AccessControl.Application.Validators.AccessPoint;

public sealed class CreateAccessPointRequestValidator : AbstractValidator<CreateAccessPointRequest>
{
    public CreateAccessPointRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(200).WithMessage("Location cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
            .When(x => x.Description is not null);

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Access point type is not valid.");

        RuleFor(x => x.RequiredAccessLevel)
            .IsInEnum().WithMessage("Required access level is not valid.");
    }
}
