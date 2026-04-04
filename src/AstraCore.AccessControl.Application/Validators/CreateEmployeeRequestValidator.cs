using AstraCore.AccessControl.Application.DTOs.Employee;
using FluentValidation;

namespace AstraCore.AccessControl.Application.Validators;

public sealed class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
{
	public CreateEmployeeRequestValidator()
	{
		RuleLevelCascadeMode = CascadeMode.Stop;

		RuleFor(x => x.FirstName)
			.NotEmpty().WithMessage("First name is required.")
			.MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");

		RuleFor(x => x.LastName)
			.NotEmpty().WithMessage("Last name is required.")
			.MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");

		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Email is required.")
			.MaximumLength(200).WithMessage("Email cannot exceed 200 characters.")
			.EmailAddress().WithMessage("Email format is invalid.");

		RuleFor(x => x.Department)
			.NotEmpty().WithMessage("Department is required.")
			.MaximumLength(100).WithMessage("Department cannot exceed 100 characters.");

		RuleFor(x => x.Position)
			.MaximumLength(100).WithMessage("Position cannot exceed 100 characters.")
			.When(x => x.Position is not null);

		RuleFor(x => x.HireDate)
			.NotEqual(default(DateTime)).WithMessage("Hire date is required.")
			.Must(date => date <= DateTime.UtcNow).WithMessage("Hire date cannot be in the future.");
	}
}