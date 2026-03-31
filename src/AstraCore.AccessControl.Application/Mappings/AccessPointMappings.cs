using AstraCore.AccessControl.Application.DTOs.AccessPoint;
using AstraCore.AccessControl.Domain.Entities;

namespace AstraCore.AccessControl.Application.Mappings;

public static class AccessPointMappings
{
    public static AccessPointResponse ToResponse(this AccessPoint accessPoint) => new()
    {
        Id = accessPoint.Id,
        Name = accessPoint.Name,
        Location = accessPoint.Location,
        Description = accessPoint.Description,
        Type = accessPoint.Type.ToString(),
        RequiredAccessLevel = accessPoint.RequiredAccessLevel.ToString(),
        IsEnabled = accessPoint.IsEnabled,
        CreatedAt = accessPoint.CreatedAt,
        UpdatedAt = accessPoint.UpdatedAt
    };

    public static IEnumerable<AccessPointResponse> ToResponseList(this IEnumerable<AccessPoint> accessPoints)
        => accessPoints.Select(ap => ap.ToResponse());
}
