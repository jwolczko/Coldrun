# Coldrun

Coldrun is a modular .NET REST API for managing trucks in an ERP-style system. The current module supports truck CRUD operations, truck status changes, filtering, sorting, and pagination.

Original task description: https://coldrun.notion.site/coding-task-net-engineer

## Requirements

- .NET SDK 10.0 or newer
- SQL Server, for example SQL Server Developer, SQL Server Express, LocalDB, or a SQL Server Docker container
- A SQL client capable of running T-SQL scripts, for example SQL Server Management Studio or Azure Data Studio
- Optional: Bruno, if you want to use the request collection from the `bruno` directory

## Database Setup

The database script is located at:

```text
database/ColdrunDb/Coldrun.sql
```

Run this script against your SQL Server instance. The script is idempotent and can be executed multiple times. It will:

- create the `Coldrun` database if it does not exist
- create the `TruckManagement` schema if it does not exist
- create the `TruckStatuses` and `Trucks` tables if they do not exist
- create required constraints and indexes if they do not exist
- seed the truck statuses

The API uses this connection string by default:

```json
"ConnectionStrings": {
  "TruckManagement": "Server=localhost;Database=Coldrun;Trusted_Connection=True;TrustServerCertificate=True"
}
```

If your SQL Server uses a different host, instance name, user, or password, update `src/Coldrun.Api/appsettings.json` or override the connection string with environment-specific configuration.

## Build

From the repository root:

```bash
dotnet restore Coldrun.slnx
dotnet build Coldrun.slnx
```

## Run Tests

```bash
dotnet test Coldrun.slnx
```

## Run The API

Start the API project:

```bash
dotnet run --project src/Coldrun.Api/Coldrun.Api.csproj
```

The default HTTP URL is:

```text
http://localhost:5189
```

In Development mode, Swagger UI is available at:

```text
http://localhost:5189
```

The OpenAPI document is available at:

```text
http://localhost:5189/openapi/v1.json
```

## Bruno Collection

A Bruno collection is included in:

```text
bruno
```

Open this exact folder in Bruno:

```text
\Coldrun\bruno
```

Use the `Local` environment. It defines:

- `baseUrl`: `http://localhost:5189`
- `truckCode`: `TRK-001`

## Available Truck Statuses

The seeded statuses are:

- `Out Of Service`
- `Loading`
- `To Job`
- `At Job`
- `Returning`

Status transitions are validated by the domain model. `Out Of Service` can be entered from any status, any status can be entered from `Out Of Service`, and the normal workflow is:

```text
Loading -> To Job -> At Job -> Returning -> Loading
```

## Main Endpoints

- `GET /api/v1/trucks`
- `POST /api/v1/trucks`
- `GET /api/v1/trucks/{code}`
- `PATCH /api/v1/trucks/{code}`
- `DELETE /api/v1/trucks/{code}`
- `GET /api/v1/trucks/{code}/status`
- `PUT /api/v1/trucks/{code}/status`
