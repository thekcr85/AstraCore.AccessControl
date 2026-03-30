using AstraCore.AccessControl.Domain.Common;
using AstraCore.AccessControl.Domain.Enums;

namespace AstraCore.AccessControl.Domain.Entities;

public sealed class AccessCard : BaseEntity
{
	public string CardNumber { get; private set; } = string.Empty;
	public AccessLevel AccessLevel { get; private set; }
	public DateTime IssuedDate { get; private set; }
	public DateTime ExpiryDate { get; private set; }
	public bool IsActive { get; private set; }
	public Guid EmployeeId { get; private set; }
	public Employee Employee { get; private set; } = null!;

	private readonly List<AccessLog> _accessLogs = new List<AccessLog>();
	public IReadOnlyCollection<AccessLog> GetAccessLogs() => _accessLogs.AsReadOnly();

	public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
	public bool IsValid => IsActive && !IsExpired;


	private AccessCard() { }

	public AccessCard(string cardNumber, AccessLevel accessLevel, Guid employeeId, DateTime expiryDate)
	{
		if (string.IsNullOrWhiteSpace(cardNumber))
			throw new ArgumentException("Card number is required.", nameof(cardNumber));
		if (employeeId == Guid.Empty)
			throw new ArgumentException("Employee ID is required.", nameof(employeeId));
		if (expiryDate <= DateTime.UtcNow)
			throw new ArgumentException("Expiry date must be in the future.", nameof(expiryDate));

		CardNumber = cardNumber;
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
		IsActive = true;
		Touch();
	}


	public void ExtendExpiry(DateTime newExpiryDate)
	{
		if (newExpiryDate <= ExpiryDate)
			throw new InvalidOperationException("New expiry date must be later than current expiry date");

		ExpiryDate = newExpiryDate;
	}
}
