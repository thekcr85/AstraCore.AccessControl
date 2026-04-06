using AstraCore.AccessControl.Application.Interfaces.Repositories;
using AstraCore.AccessControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AstraCore.AccessControl.Infrastructure.Persistence.Repositories;

internal sealed class AccessPointRepository(AppDbContext context) : IAccessPointRepository
{
    public async Task<AccessPoint?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.AccessPoints.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<AccessPoint>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.AccessPoints.ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<AccessPoint>> GetEnabledAsync(CancellationToken cancellationToken = default)
        => await context.AccessPoints
            .Where(ap => ap.IsEnabled)
            .ToListAsync(cancellationToken);

    public void Add(AccessPoint accessPoint)
        => context.AccessPoints.Add(accessPoint);

    public void Update(AccessPoint accessPoint)
        => context.AccessPoints.Update(accessPoint);

    public void Remove(AccessPoint accessPoint)
        => context.AccessPoints.Remove(accessPoint);
}
