# job-seeker

## Activate Dotnet EF CLI
dotnet tool install --global dotnet-ef

## Create migration
dotnet ef migrations add InitialCreate

## Remove migration
dotnet ef migrations remove

## Running Migration
dotnet ef database update
