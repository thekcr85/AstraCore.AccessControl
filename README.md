# AstraCore Access Control API 🔐

> Physical access control REST API built with Clean Architecture in .NET 10

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-14.0-239120)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![EF Core](https://img.shields.io/badge/EF_Core-10.0-512BD4)](https://docs.microsoft.com/en-us/ef/core/)
[![FluentValidation](https://img.shields.io/badge/FluentValidation-12.0-blue)](https://docs.fluentvalidation.net/)
[![xUnit](https://img.shields.io/badge/xUnit-2.9.3-green)](https://xunit.net/)

## What This Does

A production-structured **.NET 10 REST API** for managing physical building access control — the backend that powers door scanners, employee credentials, and security audit logs.

When an employee scans their card at a door, the API:

1. Looks up the card by its number
2. Verifies the employee is active
3. Checks the card is valid and not expired
4. Checks the access point is enabled
5. Compares the card's clearance level against the door's required level
6. Logs the outcome — `Granted`, `Denied`, `InsufficientClearance`, etc.

- 🏗️ **Clean Architecture** — strict layer separation, one-way dependencies
- 🗄️ **EF Core 10** — SQL Server with Fluent API entity configuration
- 📋 **Repository + Unit of Work** — decoupled data access, single-commit transactions
- 🎯 **Domain-Driven Design** — rich entities, value objects, domain enums
- ✅ **FluentValidation 12** — cascade-stop validation on every request DTO
- 🧪 **xUnit Tests** — *(next step)*

## Tech Stack

```
.NET 10 + C# 14
ASP.NET Core Minimal API
Entity Framework Core 10 + SQL Server
FluentValidation 12
OpenAPI (Microsoft.AspNetCore.OpenApi)
xUnit
```

## Architecture

```
┌──────────────────────────────────────────────┐
│                     API                      │  Minimal API, OpenAPI, Program.cs
├──────────────────────────────────────────────┤
│               Infrastructure                 │  EF Core, SQL Server, Repositories
├──────────────────────────────────────────────┤
│                Application                   │  Services, DTOs, Validators, Mappings
├──────────────────────────────────────────────┤
│                  Domain                      │  Entities, Value Objects, Enums
└──────────────────────────────────────────────┘
```

Dependencies flow inward only. The Domain has zero dependencies.
The Application layer defines interfaces — Infrastructure implements them.

## Project Structure

```
src/
├── AstraCore.AccessControl.Domain/
│   ├── Common/         BaseEntity (Id, CreatedAt, UpdatedAt)
│   ├── Entities/       Employee, AccessCard, AccessPoint, AccessLog
│   ├── ValueObjects/   CardNumber (16-char alphanumeric, self-validating)
│   └── Enums/          AccessLevel, AccessPointType, AccessResult, EmploymentStatus
│
├── AstraCore.AccessControl.Application/
│   ├── DTOs/           Request + response records per entity
│   ├── Interfaces/     IRepository contracts, IService contracts, IUnitOfWork
│   ├── Mappings/       Entity → DTO extension methods
│   ├── Services/       EmployeeService, AccessCardService, AccessPointService, AccessValidationService
│   ├── Validators/     FluentValidation validators per request DTO (grouped by entity)
│   └── DependencyInjection.cs
│
├── AstraCore.AccessControl.Infrastructure/
│   ├── Persistence/
│   │   ├── AppDbContext.cs
│   │   ├── Configurations/   IEntityTypeConfiguration per entity (Fluent API)
│   │   ├── Repositories/     IRepository implementations
│   │   └── UnitOfWork/       Shared SaveChangesAsync across all repositories
│   └── DependencyInjection.cs
│
└── AstraCore.AccessControl.Api/              ✅ Complete
    ├── Endpoints/     EmployeeEndpoints, AccessCardEndpoints, AccessPointEndpoints, AccessValidationEndpoints
    ├── Extensions/    EndpointExtensions (reflection-based auto-discovery)
    ├── Filters/       ValidationFilter<T> (FluentValidation endpoint filter)
    └── Program.cs     Scalar, OpenAPI, AddApplication() + AddInfrastructure() + MapEndpoints()

tests/
└── AstraCore.AccessControl.Tests/            🧪 xUnit — in progress
```

## Domain Model

```
Employee ──< AccessCard ──< AccessLog >── AccessPoint
```

An `Employee` holds many `AccessCards`. Each card scan creates an `AccessLog` linked to both the card and an `AccessPoint`. The log is immutable — a permanent security record.

**Access Levels** *(hierarchical)*
```
None → Basic → Standard → Elevated → High → Critical
```

**Access Results** stored as strings in the database:
```
Granted | Denied | CardInvalid | CardExpired | InsufficientClearance | EmployeeInactive | AccessPointDisabled
```

## Key Design Patterns

### Repository + Unit of Work

Write operations only stage changes in EF Core's change tracker.
A single `SaveChangesAsync` commits everything in one transaction.

```csharp
repository.Add(employee);              // tracked in memory, no SQL yet
await unitOfWork.SaveChangesAsync(ct); // one INSERT, one transaction
```

All repositories and `UnitOfWork` share the same **Scoped** `AppDbContext` — one per HTTP request.

### Value Object

`CardNumber` is a `sealed record` wrapping a validated 16-character alphanumeric string.
EF Core stores it as `NVARCHAR(16)` via a value converter.

```csharp
var card = CardNumber.Create("ABCD12345678EFGH"); // validates on creation
var fromDb = CardNumber.FromDatabase(value);       // trusted read, skips validation
```

### FluentValidation with Cascade Stop

Every request DTO has a dedicated validator. `RuleLevelCascadeMode = CascadeMode.Stop`
stops on the first failure per property — no redundant error messages.

```csharp
public CreateEmployeeRequestValidator()
{
    RuleLevelCascadeMode = CascadeMode.Stop;

    RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email is required.")
        .MaximumLength(200).WithMessage("Email cannot exceed 200 characters.")
        .EmailAddress().WithMessage("Email format is invalid.");
}
```

Validators are auto-discovered via `AddValidatorsFromAssembly` — no manual registration.

### Access Validation Flow

```
POST /access/attempt  { cardNumber, accessPointId }
         ↓
┌──────────────────────────────────────┐
│ FluentValidation                     │
│ CardNumber: 16 alphanumeric chars?   │
│ AccessPointId: not empty Guid?       │
└──────────────────────────────────────┘
         ↓
┌──────────────────────────────────────┐
│ AccessValidationService              │
│ 1. Find card by number               │
│ 2. Check employee is Active          │
│ 3. Check card IsActive + not expired │
│ 4. Find access point, check enabled  │
│ 5. card.AccessLevel >= RequiredLevel │
│ 6. Log result, return response       │
└──────────────────────────────────────┘
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server or SQL Server LocalDB

### Setup

```bash
# 1. Clone
git clone https://github.com/thekcr85/AstraCore.AccessControl.git
cd AstraCore.AccessControl

# 2. Update connection string
# src/AstraCore.AccessControl.Api/appsettings.json
```

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AstraCoreAccessControl;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

```bash
# 3. Apply migrations
dotnet ef migrations add InitialCreate \
  --project src/AstraCore.AccessControl.Infrastructure \
  --startup-project src/AstraCore.AccessControl.Api

dotnet ef database update \
  --project src/AstraCore.AccessControl.Infrastructure \
  --startup-project src/AstraCore.AccessControl.Api

# 4. Run
dotnet run --project src/AstraCore.AccessControl.Api
```

## Development Status

| Layer | Status |
|---|---|
| ✅ Domain — Entities, Value Objects, Enums | Complete |
| ✅ Application — Services, DTOs, Validators, Mappings | Complete |
| ✅ Infrastructure — EF Core, Repositories, Unit of Work | Complete |
| ✅ API — 27 endpoints, ValidationFilter, Scalar, auto-discovery | Complete |
| 🔧 Global Error Handling — `IExceptionHandler` middleware | **Next step** |
| 🔧 xUnit Tests — Domain, Application, Validators | **Next step** |
| 🔧 Docker — containerize API + SQL Server | **Planned** |

## Next Steps

### 🔧 Global Error Handling

Replace inline `try/catch` in every endpoint handler with a single `IExceptionHandler` middleware.
Each handler becomes a clean one-liner:

```csharp
// Before — catch in every handler
private static async Task<IResult> GetById(Guid id, IEmployeeService service, CancellationToken ct)
{
    try { return Results.Ok(await service.GetByIdAsync(id, ct)); }
    catch (KeyNotFoundException) { return Results.NotFound(); }
}

// After — global handler takes care of it
private static async Task<IResult> GetById(Guid id, IEmployeeService service, CancellationToken ct)
    => Results.Ok(await service.GetByIdAsync(id, ct));
```

---

### 🧪 xUnit Tests

```
tests/
└── AstraCore.AccessControl.Tests/
      ├── Domain/
      │     ├── EmployeeTests.cs         Activate/Suspend/Terminate state transitions
      │     ├── AccessCardTests.cs       IsExpired, IsValid, ExtendExpiry logic
      │     └── CardNumberTests.cs       Create() validation — length, alphanumeric, casing
      ├── Application/
      │     ├── EmployeeServiceTests.cs          CreateAsync duplicate email, UpdateAsync not found
      │     ├── AccessCardServiceTests.cs         ReactivateAsync conflict, ChangeLevel
      │     ├── AccessPointServiceTests.cs        Enable/Disable, ChangeRequiredLevel
      │     └── AccessValidationServiceTests.cs   All 7 AccessResult outcomes
      └── Validators/
            ├── CreateEmployeeRequestValidatorTests.cs
            ├── CreateAccessCardRequestValidatorTests.cs
            ├── CreateAccessPointRequestValidatorTests.cs
            └── AccessAttemptRequestValidatorTests.cs
```

Domain tests require **no mocks** — pure C# logic.
Application tests mock `IEmployeeRepository`, `IUnitOfWork`, etc. using `NSubstitute`.

---

### 🐳 Docker Containerization

```
Dockerfile               Multi-stage build — SDK image to build, runtime image to run
docker-compose.yml       API container + SQL Server container, shared network
.dockerignore            Exclude bin/, obj/, .env from image
```

**Planned `docker-compose.yml` services:**

```yaml
services:
  api:
    build: .
    ports:
      - "8080:8080"
    environment:
      - ConnectionStrings__DefaultConnection=Server=db;Database=AstraCore;...
    depends_on:
      - db

  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourStrong@Passw0rd
```

Once containerized, the full stack runs with a single command:

```bash
docker compose up --build
```

## NuGet Packages

### Application
```xml
<PackageReference Include="FluentValidation" Version="12.1.1" />
<PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="12.1.1" />
```

### Infrastructure
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.5" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.5" />
```

### API
```xml
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="10.0.5" />
<PackageReference Include="Scalar.AspNetCore" Version="2.5.x" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.5" />
```

### Tests
```xml
<PackageReference Include="xunit" Version="2.9.3" />
<PackageReference Include="xunit.runner.visualstudio" Version="3.1.4" />
<PackageReference Include="coverlet.collector" Version="6.0.4" />
```

## Architecture Highlights

✅ **Clean Architecture** — testable, maintainable, strict layer separation
✅ **Repository Pattern** — data access decoupled from business logic
✅ **Unit of Work** — all repositories commit in a single transaction
✅ **Value Objects** — `CardNumber` encapsulates validation and equality
✅ **FluentValidation** — cascade-stop validators, auto-discovered from assembly
✅ **Enum-as-string storage** — self-documenting database columns
✅ **EF Core Fluent API** — entity configurations isolated per file
✅ **C# 14** — primary constructors, records, collection expressions

## Author

**Michał Bąkiewicz** • [GitHub](https://github.com/thekcr85)

---

**Project Repository**: [github.com/thekcr85/AstraCore.AccessControl](https://github.com/thekcr85/AstraCore.AccessControl)

---

## License

MIT License

---

**Status**: Active development 🚧