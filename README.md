# Library Management API — Project Setup

## Structure

One Web API project, organized into folders by layer (simple N-Tier, not split into
separate class libraries):

```
LibraryManagement.sln
LibraryManagement.API/
  Controllers/          → one controller per module (placeholder for now)
  Services/              → business logic
    Interfaces/
  Repositories/          → data access (generic Repository<T> + module-specific repos)
    Interfaces/
  Data/
    AppDbContext.cs      → extends IdentityDbContext<ApplicationUser>
    Configurations/      → optional: split Fluent API config out of AppDbContext per entity
    Migrations/          → EF Core migrations land here
  Models/
    Entities/            → Book, Member, Loan, Reservation, Fine, Payment, etc.
    Enums/               → CopyStatus, LoanStatus, ReservationStatus, FineStatus, ...
    Common/              → PagedResult, PagedRequest
  DTOs/                  → one subfolder per module (Auth, Books, Loans, ...)
  Middleware/            → global exception handling
  Common/                → ApiResponse<T> wrapper
  Helpers/               → misc utilities (JWT token generation, etc. - add as needed)
  Program.cs
  appsettings.json
```

Everything lives in one project (`LibraryManagement.API`). No inter-project
references to manage day to day — just don't create circular folder dependencies
(e.g. a Repository shouldn't call back into a Controller).

## Prerequisites

- .NET 10 SDK ([download](https://dotnet.microsoft.com/download))
- SQL Server (LocalDB, Express, or full) — update `ConnectionStrings:DefaultConnection`
  in `appsettings.json`
- (Optional) EF Core CLI tool: `dotnet tool install --global dotnet-ef`

## First-time setup

All NuGet packages are already declared in `LibraryManagement.API.csproj` as
`PackageReference` entries — restoring pulls them in automatically:

```bash
cd LibraryManagement
dotnet restore
dotnet build
```

If a version fails to resolve against your installed SDK, re-run without a version
pin: `dotnet add package <Name>` grabs the latest compatible version.

### Packages already wired in

| Purpose               | Package                                                                       |
| --------------------- | ----------------------------------------------------------------------------- |
| JWT auth              | `Microsoft.AspNetCore.Authentication.JwtBearer`                               |
| Identity + EF         | `Microsoft.AspNetCore.Identity.EntityFrameworkCore`                           |
| EF Core / SQL Server  | `Microsoft.EntityFrameworkCore.SqlServer`                                     |
| EF migrations tooling | `Microsoft.EntityFrameworkCore.Tools`, `Microsoft.EntityFrameworkCore.Design` |
| Object mapping        | `AutoMapper`, `AutoMapper.Extensions.Microsoft.DependencyInjection`           |
| API docs              | `Swashbuckle.AspNetCore`                                                      |

### Create the database

`AppDbContext` extends `IdentityDbContext<ApplicationUser>`, so the first migration
includes both Identity's tables (AspNetUsers, AspNetRoles, etc.) and the library's
own tables in one go:

```bash
cd LibraryManagement.API
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Run the API

```bash
cd LibraryManagement.API
dotnet run
```

Swagger UI: `https://localhost:{port}/swagger` (Development environment).

## What's already in place vs. what each module owner builds

**Already scaffolded:**

- Full folder layout above
- All entities with relationships configured in `AppDbContext.OnModelCreating`
- Enums shared across modules
- `ApplicationUser : IdentityUser` for Identity-based auth
- Generic `IRepository<T>` / `Repository<T>`
- `ApiResponse<T>` wrapper + global exception middleware
- JWT + Identity wired together in `Program.cs`
- One placeholder controller per module

**Each module owner adds, inside the shared folders:**

1. DTOs in `DTOs/{ModuleName}/`
2. A module-specific repository interface + class in `Repositories/` (extend `Repository<T>`
   for simple CRUD, or add custom query methods for anything more complex)
3. A service interface + class in `Services/`
4. Replace the placeholder controller in `Controllers/` with real actions
5. Register the new repository/service in `Program.cs` (see the `// TODO` comment)
6. Any entity-specific Fluent API config not already in `AppDbContext`

## Before coding starts — agree on these as a team

- **Migration conflicts**: since everyone edits the same `AppDbContext` and shares
  the `Migrations/` folder, decide who applies/merges migrations (e.g. one gatekeeper
  reviews and runs `dotnet ef migrations add` after each module's entity changes are
  merged, rather than everyone generating migrations independently)
- **Git workflow**: feature branch per module → PR → merge to `develop`
- **Background job** for overdue detection / reservation expiration — one Hosted
  Service, not scattered per-module logic
