using AstraCore.AccessControl.Application.DTOs.Employee;
using AstraCore.AccessControl.Application.Interfaces.Repositories;
using AstraCore.AccessControl.Application.Interfaces.Services;
using AstraCore.AccessControl.Application.Interfaces.UnitOfWork;
using AstraCore.AccessControl.Application.Mappings;
using AstraCore.AccessControl.Domain.Entities;

namespace AstraCore.AccessControl.Application.Services;

public sealed class EmployeeService(
    IEmployeeRepository repository,
    IUnitOfWork unitOfWork) : IEmployeeService
{
    public async Task<EmployeeResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var employee = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Employee {id} was not found.");

        return employee.ToResponse();
    }

    public async Task<IReadOnlyList<EmployeeSummaryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var employees = await repository.GetAllAsync(cancellationToken);
        return employees.ToSummaryResponseList().ToList();
    }

    public async Task<IReadOnlyList<EmployeeSummaryResponse>> GetByDepartmentAsync(string department, CancellationToken cancellationToken = default)
    {
        var employees = await repository.GetAllAsync(cancellationToken);

        return employees
            .Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase))
            .ToSummaryResponseList()
            .ToList();
    }

    public async Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request, CancellationToken cancellationToken = default)
    {
        if (await repository.ExistsByEmailAsync(request.Email, cancellationToken))
            throw new InvalidOperationException($"Email '{request.Email}' is already in use.");

        var employee = new Employee(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Department,
            request.HireDate,
            request.Position);

        repository.Add(employee);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return employee.ToResponse();
    }

    public async Task<EmployeeResponse> UpdateAsync(Guid id, UpdateEmployeeRequest request, CancellationToken cancellationToken = default)
    {
        var employee = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Employee {id} was not found.");

        if (!employee.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)
            && await repository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            throw new InvalidOperationException($"Email '{request.Email}' is already in use.");
        }

        employee.UpdateContactInfo(request.Email, request.Department);
        employee.UpdatePosition(request.Position);

        repository.Update(employee);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return employee.ToResponse();
    }

    public async Task ActivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var employee = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Employee {id} was not found.");

        employee.Activate();
        repository.Update(employee);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task SuspendAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var employee = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Employee {id} was not found.");

        employee.Suspend();
        repository.Update(employee);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task TerminateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var employee = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Employee {id} was not found.");

        employee.Terminate();
        repository.Update(employee);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task SetOnLeaveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var employee = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Employee {id} was not found.");

        employee.SetOnLeave();
        repository.Update(employee);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
