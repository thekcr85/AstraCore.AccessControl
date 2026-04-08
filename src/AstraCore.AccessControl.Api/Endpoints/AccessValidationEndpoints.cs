using AstraCore.AccessControl.Api.Filters;
using AstraCore.AccessControl.Application.DTOs.AccessLog;
using AstraCore.AccessControl.Application.Interfaces.Services;

namespace AstraCore.AccessControl.Api.Endpoints;

public sealed class AccessValidationEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/access").WithTags("Access Validation");

        group.MapPost("/attempt", ValidateAccess)
            .WithSummary("Validate a card scan at an access point")
            .AddEndpointFilter<ValidationFilter<AccessAttemptRequest>>();

        group.MapGet("/logs/employee/{employeeId:guid}", GetLogsByEmployee)
            .WithSummary("Get all access logs for an employee");

        group.MapGet("/logs/card/{cardId:guid}", GetLogsByCard)
            .WithSummary("Get access logs for a card with optional date range");

        group.MapGet("/logs/point/{pointId:guid}", GetLogsByPoint)
            .WithSummary("Get access logs for an access point with optional date range");
    }

    private static async Task<IResult> ValidateAccess(
        AccessAttemptRequest request,
        IAccessValidationService service,
        CancellationToken ct)
    {
        var result = await service.ValidateAccessAsync(request, ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetLogsByEmployee(
        Guid employeeId,
        IAccessValidationService service,
        CancellationToken ct)
    {
        var result = await service.GetLogsByEmployeeAsync(employeeId, ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetLogsByCard(
        Guid cardId,
        IAccessValidationService service,
        CancellationToken ct,
        DateTime? from = null,
        DateTime? to = null)
    {
        var result = await service.GetLogsByCardAsync(cardId, from, to, ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetLogsByPoint(
        Guid pointId,
        IAccessValidationService service,
        CancellationToken ct,
        DateTime? from = null,
        DateTime? to = null)
    {
        var result = await service.GetLogsByAccessPointAsync(pointId, from, to, ct);
        return Results.Ok(result);
    }
}
