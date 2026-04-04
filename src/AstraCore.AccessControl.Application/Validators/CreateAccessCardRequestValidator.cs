using AstraCore.AccessControl.Application.DTOs.AccessCard;
using FluentValidation;

namespace AstraCore.AccessControl.Application.Validators;

public sealed class CreateAccessCardRequestValidator : AbstractValidator<CreateAccessCardRequest>
{
    public CreateAccessCardRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.CardNumber)
            .NotEmpty().WithMessage("Card number is required.")
            .Matches(@"^[A-Za-z0-9]{16}$").WithMessage("Card number must be exactly 16 alphanumeric characters.");

        RuleFor(x => x.AccessLevel)
            .IsInEnum().WithMessage("Access level is not valid.");

        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.ExpiryDate)
            .Must(date => date > DateTime.UtcNow).WithMessage("Expiry date must be in the future.");
    }
}
