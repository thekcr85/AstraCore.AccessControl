using AstraCore.AccessControl.Application.DTOs.AccessCard;
using FluentValidation;

namespace AstraCore.AccessControl.Application.Validators.AccessCard;

public sealed class UpdateAccessCardRequestValidator : AbstractValidator<UpdateAccessCardRequest>
{
    public UpdateAccessCardRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.AccessLevel)
            .IsInEnum().WithMessage("Access level is not valid.");

        RuleFor(x => x.ExpiryDate)
            .Must(date => date > DateTime.UtcNow).WithMessage("Expiry date must be in the future.");
    }
}
