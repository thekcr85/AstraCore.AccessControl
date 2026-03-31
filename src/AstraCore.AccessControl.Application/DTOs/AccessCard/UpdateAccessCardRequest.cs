using AstraCore.AccessControl.Domain.Enums;

namespace AstraCore.AccessControl.Application.DTOs.AccessCard;

public sealed record UpdateAccessCardRequest
{
    public required AccessLevel AccessLevel { get; init; }
    public required DateTime ExpiryDate { get; init; }
}
