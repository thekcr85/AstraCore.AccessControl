using AstraCore.AccessControl.Domain.Entities;

namespace AstraCore.AccessControl.Application.Interfaces.Repositories;

public interface IAccessPointRepository
{
    Task<AccessPoint?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AccessPoint>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AccessPoint>> GetEnabledAsync(CancellationToken cancellationToken = default);
    void Add(AccessPoint accessPoint);
    void Update(AccessPoint accessPoint);
    void Remove(AccessPoint accessPoint);
}
