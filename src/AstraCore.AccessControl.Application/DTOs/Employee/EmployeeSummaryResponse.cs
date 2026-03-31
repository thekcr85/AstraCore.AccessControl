namespace AstraCore.AccessControl.Application.DTOs.Employee;

public sealed record EmployeeSummaryResponse
{
    public required Guid Id { get; init; }
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required string Department { get; init; }
    public required string Status { get; init; }
}
