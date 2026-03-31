namespace AstraCore.AccessControl.Application.DTOs.Employee;

public sealed record UpdateEmployeeRequest
{
    public required string Email { get; init; }
    public required string Department { get; init; }
    public string? Position { get; init; }
}
