using AstraCore.AccessControl.Domain.Enums;

namespace AstraCore.AccessControl.Application.DTOs.AccessPoint;

public sealed record CreateAccessPointRequest
{
    public required string Name { get; init; }
    public required string Location { get; init; }
    public string? Description { get; init; }
    public required AccessPointType Type { get; init; }
    public required AccessLevel RequiredAccessLevel { get; init; }
}
