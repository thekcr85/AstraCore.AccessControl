using AstraCore.AccessControl.Application.Interfaces.Repositories;
using AstraCore.AccessControl.Domain.Entities;
using AstraCore.AccessControl.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AstraCore.AccessControl.Infrastructure.Persistence.Repositories;

internal sealed class AccessCardRepository(AppDbContext context) : IAccessCardRepository
{
    public async Task<AccessCard?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.AccessCards.FindAsync([id], cancellationToken);

    public async Task<AccessCard?> GetByCardNumberAsync(string cardNumber, CancellationToken cancellationToken = default)
    {
        var cardNumberValue = CardNumber.Create(cardNumber);
        return await context.AccessCards.FirstOrDefaultAsync(c => c.CardNumber == cardNumberValue, cancellationToken);
    }

    public async Task<IReadOnlyList<AccessCard>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default)
        => await context.AccessCards
            .Where(c => c.EmployeeId == employeeId)
            .ToListAsync(cancellationToken);

    public void Add(AccessCard card)
        => context.AccessCards.Add(card);

    public void Update(AccessCard card)
        => context.AccessCards.Update(card);

    public void Remove(AccessCard card)
        => context.AccessCards.Remove(card);
}
