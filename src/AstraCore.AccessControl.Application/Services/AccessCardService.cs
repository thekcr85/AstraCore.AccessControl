using AstraCore.AccessControl.Application.DTOs.AccessCard;
using AstraCore.AccessControl.Application.Interfaces.Repositories;
using AstraCore.AccessControl.Application.Interfaces.Services;
using AstraCore.AccessControl.Application.Interfaces.UnitOfWork;
using AstraCore.AccessControl.Application.Mappings;
using AstraCore.AccessControl.Domain.Entities;
using AstraCore.AccessControl.Domain.Enums;

namespace AstraCore.AccessControl.Application.Services;

public sealed class AccessCardService(
    IAccessCardRepository repository,
    IUnitOfWork unitOfWork) : IAccessCardService
{
    public async Task<AccessCardResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var card = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Access card {id} was not found.");

        return card.ToResponse();
    }

    public async Task<IReadOnlyList<AccessCardResponse>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default)
    {
        var cards = await repository.GetByEmployeeIdAsync(employeeId, cancellationToken);
        return cards.ToResponseList().ToList();
    }

    public async Task<AccessCardResponse> CreateAsync(CreateAccessCardRequest request, CancellationToken cancellationToken = default)
    {
        var card = new AccessCard(
            request.CardNumber,
            request.AccessLevel,
            request.EmployeeId,
            request.ExpiryDate);

        repository.Add(card);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return card.ToResponse();
    }

    public async Task DeactivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var card = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Access card {id} was not found.");

        card.Deactivate();
        repository.Update(card);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ReactivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var card = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Access card {id} was not found.");

        card.Reactivate();
        repository.Update(card);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<AccessCardResponse> ExtendExpiryAsync(Guid id, DateTime newExpiryDate, CancellationToken cancellationToken = default)
    {
        var card = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Access card {id} was not found.");

        card.ExtendExpiry(newExpiryDate);
        repository.Update(card);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return card.ToResponse();
    }

    public async Task<AccessCardResponse> ChangeAccessLevelAsync(Guid id, AccessLevel newLevel, CancellationToken cancellationToken = default)
    {
        var card = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Access card {id} was not found.");

        card.ChangeAccessLevel(newLevel);
        repository.Update(card);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return card.ToResponse();
    }
}
