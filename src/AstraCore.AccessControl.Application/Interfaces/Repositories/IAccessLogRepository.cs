using AstraCore.AccessControl.Domain.Entities;

namespace AstraCore.AccessControl.Application.Interfaces.Repositories;

public interface IAccessLogRepository
{
    Task<AccessLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AccessLog>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AccessLog>> GetByCardIdAsync(Guid cardId, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AccessLog>> GetByAccessPointIdAsync(Guid accessPointId, DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default);
    void Add(AccessLog log);
}
