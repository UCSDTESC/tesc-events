# tesc.events

REST API for the Triton Engineering Student Council event manager. This project is open-source, and we welcome any contributions!

### Build Instructions

This project is built on .NET 6, C# 10. It is assumed you have Docker CLI and Docker Desktop, as well as the .NET SDK (in particular, the .NET CLI) installed on your local machine.

1. Clone the repository with ssh: `git clone git@github.com:UCSDTESC/tesc-events.git`.
2. Navigate to the directory: `cd tesc-events/TescEvents`.
3. Install PostgreSQL. See [installation instructions below](#installing-postgres).
4. Create a new `.env` file using [`.env.example`](#sample-env) as a template: `cp .env.example .env`.
5. Fill out the `.env`.
6. Run containerized services (e.g. PostgreSQL): `docker compose up -d`.
7. Restore NuGet packages with: `dotnet restore`.
8. Install Entity Framework Core tools: `dotnet tool install --global dotnet-ef`.
9. Verify the EF Core CLI is correctly installed: `dotnet ef`.
10. Initialize the database: `dotnet ef database update`.
11. Start the project: `dotnet run`.

#### Installing Postgres

MacOS and Linux users can install Postgres version 14.5 via [Homebrew](https://brew.sh), and Linux users can use `apt`. Windows users will need to download the Postgres 14.5 installer from [here](https://www.postgresql.org/download/windows/), run the installer, and add the Postgres bin to the PATH environment variable.

#### Sample `.env`

```
DB_HOST=localhost
DB_PORT=5432
DB_DATABASE=tesc-events
DB_USER=tesc-dev
DB_PASS=password
```

**NOTE**: For Windows users, `localhost` won't work&mdash;you'll need to set `DB_HOST` to the Docker machine's IP address

1. Run `docker network inspect -f '{{range .IPAM.Config}}{{.Subnet}}{{end}}' tescevents_default`.
2. Set `DB_HOST={{ip without subnet mask}}` in your `.env` file.
