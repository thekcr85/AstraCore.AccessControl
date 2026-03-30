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


    public AccessCard()
    {
    }

    public AccessCard(string cardNumber, AccessLevel accessLevel, Guid employeeId, DateTime expiryDate)
    {
        CardNumber = cardNumber;
        AccessLevel = accessLevel;
        EmployeeId = employeeId;
        IssuedDate = DateTime.UtcNow;
        ExpiryDate = expiryDate;
        IsActive = true;
    }

    public void Deactivate() => IsActive = false;
    public void Reactivate() => IsActive = true;

    public void ExtendExpiry(DateTime newExpiryDate)
    {
        if (newExpiryDate <= ExpiryDate)
            throw new InvalidOperationException("New expiry date must be later than current expiry date");

        ExpiryDate = newExpiryDate;
    }
}
