using AstraCore.AccessControl.Api.Filters;
using AstraCore.AccessControl.Application.DTOs.AccessPoint;
using AstraCore.AccessControl.Application.Interfaces.Services;
using AstraCore.AccessControl.Domain.Enums;

namespace AstraCore.AccessControl.Api.Endpoints;

public sealed class AccessPointEndpoints : IEndpoint
{
    private sealed record ChangeRequiredLevelRequest(AccessLevel NewLevel);

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/access-points").WithTags("Access Points");

        group.MapGet("/", GetAll)
            .WithSummary("Get all access points");

        group.MapGet("/{id:guid}", GetById)
            .WithSummary("Get access point by ID");

        group.MapPost("/", Create)
            .WithSummary("Create a new access point")
            .AddEndpointFilter<ValidationFilter<CreateAccessPointRequest>>();

        group.MapPut("/{id:guid}", Update)
            .WithSummary("Update access point details")
            .AddEndpointFilter<ValidationFilter<UpdateAccessPointRequest>>();

        group.MapPatch("/{id:guid}/enable", Enable)
            .WithSummary("Enable an access point");

        group.MapPatch("/{id:guid}/disable", Disable)
            .WithSummary("Disable an access point");

        group.MapPatch("/{id:guid}/level", ChangeLevel)
            .WithSummary("Change required access level");
    }

    private static async Task<IResult> GetAll(
        IAccessPointService service,
        CancellationToken ct)
    {
        var result = await service.GetAllAsync(ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetById(
        Guid id,
        IAccessPointService service,
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

    private static async Task<IResult> Create(
        CreateAccessPointRequest request,
        IAccessPointService service,
        CancellationToken ct)
    {
        var result = await service.CreateAsync(request, ct);
        return Results.Created($"/api/access-points/{result.Id}", result);
    }

    private static async Task<IResult> Update(
        Guid id,
        UpdateAccessPointRequest request,
        IAccessPointService service,
        CancellationToken ct)
    {
        try
        {
            var result = await service.UpdateAsync(id, request, ct);
            return Results.Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return Results.NotFound();
        }
    }

    private static async Task<IResult> Enable(
        Guid id,
        IAccessPointService service,
        CancellationToken ct)
    {
        try
        {
            await service.EnableAsync(id, ct);
            return Results.NoContent();
        }
        catch (KeyNotFoundException)
        {
            return Results.NotFound();
        }
    }

    private static async Task<IResult> Disable(
        Guid id,
        IAccessPointService service,
        CancellationToken ct)
    {
        try
        {
            await service.DisableAsync(id, ct);
            return Results.NoContent();
        }
        catch (KeyNotFoundException)
        {
            return Results.NotFound();
        }
    }

    private static async Task<IResult> ChangeLevel(
        Guid id,
        ChangeRequiredLevelRequest request,
        IAccessPointService service,
        CancellationToken ct)
    {
        try
        {
            var result = await service.ChangeRequiredAccessLevelAsync(id, request.NewLevel, ct);
            return Results.Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return Results.NotFound();
        }
    }
}
