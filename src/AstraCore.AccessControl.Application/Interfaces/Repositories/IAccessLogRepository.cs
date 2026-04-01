using AstraCore.AccessControl.Domain.Entities;

namespace AstraCore.AccessControl.Application.Interfaces.Repositories;

public interface IAccessLogRepository
{
    Task<AccessLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AccessLog>> GetByCardIdAsync(Guid cardId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AccessLog>> GetByAccessPointIdAsync(Guid accessPointId, CancellationToken cancellationToken = default);
    Task AddAsync(AccessLog log, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
