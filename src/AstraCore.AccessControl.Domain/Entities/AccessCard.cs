using AstraCore.AccessControl.Domain.Common;
using AstraCore.AccessControl.Domain.Enums;
using AstraCore.AccessControl.Domain.ValueObjects;

namespace AstraCore.AccessControl.Domain.Entities;

public sealed class AccessCard : BaseEntity
{
    public string CardNumber { get; set; } = string.Empty;
    public AccessLevel AccessLevel { get; set; }
    public DateTime IssuedDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsActive { get; set; }

    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public ICollection<AccessLog> AccessLogs { get; set; } = new List<AccessLog>();

    public bool IsValid => IsActive && DateTime.UtcNow < ExpiryDate;

    public bool IsExpired => DateTime.UtcNow >= ExpiryDate;

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
