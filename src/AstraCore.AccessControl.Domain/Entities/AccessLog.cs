using AstraCore.AccessControl.Domain.Common;
using AstraCore.AccessControl.Domain.Enums;

namespace AstraCore.AccessControl.Domain.Entities;

public sealed class AccessLog : BaseEntity
{
    public Guid AccessCardId { get; private set; }
    public AccessCard AccessCard { get; private set; } = null!;

    public Guid AccessPointId { get; private set; }
    public AccessPoint AccessPoint { get; private set; } = null!;
    public DateTime AttemptedAt { get; private set; }
    public AccessResult Result { get; private set; }
    public string? Notes { get; private set; }

    public bool WasSuccessful => Result == AccessResult.Granted;

    private AccessLog()
    {
    }

    public AccessLog(Guid accessCardId, Guid accessPointId, AccessResult result, string? notes = null)
    {
		if (accessCardId == Guid.Empty)
			throw new ArgumentException("Access card ID is required.", nameof(accessCardId));
		if (accessPointId == Guid.Empty)
			throw new ArgumentException("Access point ID is required.", nameof(accessPointId));

		AccessCardId = accessCardId;
        AccessPointId = accessPointId;
        AttemptedAt = DateTime.UtcNow;
        Result = result;
        Notes = notes;
    }

}
