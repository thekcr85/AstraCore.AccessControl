namespace AstraCore.AccessControl.Application.DTOs.AccessLog;

public sealed record AccessLogResponse
{
    public required Guid Id { get; init; }
    public required Guid AccessCardId { get; init; }
    public required Guid AccessPointId { get; init; }
    public required DateTime AttemptedAt { get; init; }
    public required string Result { get; init; }
    public required bool WasSuccessful { get; init; }
    public string? Notes { get; init; }
}
