using AstraCore.AccessControl.Application.Interfaces.Repositories;
using AstraCore.AccessControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AstraCore.AccessControl.Infrastructure.Persistence.Repositories;

internal sealed class EmployeeRepository(AppDbContext context) : IEmployeeRepository
{
	public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
		=> await context.Employees.FindAsync([id], cancellationToken);

	public async Task<Employee?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
		=> await context.Employees.FirstOrDefaultAsync(e => e.Email == email, cancellationToken);

	public async Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
		=> await context.Employees.ToListAsync(cancellationToken);

	public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
		=> await context.Employees.AnyAsync(e => e.Email == email, cancellationToken);

	public void Add(Employee employee)
		=> context.Employees.Add(employee);

	public void Update(Employee employee)
		=> context.Employees.Update(employee);

	public void Remove(Employee employee)
		=> context.Employees.Remove(employee);
}