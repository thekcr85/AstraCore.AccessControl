using AstraCore.AccessControl.Api.Filters;
using AstraCore.AccessControl.Application.DTOs.AccessCard;
using AstraCore.AccessControl.Application.Interfaces.Services;
using AstraCore.AccessControl.Domain.Enums;

namespace AstraCore.AccessControl.Api.Endpoints;

public sealed class AccessCardEndpoints : IEndpoint
{
    private sealed record ExtendExpiryRequest(DateTime NewExpiryDate);
    private sealed record ChangeAccessLevelRequest(AccessLevel NewLevel);

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/access-cards").WithTags("Access Cards");

        group.MapGet("/{id:guid}", GetById)
            .WithSummary("Get access card by ID");

        group.MapGet("/employee/{employeeId:guid}", GetByEmployee)
            .WithSummary("Get all cards for an employee");

        group.MapPost("/", Create)
            .WithSummary("Issue a new access card")
            .AddEndpointFilter<ValidationFilter<CreateAccessCardRequest>>();

        group.MapPatch("/{id:guid}/deactivate", Deactivate)
            .WithSummary("Deactivate an access card");

        group.MapPatch("/{id:guid}/reactivate", Reactivate)
            .WithSummary("Reactivate an access card");

        group.MapPatch("/{id:guid}/expiry", ExtendExpiry)
            .WithSummary("Extend card expiry date");

        group.MapPatch("/{id:guid}/level", ChangeLevel)
            .WithSummary("Change card access level");
    }

    private static async Task<IResult> GetById(
        Guid id,
        IAccessCardService service,
        CancellationToken ct)
    {
        try
        {
            var result = await service.GetByIdAsync(id, ct);
            return Results.Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return Results.NotFound();
        }
    }

    private static async Task<IResult> GetByEmployee(
        Guid employeeId,
        IAccessCardService service,
        CancellationToken ct)
    {
        var result = await service.GetByEmployeeIdAsync(employeeId, ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> Create(
        CreateAccessCardRequest request,
        IAccessCardService service,
        CancellationToken ct)
    {
        try
        {
            var result = await service.CreateAsync(request, ct);
            return Results.Created($"/api/access-cards/{result.Id}", result);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(ex.Message);
        }
    }

    private static async Task<IResult> Deactivate(
        Guid id,
        IAccessCardService service,
        CancellationToken ct)
    {
        try
        {
            await service.DeactivateAsync(id, ct);
            return Results.NoContent();
        }
        catch (KeyNotFoundException)
        {
            return Results.NotFound();
        }
    }

    private static async Task<IResult> Reactivate(
        Guid id,
        IAccessCardService service,
        CancellationToken ct)
    {
        try
        {
            await service.ReactivateAsync(id, ct);
            return Results.NoContent();
        }
        catch (KeyNotFoundException)
        {
            return Results.NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(ex.Message);
        }
    }

    private static async Task<IResult> ExtendExpiry(
        Guid id,
        ExtendExpiryRequest request,
        IAccessCardService service,
        CancellationToken ct)
    {
        try
        {
            var result = await service.ExtendExpiryAsync(id, request.NewExpiryDate, ct);
            return Results.Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return Results.NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(ex.Message);
        }
    }

    private static async Task<IResult> ChangeLevel(
        Guid id,
        ChangeAccessLevelRequest request,
        IAccessCardService service,
        CancellationToken ct)
    {
        try
        {
            var result = await service.ChangeAccessLevelAsync(id, request.NewLevel, ct);
            return Results.Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return Results.NotFound();
        }
    }
}
