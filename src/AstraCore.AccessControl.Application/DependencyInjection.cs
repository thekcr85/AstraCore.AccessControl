using AstraCore.AccessControl.Application.Interfaces.Services;
using AstraCore.AccessControl.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AstraCore.AccessControl.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IAccessCardService, AccessCardService>();
        services.AddScoped<IAccessPointService, AccessPointService>();
        services.AddScoped<IAccessValidationService, AccessValidationService>();

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}
