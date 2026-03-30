using AstraCore.AccessControl.Domain.Common;
using AstraCore.AccessControl.Domain.Enums;
using AstraCore.AccessControl.Domain.ValueObjects;

namespace AstraCore.AccessControl.Domain.Entities;

public sealed class AccessCard : BaseEntity
{
	public CardNumber CardNumber { get; private set; } = null!; 
	public AccessLevel AccessLevel { get; private set; }
	public DateTime IssuedDate { get; private set; }
	public DateTime ExpiryDate { get; private set; }
	public bool IsActive { get; private set; }
	public Guid EmployeeId { get; private set; }
	public Employee Employee { get; private set; } = null!;

	private readonly List<AccessLog> _accessLogs = new();
	public IReadOnlyCollection<AccessLog> AccessLogs => _accessLogs.AsReadOnly();

	public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
	public bool IsValid => IsActive && !IsExpired;


	private AccessCard() { }

	public AccessCard(string cardNumber, AccessLevel accessLevel, Guid employeeId, DateTime expiryDate)
	{
		if (employeeId == Guid.Empty)
			throw new ArgumentException("Employee ID is required.", nameof(employeeId));
		if (expiryDate <= DateTime.UtcNow)
			throw new ArgumentException("Expiry date must be in the future.", nameof(expiryDate));

		CardNumber = CardNumber.Create(cardNumber);
		AccessLevel = accessLevel;
		EmployeeId = employeeId;
		IssuedDate = DateTime.UtcNow;
		ExpiryDate = expiryDate;
		IsActive = true;
	}

	public void Deactivate()
	{
		IsActive = false;
		Touch();
	}

	public void Reactivate()
	{
		if (IsExpired)
			throw new InvalidOperationException("Cannot reactivate an expired card.");

		IsActive = true;
		Touch();
	}

	public void ExtendExpiry(DateTime newExpiryDate)
	{
		if (newExpiryDate <= ExpiryDate)
			throw new InvalidOperationException("New expiry date must be later than current expiry date.");

		ExpiryDate = newExpiryDate;
		Touch();
	}

	public void ChangeAccessLevel(AccessLevel newLevel)
	{
		AccessLevel = newLevel;
		Touch();
	}
}
