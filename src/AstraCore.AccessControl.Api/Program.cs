using AstraCore.AccessControl.Application;
using AstraCore.AccessControl.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// TODO: Add endpoint mappings for all services (Employee, AccessCard, AccessPoint, AccessValidation)
// Endpoints will be implemented in a structured Endpoints/ folder

app.Run();