using AstraCore.AccessControl.Domain.Entities;

namespace AstraCore.AccessControl.Application.Interfaces.Repositories;

public interface IAccessCardRepository
{
    Task<AccessCard?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AccessCard?> GetByCardNumberAsync(string cardNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AccessCard>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default);
    void Add(AccessCard card);
    void Update(AccessCard card);
    void Remove(AccessCard card);
}
