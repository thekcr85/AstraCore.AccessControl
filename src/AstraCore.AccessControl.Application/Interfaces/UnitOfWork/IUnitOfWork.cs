namespace AstraCore.AccessControl.Application.Interfaces.UnitOfWork;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
