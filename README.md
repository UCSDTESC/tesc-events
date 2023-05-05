# tesc.events

REST API for the Triton Engineering Student Council event manager. This project is open-source, and we welcome any contributions!

### Build Instructions

This project is built on .NET 6, C# 10. It is assumed you have Docker CLI and Docker Desktop, as well as the .NET SDK (in particular, the .NET CLI) installed on your local machine.

1. Clone the repository with ssh: `git clone git@github.com:UCSDTESC/tesc-events.git`.
2. Navigate to the directory: `cd tesc-events/TescEvents`.
3. Install PostgreSQL. See [installation instructions below](#installing-postgres).
4. Create or overwrite the `appsettings.Development.json` file using [Sample appsettings.Development.json](#sample-appsettingsdevelopmentjson) as a template
5. Create or overwrite the `.env` file using [Sample .env](#sample-env) (this is used for the database running in Docker)
6. Fill out the `appsettings.Development.json` file.
7. Run containerized services (e.g. PostgreSQL): `docker compose up -d`.
8. Restore NuGet packages with: `dotnet restore`.
9. Install Entity Framework Core tools: `dotnet tool install --global dotnet-ef`.
10. Verify the EF Core CLI is correctly installed: `dotnet ef`.
11. Initialize the database: `dotnet ef database update`.
12. Start the project: `dotnet run`.

#### Installing Postgres

MacOS and Linux users can install Postgres version 14.5 via [Homebrew](https://brew.sh), and Linux users can use `apt`. Windows users will need to download the Postgres 14.5 installer from [here](https://www.postgresql.org/download/windows/), run the installer, and add the Postgres bin to the PATH environment variable.

#### Migrating the Database

We use a code-first approach to our database migrations. This means every time we want to change the schema, we have to modify the schema within the code, then create a "migration" to upgrade the database.

1. Modify the schema
2. Run `dotnet ef migrations add [MIGRATION NAME HERE]`, e.g. `dotnet ef migrations add 0005-AddFirstNameColumnToUserTable`.
3. In the `TescEvents/Migrations/` directory, ensure the class name of the created migration in `0005-AddFirstNameColumnToUserTable.cs` matches the form `_0005AddFirstNameColumnToUserTable`.
4. In the `TescEvents/Migrations/` directory, ensure the annotation in `0005-AddFirstNameColumnToUserTable.Designer.cs`, matches the form `[Migration("0005-AddFirstNameColumnToUserTable")]`.
5. Update the database with `dotnet ef database update`.

#### Sample appsettings.Development.json

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Db": {
    "Host": "localhost",
    "Port": 5432,
    "Database": "tesc-events",
    "User": "tesc-dev",
    "Password": "password"
  },
  "Jwt": {
    "Key": "superultrahypermetasecretkey",
    "Issuer": "https://localhost:7208",
    "Audience": "https://localhost:7208"
  }
}
```

#### Sample .env

```
DB_DATABASE="tesc-events"
DB_USER="tesc-dev"
DB_PASSWORD="password"
DB_PORT=5432
```

**NOTE**: For Windows users, `localhost` won't work&mdash;you'll need to set `DB_HOST` to the Docker machine's IP address

1. Run `docker network inspect -f '{{range .IPAM.Config}}{{.Subnet}}{{end}}' tescevents_default`.
2. Set `DB_HOST={{ip without subnet mask}}` in your `.env` file.
