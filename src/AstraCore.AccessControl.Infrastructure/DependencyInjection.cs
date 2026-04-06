using AstraCore.AccessControl.Application.Interfaces.Repositories;
using AstraCore.AccessControl.Application.Interfaces.UnitOfWork;
using AstraCore.AccessControl.Infrastructure.Persistence;
using AstraCore.AccessControl.Infrastructure.Persistence.Repositories;
using AstraCore.AccessControl.Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AstraCore.AccessControl.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("DefaultConnection")
			?? throw new InvalidOperationException(
				"Connection string 'DefaultConnection' is not configured.");

		services.AddDbContext<AppDbContext>(options =>
			options.UseSqlServer(connectionString));

		services.AddScoped<IUnitOfWork, UnitOfWork>();

		services.AddScoped<IEmployeeRepository, EmployeeRepository>();
		services.AddScoped<IAccessCardRepository, AccessCardRepository>();
		services.AddScoped<IAccessPointRepository, AccessPointRepository>();
		services.AddScoped<IAccessLogRepository, AccessLogRepository>();

		return services;
	}
}
