# Research: PixelSchubser Plattformkern

## Decision 1: .NET 10 Core with Avalonia UI, REST API, and MCP

- **Decision**: Use .NET 10 for the solution, Avalonia UI for the desktop host,
  ASP.NET Core for the REST API host, and an MCP host for automation workflows.
- **Rationale**: The specification requires desktop, API, and MCP surfaces while
  keeping the domain independent from adapter technology. Context7 confirms
  Avalonia is a XAML-first cross-platform framework with generated control
  wiring and a clear MVVM-friendly model, which fits the thin-adapter approach.
- **Alternatives considered**:
  - WPF: rejected because it is Windows-only.
  - Web-first UI: rejected because the current scope is desktop-first plus API,
    not web-first.
  - A custom immediate-mode UI: rejected because it adds unnecessary platform
    and maintenance risk.

## Decision 2: Thin Avalonia Shell, No Business Logic in Views

- **Decision**: Implement Avalonia views as thin adapters backed by viewmodels
  that only translate UI events into application use cases.
- **Rationale**: This aligns with the hexagonal architecture and keeps the core
  reusable for later web, API, and MCP adapters.
- **Alternatives considered**:
  - Code-behind-heavy views: rejected because they blur adapter and domain
    responsibilities.
  - Docking framework-first architecture: rejected for v1 because the shell can
    start with a simpler panel layout and avoid extra dependency surface.

## Decision 2b: Thin REST API Host

- **Decision**: Implement the REST API as a thin host that maps HTTP requests to
  application commands and queries.
- **Rationale**: This preserves the hexagonal architecture, keeps API behavior
  aligned with the desktop adapter, and ensures the Avalonia app does not depend
  on the API host.
- **Alternatives considered**:
  - API logic inside controllers: rejected because it would duplicate business
    rules and weaken testability.
  - Deferring API to a later phase: rejected because the user clarified that the
    API must be included in the plan and implemented now.

## Decision 3: File-Based Persistence and In-Memory Test Stores

- **Decision**: Use file-based persistence for projects and settings, plus
  in-memory stores for tests.
- **Rationale**: The product spec emphasizes local project files, autosave, and
  headless testability. This keeps the storage layer replaceable and easy to
  verify.
- **Alternatives considered**:
  - SQLite-first persistence: rejected for v1 because project files are the
    primary user-facing storage model.
  - Browser storage: rejected because the current phase does not include the web
    frontend.

## Decision 4: Command/Query Application Model with Explicit Undo Coordination

- **Decision**: Model edits as application commands and read paths as queries,
  with undo/redo coordinated in the application layer.
- **Rationale**: The architecture principles require automation-first use cases
  and explicit undo/redo support while keeping domain logic UI-independent.
- **Alternatives considered**:
  - UI-managed undo stack: rejected because it would couple history to the host.
  - Full event sourcing: rejected because the architecture explicitly calls for
    CQRS light, not a heavyweight event store.

## Decision 5: Dedicated Rendering Model

- **Decision**: Convert domain state into a render model before painting in the
  Avalonia host or exporting images.
- **Rationale**: This matches the architecture principle that rendering is
  separate from the domain and avoids leaking presentation concerns into the
  sprite model.
- **Alternatives considered**:
  - Direct domain rendering: rejected because it would mix representation with
    rules.
  - UI-specific rasterization inside the domain: rejected for the same reason.

## Decision 6: API and MCP Included as First-Class Adapters

- **Decision**: Implement REST API and MCP in this phase as first-class
  adapters.
- **Rationale**: The user clarified that MCP must also be planned and delivered
  now. Both hosts remain thin and directly reuse application use cases so the
  desktop shell stays independent.
- **Alternatives considered**:
  - Deferring API/MCP to a later phase: rejected due to the clarified scope.

## Decision 7: Self-Contained MCP Delivery

- **Decision**: Publish the MCP host as self-contained single-file artifact.
- **Rationale**: The user requires MCP deployment without post-deploy assembly
  loading or missing runtime dependencies.
- **Alternatives considered**:
  - Framework-dependent MCP build: rejected because it requires target runtime
    presence and can break portability.
  - Multi-file self-contained output: rejected because single-file artifact is
    operationally simpler for distribution.
