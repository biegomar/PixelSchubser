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

## Adapter Boundaries (Architecture Guard)

- Avalonia is a desktop-only adapter and must call Application use cases
  directly.
- Avalonia must not call REST API or MCP hosts at runtime.
- REST API endpoints are transport adapters and must delegate all business
  behavior to Application services.
- MCP tools are automation adapters and must delegate all business behavior to
  Application services.
- Domain and Application projects must not reference API, Avalonia, MCP, or
  Infrastructure adapters.

## Performance Threshold Validation

- Interactive edit operations: p95 < 50 ms validated by
  `EditAndPreviewPerformanceTests`.
- Preview rendering path: p95 < 250 ms validated by
  `EditAndPreviewPerformanceTests`.
- Export path: p95 < 250 ms validated by `ImportExportPerformanceTests`.
- Import path: p95 < 250 ms validated by `ImportExportPerformanceTests`.

Run the dedicated performance checks:

```bash
dotnet test tests/PixelSchubser.Application.Tests/PixelSchubser.Application.Tests.csproj -c Debug --filter "FullyQualifiedName~Performance"
```

## Quickstart Validation Results (2026-06-08)

| Command                                                                                                                               | Result | Notes                                                                        |
| ------------------------------------------------------------------------------------------------------------------------------------- | ------ | ---------------------------------------------------------------------------- |
| `dotnet restore PixelSchubser.sln`                                                                                                    | PASS   | Restore completed successfully.                                              |
| `dotnet build PixelSchubser.sln -c Debug`                                                                                             | PASS   | All projects build successfully.                                             |
| `dotnet test PixelSchubser.sln -c Debug`                                                                                              | PASS   | 44 tests passed, 0 failed.                                                   |
| `dotnet run --project src/PixelSchubser.Api/PixelSchubser.Api.csproj -c Debug`                                                        | PASS   | Host started and listened on `http://localhost:5189`.                        |
| `dotnet run --project src/PixelSchubser.Mcp/PixelSchubser.Mcp.csproj -c Debug`                                                        | PASS   | Host started (`pixelschubser.tools v1.0`, stdio transport active).           |
| `dotnet run --project src/PixelSchubser.Avalonia/PixelSchubser.Avalonia.csproj -c Debug`                                              | PASS   | Desktop host starts successfully after restoring the Avalonia app bootstrap. |
| `dotnet publish src/PixelSchubser.Mcp/PixelSchubser.Mcp.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true` | PASS   | Self-contained single-file publish output produced.                          |

## Notes

- The core must remain testable without the Avalonia shell.
