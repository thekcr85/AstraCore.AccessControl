using AstraCore.AccessControl.Application.DTOs.AccessCard;
using AstraCore.AccessControl.Domain.Enums;

namespace AstraCore.AccessControl.Application.Interfaces.Services;

public interface IAccessCardService
{
	Task<AccessCardResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
	Task<IReadOnlyList<AccessCardResponse>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default);
	Task<AccessCardResponse> CreateAsync(CreateAccessCardRequest request, CancellationToken cancellationToken = default);
	Task DeactivateAsync(Guid id, CancellationToken cancellationToken = default);
	Task ReactivateAsync(Guid id, CancellationToken cancellationToken = default);
	Task<AccessCardResponse> ExtendExpiryAsync(Guid id, DateTime newExpiryDate, CancellationToken cancellationToken = default);
	Task<AccessCardResponse> ChangeAccessLevelAsync(Guid id, AccessLevel newLevel, CancellationToken cancellationToken = default);
}