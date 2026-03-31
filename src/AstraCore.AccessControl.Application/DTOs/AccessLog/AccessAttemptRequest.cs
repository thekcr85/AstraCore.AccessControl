namespace AstraCore.AccessControl.Application.DTOs.AccessLog;

public sealed record AccessAttemptRequest
{
    public required string CardNumber { get; init; }
    public required Guid AccessPointId { get; init; }
}
