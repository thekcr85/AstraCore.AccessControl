using AstraCore.AccessControl.Application.DTOs.AccessPoint;
using AstraCore.AccessControl.Domain.Enums;

namespace AstraCore.AccessControl.Application.Interfaces.Services;

public interface IAccessPointService
{
	Task<AccessPointResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
	Task<IReadOnlyList<AccessPointResponse>> GetAllAsync(CancellationToken cancellationToken = default);
	Task<AccessPointResponse> CreateAsync(CreateAccessPointRequest request, CancellationToken cancellationToken = default);
	Task<AccessPointResponse> UpdateAsync(Guid id, UpdateAccessPointRequest request, CancellationToken cancellationToken = default);
	Task EnableAsync(Guid id, CancellationToken cancellationToken = default);
	Task DisableAsync(Guid id, CancellationToken cancellationToken = default);
	Task<AccessPointResponse> ChangeRequiredAccessLevelAsync(Guid id, AccessLevel newLevel, CancellationToken cancellationToken = default);
}