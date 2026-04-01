using AstraCore.AccessControl.Domain.Entities;

namespace AstraCore.AccessControl.Application.Interfaces.Repositories;

public interface IAccessPointRepository
{
    Task<AccessPoint?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AccessPoint>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AccessPoint>> GetEnabledAsync(CancellationToken cancellationToken = default);
    Task AddAsync(AccessPoint accessPoint, CancellationToken cancellationToken = default);
    void Update(AccessPoint accessPoint);
    void Remove(AccessPoint accessPoint);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
