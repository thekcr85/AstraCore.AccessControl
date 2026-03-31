namespace AstraCore.AccessControl.Application.DTOs.AccessCard;

public sealed record AccessCardResponse
{
    public required Guid Id { get; init; }
    public required string CardNumber { get; init; }
    public required string AccessLevel { get; init; }
    public required DateTime IssuedDate { get; init; }
    public required DateTime ExpiryDate { get; init; }
    public required bool IsActive { get; init; }
    public required bool IsValid { get; init; }
    public required bool IsExpired { get; init; }
    public required Guid EmployeeId { get; init; }
    public required DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
