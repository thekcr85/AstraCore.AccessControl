using AstraCore.AccessControl.Domain.Common;
using AstraCore.AccessControl.Domain.Enums;

namespace AstraCore.AccessControl.Domain.Entities;

public sealed class Employee : BaseEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Department { get; private set; } = string.Empty;
    public string? Position { get; private set; }
    public DateTime HireDate { get; private set; }
    public EmploymentStatus Status { get; private set; }

    private readonly List<AccessCard> AccessCards = new List<AccessCard>();
    public IReadOnlyCollection<AccessCard> GetAccessCards() => AccessCards.AsReadOnly();

	public string FullName => $"{FirstName} {LastName}";

    public bool IsActive => Status == EmploymentStatus.Active;

    private Employee()
    {
    }

    public Employee(string firstName, string lastName, string email, string department, DateTime hireDate)
    {
		if (string.IsNullOrWhiteSpace(firstName))
			throw new ArgumentException("First name is required.", nameof(firstName));
		if (string.IsNullOrWhiteSpace(lastName))
			throw new ArgumentException("Last name is required.", nameof(lastName));
		if (string.IsNullOrWhiteSpace(email))
			throw new ArgumentException("Email is required.", nameof(email));
		if (string.IsNullOrWhiteSpace(department))
			throw new ArgumentException("Department is required.", nameof(department));

		FirstName = firstName;
        LastName = lastName;
        Email = email;
        Department = department;
        HireDate = hireDate;
        Status = EmploymentStatus.Active;
    }

    public void Activate()
    {
        Status = EmploymentStatus.Active;
        Touch();
	}

    public void Suspend()
    {
        Status = EmploymentStatus.Suspended;
        Touch();
    }

    public void Terminate()
    {
        Status = EmploymentStatus.Terminated;
        Touch();
    }

    public void SetOnLeave()
    {
        Status = EmploymentStatus.OnLeave;
        Touch();
    }
}