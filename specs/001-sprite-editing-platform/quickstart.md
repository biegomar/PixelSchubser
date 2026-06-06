# Quickstart: PixelSchubser Plattformkern

## Prerequisites

- .NET 10 SDK
- Avalonia desktop prerequisites for your OS

## Setup

1. Restore dependencies for the solution.
2. Build the solution.
3. Run the unit and integration test suites.
4. Start the Avalonia desktop application.
5. Start the REST API host.
6. Start the MCP host.
7. Publish the MCP host as self-contained single-file.

## Expected Commands

```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project src/PixelSchubser.Avalonia/PixelSchubser.Avalonia.csproj
dotnet run --project src/PixelSchubser.Api/PixelSchubser.Api.csproj
dotnet run --project src/PixelSchubser.Mcp/PixelSchubser.Mcp.csproj
dotnet publish src/PixelSchubser.Mcp/PixelSchubser.Mcp.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

## What to Verify

- The application starts without requiring API or MCP hosts.
- The Avalonia application starts without depending on the API host.
- The API host starts independently and exercises the same application use
  cases.
- The MCP host starts independently and exercises the same application use
  cases.
- The MCP publish output is self-contained single-file and starts without extra
  assembly deployment.
- A sprite can be created, edited, transformed, undone, and redone.
- Import/export flows work through application use cases, not UI logic.
- Preview rendering matches the underlying sprite state.

## Notes

- The core must remain testable without the Avalonia shell.
