namespace AstraCore.AccessControl.Application.DTOs.Employee;

public sealed record EmployeeResponse
{
    public required Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required string Department { get; init; }
    public string? Position { get; init; }
    public required DateTime HireDate { get; init; }
    public required string Status { get; init; }
    public required bool IsActive { get; init; }
    public required DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
