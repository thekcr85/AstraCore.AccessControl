using AstraCore.AccessControl.Application.DTOs.AccessLog;

namespace AstraCore.AccessControl.Application.Interfaces.Services;

public interface IAccessValidationService
{
	Task<AccessLogResponse> ValidateAccessAsync(AccessAttemptRequest request, CancellationToken cancellationToken = default);
	Task<IReadOnlyList<AccessLogResponse>> GetLogsByEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default);
	Task<IReadOnlyList<AccessLogResponse>> GetLogsByCardAsync(Guid cardId, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default);
	Task<IReadOnlyList<AccessLogResponse>> GetLogsByAccessPointAsync(Guid accessPointId, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default);
}