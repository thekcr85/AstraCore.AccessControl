using AstraCore.AccessControl.Domain.Common;
using AstraCore.AccessControl.Domain.Enums;

namespace AstraCore.AccessControl.Domain.Entities;

public sealed class AccessLog : BaseEntity
{
    public Guid AccessCardId { get; set; }
    public AccessCard AccessCard { get; set; } = null!;

    public Guid AccessPointId { get; set; }
    public AccessPoint AccessPoint { get; set; } = null!;

    public DateTime AttemptedAt { get; set; }
    public AccessResult Result { get; set; }
    public string? Notes { get; set; }

    public AccessLog()
    {
    }

    public AccessLog(Guid accessCardId, Guid accessPointId, AccessResult result, string? notes = null)
    {
        AccessCardId = accessCardId;
        AccessPointId = accessPointId;
        AttemptedAt = DateTime.UtcNow;
        Result = result;
        Notes = notes;
    }

    public bool WasSuccessful => Result == AccessResult.Granted;
}
