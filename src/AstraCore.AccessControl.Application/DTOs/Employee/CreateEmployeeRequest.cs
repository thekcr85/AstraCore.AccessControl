namespace AstraCore.AccessControl.Application.DTOs.Employee;

public sealed record CreateEmployeeRequest
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string Department { get; init; }
    public string? Position { get; init; }
    public required DateTime HireDate { get; init; }
}
