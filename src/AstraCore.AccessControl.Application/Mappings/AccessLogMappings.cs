using AstraCore.AccessControl.Application.DTOs.AccessLog;
using AstraCore.AccessControl.Domain.Entities;

namespace AstraCore.AccessControl.Application.Mappings;

public static class AccessLogMappings
{
    public static AccessLogResponse ToResponse(this AccessLog log) => new()
    {
        Id = log.Id,
        AccessCardId = log.AccessCardId,
        AccessPointId = log.AccessPointId,
        AttemptedAt = log.AttemptedAt,
        Result = log.Result.ToString(),
        WasSuccessful = log.WasSuccessful,
        Notes = log.Notes
    };

    public static IEnumerable<AccessLogResponse> ToResponseList(this IEnumerable<AccessLog> logs)
        => logs.Select(l => l.ToResponse());
}
