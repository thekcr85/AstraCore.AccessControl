using AstraCore.AccessControl.Application.DTOs.Employee;
using AstraCore.AccessControl.Domain.Entities;

namespace AstraCore.AccessControl.Application.Mappings;

public static class EmployeeMappings
{
    public static EmployeeResponse ToResponse(this Employee employee) => new()
    {
        Id = employee.Id,
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        FullName = employee.FullName,
        Email = employee.Email,
        Department = employee.Department,
        Position = employee.Position,
        HireDate = employee.HireDate,
        Status = employee.Status.ToString(),
        IsActive = employee.IsActive,
        CreatedAt = employee.CreatedAt,
        UpdatedAt = employee.UpdatedAt
    };

    public static EmployeeSummaryResponse ToSummaryResponse(this Employee employee) => new()
    {
        Id = employee.Id,
        FullName = employee.FullName,
        Email = employee.Email,
        Department = employee.Department,
        Status = employee.Status.ToString()
    };

    public static IEnumerable<EmployeeSummaryResponse> ToSummaryResponseList(this IEnumerable<Employee> employees)
        => employees.Select(e => e.ToSummaryResponse());
}
