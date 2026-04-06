using AstraCore.AccessControl.Application.Interfaces.Repositories;
using AstraCore.AccessControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AstraCore.AccessControl.Infrastructure.Persistence.Repositories;

internal sealed class AccessLogRepository(AppDbContext context) : IAccessLogRepository
{
    public async Task<AccessLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.AccessLogs.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<AccessLog>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default)
        => await context.AccessLogs
            .Where(l => l.AccessCard.EmployeeId == employeeId)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<AccessLog>> GetByCardIdAsync(
        Guid cardId, DateTime? from = null, DateTime? to = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.AccessLogs.Where(l => l.AccessCardId == cardId);

        if (from.HasValue)
            query = query.Where(l => l.AttemptedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(l => l.AttemptedAt <= to.Value);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AccessLog>> GetByAccessPointIdAsync(
        Guid accessPointId, DateTime? from = null, DateTime? to = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.AccessLogs.Where(l => l.AccessPointId == accessPointId);

        if (from.HasValue)
            query = query.Where(l => l.AttemptedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(l => l.AttemptedAt <= to.Value);

        return await query.ToListAsync(cancellationToken);
    }

    public void Add(AccessLog log)
        => context.AccessLogs.Add(log);
}
