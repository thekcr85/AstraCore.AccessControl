using AstraCore.AccessControl.Domain.Common;
using AstraCore.AccessControl.Domain.Enums;

namespace AstraCore.AccessControl.Domain.Entities;

public sealed class Employee : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string? Position { get; set; }
    public DateTime HireDate { get; set; }
    public EmploymentStatus Status { get; set; }

    public ICollection<AccessCard> AccessCards { get; set; } = new List<AccessCard>();

    public string FullName => $"{FirstName} {LastName}";

    public bool IsActive => Status == EmploymentStatus.Active;

    private Employee()
    {
    }

    public Employee(string firstName, string lastName, string email, string department, DateTime hireDate)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Department = department;
        HireDate = hireDate;
        Status = EmploymentStatus.Active;
    }

    public void Activate() => Status = EmploymentStatus.Active;
    public void Suspend() => Status = EmploymentStatus.Suspended;
    public void Terminate() => Status = EmploymentStatus.Terminated;
    public void SetOnLeave() => Status = EmploymentStatus.OnLeave;
}
