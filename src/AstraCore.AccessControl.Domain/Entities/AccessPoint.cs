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

    private readonly List<AccessLog> _accessLogs = [];
    public IReadOnlyCollection<AccessLog> AccessLogs => _accessLogs.AsReadOnly();

	private AccessPoint() { }

	public AccessPoint(string name, string location, AccessPointType type, AccessLevel requiredAccessLevel, string? description = null)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentException("Name is required.", nameof(name));
		if (string.IsNullOrWhiteSpace(location))
			throw new ArgumentException("Location is required.", nameof(location));

		Name = name;
		Location = location;
		Description = description;
		Type = type;
		RequiredAccessLevel = requiredAccessLevel;
		IsEnabled = true;
	}

    public void Enable()
    {
        IsEnabled = true;
        Touch();
    }

    public void Disable()
    {
        IsEnabled = false;
        Touch();
    }

	public void UpdateDetails(string name, string location, string? description)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentException("Name is required.", nameof(name));
		if (string.IsNullOrWhiteSpace(location))
			throw new ArgumentException("Location is required.", nameof(location));

		Name = name;
		Location = location;
		Description = description;
		Touch();
	}

	public void ChangeRequiredAccessLevel(AccessLevel newLevel)
	{
		RequiredAccessLevel = newLevel;
		Touch();
	}

	public bool CanAccess(AccessCard card)
	{
		return IsEnabled && card.IsValid && card.AccessLevel >= RequiredAccessLevel;
	}
}
