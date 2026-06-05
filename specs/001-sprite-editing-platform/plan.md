# Implementation Plan: PixelSchubser Plattformkern

**Branch**: `[001-specification]` | **Date**: 2026-06-06 | **Spec**:
[spec.md](spec.md)

**Input**: Feature specification from
`/specs/001-sprite-editing-platform/spec.md`

**Note**: This plan is desktop-first, but it also includes the REST API as a
separate adapter on the same application core. The Avalonia UI remains a thin
adapter and does not depend on the API host.

## Summary

Implement a .NET 10 sprite editing platform with Avalonia UI and REST API on top
of a hexagonal core. The first delivery phase covers domain rules, application
use cases, rendering, import/export adapters, file-based persistence, undo/redo,
the Avalonia shell, and the API host. All business behavior stays inside the
core and is exposed to adapters through commands and queries.

## Technical Context

**Language/Version**: C# on .NET 10

**Primary Dependencies**: Avalonia UI, ASP.NET Core, xUnit,
CommunityToolkit.Mvvm

**Storage**: File-based project storage, autosave files, settings store,
in-memory test store

**Testing**: Domain unit tests, application use-case tests, import/export golden
master tests, renderer snapshot tests, headless integration tests

**Target Platform**: Windows, Linux, macOS desktop

**Project Type**: Desktop application plus REST API with reusable headless core
libraries

**Performance Goals**: Interactive edit actions p95 under 50 ms; preview render
p95 under 250 ms

**Constraints**: Hexagonal boundaries enforced; no UI logic in domain; no
filesystem, JSON, HTTP, or logging dependencies in domain; Avalonia must not
depend on the API host; API must stay thin and delegate to application use cases

**Scale/Scope**: Desktop editor and REST API for C64-focused sprite workflows
with a reusable core for later MCP/web adapters

## Constitution Check

_GATE: Must pass before Phase 0 research. Re-check after Phase 1 design._

- **Code Quality Gate**: Pass. Domain and application layers remain independent
  of Avalonia and host-specific concerns.
- **Testing Gate**: Pass. The plan includes unit, use-case, golden-master,
  snapshot, and headless integration coverage.
- **UX Consistency Gate**: Pass. The same command/query model backs all future
  adapters, and the desktop shell is just one adapter.
- **Performance Gate**: Pass. The plan carries the 50 ms / 250 ms budgets from
  the spec into implementation and validation.
- **Architecture Decision Gate**: Pass. The API is in scope as a thin adapter,
  and Avalonia remains decoupled from it.

## Project Structure

### Documentation (this feature)

```text
specs/001-sprite-editing-platform/
├── plan.md
├── research.md
├── data-model.md
└── quickstart.md
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
└── PixelSchubser.Api/

tests/
├── PixelSchubser.Domain.Tests/
├── PixelSchubser.Application.Tests/
├── PixelSchubser.Formats.Tests/
├── PixelSchubser.Rendering.Tests/
├── PixelSchubser.Avalonia.Tests/
└── PixelSchubser.Api.Tests/
```

**Structure Decision**: Use a desktop-first hexagonal solution with separate
Domain, Application, Formats, Rendering, Infrastructure, Avalonia host, and REST
API host projects. The Avalonia shell remains a thin adapter and does not call
the API host.

## Complexity Tracking

No constitution violations require justification for this phase.
