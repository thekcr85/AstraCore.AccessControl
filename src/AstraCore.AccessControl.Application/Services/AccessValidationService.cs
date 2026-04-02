using AstraCore.AccessControl.Application.DTOs.AccessLog;
using AstraCore.AccessControl.Application.Interfaces.Repositories;
using AstraCore.AccessControl.Application.Interfaces.Services;
using AstraCore.AccessControl.Application.Mappings;
using AstraCore.AccessControl.Domain.Entities;
using AstraCore.AccessControl.Domain.Enums;

namespace AstraCore.AccessControl.Application.Services;

public sealed class AccessValidationService(
    IAccessCardRepository cardRepository,
    IAccessPointRepository accessPointRepository,
    IAccessLogRepository logRepository,
    IEmployeeRepository employeeRepository) : IAccessValidationService
{
    public async Task<AccessLogResponse> ValidateAccessAsync(AccessAttemptRequest request, CancellationToken cancellationToken = default)
    {
        // Step 1: Find the card by its number
        var card = await cardRepository.GetByCardNumberAsync(request.CardNumber, cancellationToken);
        if (card is null)
            return CreateDenialResponse(request.AccessPointId, AccessResult.CardInvalid);

        // Step 2: Check the employee is active
        var employee = await employeeRepository.GetByIdAsync(card.EmployeeId, cancellationToken);
        if (employee is null || !employee.IsActive)
            return await LogAndSaveAsync(card.Id, request.AccessPointId, AccessResult.EmployeeInactive, cancellationToken);

        // Step 3: Check the card is valid and not expired
        if (!card.IsActive)
            return await LogAndSaveAsync(card.Id, request.AccessPointId, AccessResult.CardInvalid, cancellationToken);

        if (card.IsExpired)
            return await LogAndSaveAsync(card.Id, request.AccessPointId, AccessResult.CardExpired, cancellationToken);

        // Step 4: Find the access point
        var accessPoint = await accessPointRepository.GetByIdAsync(request.AccessPointId, cancellationToken)
            ?? throw new KeyNotFoundException($"Access point {request.AccessPointId} was not found.");

        if (!accessPoint.IsEnabled)
            return await LogAndSaveAsync(card.Id, request.AccessPointId, AccessResult.AccessPointDisabled, cancellationToken);

        // Step 5: Check clearance level
        if (card.AccessLevel < accessPoint.RequiredAccessLevel)
            return await LogAndSaveAsync(card.Id, request.AccessPointId, AccessResult.InsufficientClearance, cancellationToken);

        // Step 6: All checks passed — access granted
        return await LogAndSaveAsync(card.Id, request.AccessPointId, AccessResult.Granted, cancellationToken);
    }

    public async Task<IReadOnlyList<AccessLogResponse>> GetLogsByEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default)
    {
        var logs = await logRepository.GetByEmployeeIdAsync(employeeId, cancellationToken);
        return logs.ToResponseList().ToList();
    }

    public async Task<IReadOnlyList<AccessLogResponse>> GetLogsByCardAsync(Guid cardId, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default)
    {
        var logs = await logRepository.GetByCardIdAsync(cardId, from, to, cancellationToken);
        return logs.ToResponseList().ToList();
    }

    public async Task<IReadOnlyList<AccessLogResponse>> GetLogsByAccessPointAsync(Guid accessPointId, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default)
    {
        var logs = await logRepository.GetByAccessPointIdAsync(accessPointId, from, to, cancellationToken);
        return logs.ToResponseList().ToList();
    }

    // Persists the access attempt result and returns the mapped response
    private async Task<AccessLogResponse> LogAndSaveAsync(
        Guid cardId, Guid accessPointId, AccessResult result, CancellationToken cancellationToken)
    {
        var log = new AccessLog(cardId, accessPointId, result);

        await logRepository.AddAsync(log, cancellationToken);
        await logRepository.SaveChangesAsync(cancellationToken);

        return log.ToResponse();
    }

    // Returns an in-memory denial response when no card ID exists to log against
    private static AccessLogResponse CreateDenialResponse(Guid accessPointId, AccessResult result) => new()
    {
        Id = Guid.Empty,
        AccessCardId = Guid.Empty,
        AccessPointId = accessPointId,
        AttemptedAt = DateTime.UtcNow,
        Result = result.ToString(),
        WasSuccessful = false,
        Notes = null
    };
}
