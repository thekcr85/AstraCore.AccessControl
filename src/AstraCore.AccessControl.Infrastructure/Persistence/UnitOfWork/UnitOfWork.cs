using AstraCore.AccessControl.Application.Interfaces.UnitOfWork;

namespace AstraCore.AccessControl.Infrastructure.Persistence.UnitOfWork;

internal sealed class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);
}
