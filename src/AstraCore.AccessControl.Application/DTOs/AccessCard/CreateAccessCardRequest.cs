using AstraCore.AccessControl.Domain.Enums;

namespace AstraCore.AccessControl.Application.DTOs.AccessCard;

public sealed record CreateAccessCardRequest
{
    public required string CardNumber { get; init; }
    public required AccessLevel AccessLevel { get; init; }
    public required Guid EmployeeId { get; init; }
    public required DateTime ExpiryDate { get; init; }
}
