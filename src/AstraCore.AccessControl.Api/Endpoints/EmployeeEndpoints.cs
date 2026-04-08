using AstraCore.AccessControl.Api.Filters;
using AstraCore.AccessControl.Application.DTOs.Employee;
using AstraCore.AccessControl.Application.Interfaces.Services;

namespace AstraCore.AccessControl.Api.Endpoints;

public sealed class EmployeeEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/employees").WithTags("Employees");

        group.MapGet("/", GetAll)
            .WithSummary("Get all employees");

        group.MapGet("/{id:guid}", GetById)
            .WithSummary("Get employee by ID");

        group.MapGet("/department/{department}", GetByDepartment)
            .WithSummary("Get employees by department");

        group.MapPost("/", Create)
            .WithSummary("Create a new employee")
            .AddEndpointFilter<ValidationFilter<CreateEmployeeRequest>>();

        group.MapPut("/{id:guid}", Update)
            .WithSummary("Update employee contact info")
            .AddEndpointFilter<ValidationFilter<UpdateEmployeeRequest>>();

        group.MapPatch("/{id:guid}/activate", Activate)
            .WithSummary("Activate employee");

        group.MapPatch("/{id:guid}/suspend", Suspend)
            .WithSummary("Suspend employee");

        group.MapPatch("/{id:guid}/terminate", Terminate)
            .WithSummary("Terminate employee");

        group.MapPatch("/{id:guid}/leave", SetOnLeave)
            .WithSummary("Set employee on leave");
    }

    private static async Task<IResult> GetAll(
        IEmployeeService service,
        CancellationToken ct)
    {
        var result = await service.GetAllAsync(ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetById(
        Guid id,
        IEmployeeService service,
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

    private static async Task<IResult> GetByDepartment(
        string department,
        IEmployeeService service,
        CancellationToken ct)
    {
        var result = await service.GetByDepartmentAsync(department, ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> Create(
        CreateEmployeeRequest request,
        IEmployeeService service,
        CancellationToken ct)
    {
        try
        {
            var result = await service.CreateAsync(request, ct);
            return Results.Created($"/api/employees/{result.Id}", result);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(ex.Message);
        }
    }

    private static async Task<IResult> Update(
        Guid id,
        UpdateEmployeeRequest request,
        IEmployeeService service,
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
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(ex.Message);
        }
    }

    private static async Task<IResult> Activate(
        Guid id,
        IEmployeeService service,
        CancellationToken ct)
    {
        try
        {
            await service.ActivateAsync(id, ct);
            return Results.NoContent();
        }
        catch (KeyNotFoundException)
        {
            return Results.NotFound();
        }
    }

    private static async Task<IResult> Suspend(
        Guid id,
        IEmployeeService service,
        CancellationToken ct)
    {
        try
        {
            await service.SuspendAsync(id, ct);
            return Results.NoContent();
        }
        catch (KeyNotFoundException)
        {
            return Results.NotFound();
        }
    }

    private static async Task<IResult> Terminate(
        Guid id,
        IEmployeeService service,
        CancellationToken ct)
    {
        try
        {
            await service.TerminateAsync(id, ct);
            return Results.NoContent();
        }
        catch (KeyNotFoundException)
        {
            return Results.NotFound();
        }
    }

    private static async Task<IResult> SetOnLeave(
        Guid id,
        IEmployeeService service,
        CancellationToken ct)
    {
        try
        {
            await service.SetOnLeaveAsync(id, ct);
            return Results.NoContent();
        }
        catch (KeyNotFoundException)
        {
            return Results.NotFound();
        }
    }
}
