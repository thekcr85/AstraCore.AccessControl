namespace AstraCore.AccessControl.Application.DTOs.AccessPoint;

public sealed record AccessPointResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Location { get; init; }
    public string? Description { get; init; }
    public required string Type { get; init; }
    public required string RequiredAccessLevel { get; init; }
    public required bool IsEnabled { get; init; }
    public required DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
