using AstraCore.AccessControl.Domain.Entities;

namespace AstraCore.AccessControl.Application.Interfaces.Repositories;

public interface IAccessCardRepository
{
    Task<AccessCard?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AccessCard?> GetByCardNumberAsync(string cardNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AccessCard>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task AddAsync(AccessCard card, CancellationToken cancellationToken = default);
    void Update(AccessCard card);
    void Remove(AccessCard card);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
