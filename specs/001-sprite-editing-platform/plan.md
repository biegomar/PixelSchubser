# Implementation Plan: PixelSchubser Plattformkern

**Branch**: `[001-specification]` | **Date**: 2026-06-06 | **Spec**:
[spec.md](spec.md)

**Input**: Feature specification from
`/specs/001-sprite-editing-platform/spec.md`

**Note**: This plan is desktop-first, but it also includes REST API and MCP as
separate adapters on the same application core. The Avalonia UI remains a thin
adapter and does not depend on API or MCP hosts.

**Scope Note**: A Web adapter is intentionally planned as a follow-up adapter.
It is not part of the first implementation slice, but must follow the same
command/query and error-contract semantics used by Desktop, API, and MCP.

## Summary

Implement a .NET 10 sprite editing platform with Avalonia UI, REST API, and MCP
on top of a hexagonal core. The first delivery phase covers domain rules,
application use cases, rendering, import/export adapters, file-based
persistence, undo/redo, the Avalonia shell, the API host, and the MCP host. All
business behavior stays inside the core and is exposed to adapters through
commands and queries.

## Technical Context

**Language/Version**: C# on .NET 10

**Primary Dependencies**: Avalonia UI, ASP.NET Core, Model Context Protocol SDK,
xUnit, CommunityToolkit.Mvvm

**Storage**: File-based project storage, autosave files, settings store,
in-memory test store

**Testing**: Domain unit tests, application use-case tests, import/export golden
master tests, renderer snapshot tests, API contract tests, MCP tool tests,
headless integration tests

**Target Platform**: Windows, Linux, macOS desktop

**Project Type**: Desktop application plus REST API and MCP hosts with reusable
headless core libraries

**Performance Goals**: Interactive edit actions p95 under 50 ms; preview render
p95 under 250 ms; import/export paths measured and validated against the same
performance budgets for representative project sizes

**Constraints**: Hexagonal boundaries enforced; no UI logic in domain; no
filesystem, JSON, HTTP, or logging dependencies in domain; Avalonia must not
depend on API or MCP hosts; API must stay thin and delegate to application use
cases; MCP host must stay thin and be published as self-contained single-file
deployment without runtime assembly loading; public API endpoints and MCP tools
must use explicit contract versioning and version-aware contract tests

**Scale/Scope**: Desktop editor plus REST API and MCP hosts for C64-focused
sprite workflows with a reusable core for later web adapters

## Constitution Check

_GATE: Must pass before Phase 0 research. Re-check after Phase 1 design._

- **Code Quality Gate**: Pass. Domain and application layers remain independent
  of Avalonia and host-specific concerns.
- **Testing Gate**: Pass. The plan includes unit, use-case, golden-master,
  snapshot, and headless integration coverage.
- **UX Consistency Gate**: Pass. The same command/query model backs all future
  adapters, and the desktop shell is just one adapter.
- **Performance Gate**: Pass. The plan carries the 50 ms / 250 ms budgets from
  the spec into implementation and validation, including import/export path
  measurements.
- **Architecture Decision Gate**: Pass. API and MCP are in scope as thin
  adapters, and Avalonia remains decoupled from both.

## Project Structure

### Documentation (this feature)

```text
specs/001-sprite-editing-platform/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
└── implementation-sequence.md
```

### Source Code (repository root)

```text
src/
├── PixelSchubser.Domain/
├── PixelSchubser.Application/
├── PixelSchubser.Formats/
├── PixelSchubser.Rendering/
├── PixelSchubser.Infrastructure/
├── PixelSchubser.Avalonia/
├── PixelSchubser.Api/
└── PixelSchubser.Mcp/

tests/
├── PixelSchubser.Domain.Tests/
├── PixelSchubser.Application.Tests/
├── PixelSchubser.Formats.Tests/
├── PixelSchubser.Rendering.Tests/
├── PixelSchubser.Avalonia.Tests/
├── PixelSchubser.Api.Tests/
└── PixelSchubser.Mcp.Tests/
```

**Structure Decision**: Use a desktop-first hexagonal solution with separate
Domain, Application, Formats, Rendering, Infrastructure, Avalonia host, REST API
host, and MCP host projects. The Avalonia shell remains a thin adapter and does
not call API or MCP hosts. The MCP host is delivered as self-contained
single-file artifact.

## Complexity Tracking

No constitution violations require justification for this phase.
