using AstraCore.AccessControl.Application.DTOs.AccessPoint;
using AstraCore.AccessControl.Application.Interfaces.Repositories;
using AstraCore.AccessControl.Application.Interfaces.Services;
using AstraCore.AccessControl.Application.Mappings;
using AstraCore.AccessControl.Domain.Entities;
using AstraCore.AccessControl.Domain.Enums;

namespace AstraCore.AccessControl.Application.Services;

public sealed class AccessPointService(IAccessPointRepository repository) : IAccessPointService
{
    public async Task<AccessPointResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var accessPoint = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Access point {id} was not found.");

        return accessPoint.ToResponse();
    }

    public async Task<IReadOnlyList<AccessPointResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var accessPoints = await repository.GetAllAsync(cancellationToken);
        return accessPoints.ToResponseList().ToList();
    }

    public async Task<AccessPointResponse> CreateAsync(CreateAccessPointRequest request, CancellationToken cancellationToken = default)
    {
        var accessPoint = new AccessPoint(
            request.Name,
            request.Location,
            request.Type,
            request.RequiredAccessLevel,
            request.Description);

        await repository.AddAsync(accessPoint, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return accessPoint.ToResponse();
    }

    public async Task<AccessPointResponse> UpdateAsync(Guid id, UpdateAccessPointRequest request, CancellationToken cancellationToken = default)
    {
        var accessPoint = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Access point {id} was not found.");

        accessPoint.UpdateDetails(request.Name, request.Location, request.Description);
        accessPoint.ChangeRequiredAccessLevel(request.RequiredAccessLevel);

        repository.Update(accessPoint);
        await repository.SaveChangesAsync(cancellationToken);

        return accessPoint.ToResponse();
    }

    public async Task EnableAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var accessPoint = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Access point {id} was not found.");

        accessPoint.Enable();
        repository.Update(accessPoint);
        await repository.SaveChangesAsync(cancellationToken);
    }

    public async Task DisableAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var accessPoint = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Access point {id} was not found.");

        accessPoint.Disable();
        repository.Update(accessPoint);
        await repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<AccessPointResponse> ChangeRequiredAccessLevelAsync(Guid id, AccessLevel newLevel, CancellationToken cancellationToken = default)
    {
        var accessPoint = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Access point {id} was not found.");

        accessPoint.ChangeRequiredAccessLevel(newLevel);
        repository.Update(accessPoint);
        await repository.SaveChangesAsync(cancellationToken);

        return accessPoint.ToResponse();
    }
}
