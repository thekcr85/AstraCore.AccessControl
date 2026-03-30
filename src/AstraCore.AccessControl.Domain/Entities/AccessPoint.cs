using AstraCore.AccessControl.Domain.Common;
using AstraCore.AccessControl.Domain.Enums;

namespace AstraCore.AccessControl.Domain.Entities;

public sealed class AccessPoint : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public AccessPointType Type { get; private set; }
    public AccessLevel RequiredAccessLevel { get; private set; }
    public bool IsEnabled { get; private set; }

    public ICollection<AccessLog> AccessLogs { get; private set; } = new List<AccessLog>();

    public AccessPoint()
    {
    }

    public AccessPoint(string name, string location, AccessPointType type, AccessLevel requiredAccessLevel)
    {
        Name = name;
        Location = location;
        Type = type;
        RequiredAccessLevel = requiredAccessLevel;
        IsEnabled = true;
    }

    public void Enable() => IsEnabled = true;
    public void Disable() => IsEnabled = false;

    public bool CanAccess(AccessLevel cardAccessLevel)
    {
        return IsEnabled && cardAccessLevel >= RequiredAccessLevel;
    }
}
