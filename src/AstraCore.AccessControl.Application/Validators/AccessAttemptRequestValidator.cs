using AstraCore.AccessControl.Application.DTOs.AccessLog;
using FluentValidation;

namespace AstraCore.AccessControl.Application.Validators;

public sealed class AccessAttemptRequestValidator : AbstractValidator<AccessAttemptRequest>
{
    public AccessAttemptRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.CardNumber)
            .NotEmpty().WithMessage("Card number is required.")
            .Matches(@"^[A-Za-z0-9]{16}$").WithMessage("Card number must be exactly 16 alphanumeric characters.");

        RuleFor(x => x.AccessPointId)
            .NotEmpty().WithMessage("Access point ID is required.");
    }
}
