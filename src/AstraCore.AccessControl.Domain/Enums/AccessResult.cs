namespace AstraCore.AccessControl.Domain.Enums;

public enum AccessResult
{
    Granted = 0,
    Denied = 1,
    CardInvalid = 2,
    CardExpired = 3,
    InsufficientClearance = 4,
    EmployeeInactive = 5,
    AccessPointDisabled = 6
}
