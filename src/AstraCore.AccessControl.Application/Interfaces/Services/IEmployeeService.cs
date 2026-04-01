using AstraCore.AccessControl.Application.DTOs.Employee;

namespace AstraCore.AccessControl.Application.Interfaces.Services;

public interface IEmployeeService
{
	Task<EmployeeResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
	Task<IReadOnlyList<EmployeeSummaryResponse>> GetAllAsync(CancellationToken cancellationToken = default);
	Task<IReadOnlyList<EmployeeSummaryResponse>> GetByDepartmentAsync(string department, CancellationToken cancellationToken = default);
	Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request, CancellationToken cancellationToken = default);
	Task<EmployeeResponse> UpdateAsync(Guid id, UpdateEmployeeRequest request, CancellationToken cancellationToken = default);
	Task ActivateAsync(Guid id, CancellationToken cancellationToken = default);
	Task SuspendAsync(Guid id, CancellationToken cancellationToken = default);
	Task TerminateAsync(Guid id, CancellationToken cancellationToken = default);
	Task SetOnLeaveAsync(Guid id, CancellationToken cancellationToken = default);
}
