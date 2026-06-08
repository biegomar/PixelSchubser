# Tasks: PixelSchubser Plattformkern

**Input**: Design documents from `/specs/001-sprite-editing-platform/`

**Prerequisites**: plan.md, spec.md, research.md, data-model.md, quickstart.md

**Tests**: Tests are REQUIRED by the constitution. Every user story includes
automated tests.

**Organization**: Tasks are grouped by user story so each story is independently
implementable and testable.

## Format: `[ID] [P?] [Story] Description`

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Initialize solution and baseline project structure for hexagonal
architecture.

- [x] T001 Create .NET solution file at PixelSchubser.sln
- [x] T002 Create base project files at
      src/PixelSchubser.Domain/PixelSchubser.Domain.csproj and
      src/PixelSchubser.Application/PixelSchubser.Application.csproj
- [x] T003 [P] Create adapter project files at
      src/PixelSchubser.Avalonia/PixelSchubser.Avalonia.csproj and
      src/PixelSchubser.Api/PixelSchubser.Api.csproj and
      src/PixelSchubser.Mcp/PixelSchubser.Mcp.csproj
- [x] T004 [P] Create infrastructure and support project files at
      src/PixelSchubser.Infrastructure/PixelSchubser.Infrastructure.csproj,
      src/PixelSchubser.Rendering/PixelSchubser.Rendering.csproj, and
      src/PixelSchubser.Formats/PixelSchubser.Formats.csproj
- [x] T005 Add all projects to solution and wire project references in
      PixelSchubser.sln

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Implement shared contracts and baseline architecture required by
all stories.

**CRITICAL**: No user story work can begin until this phase is complete.

- [x] T006 Define domain error/result primitives in
      src/PixelSchubser.Domain/Errors/DomainError.cs and
      src/PixelSchubser.Domain/Results/DomainResult.cs
- [x] T007 Define application command/query abstractions in
      src/PixelSchubser.Application/Abstractions/ICommand.cs and
      src/PixelSchubser.Application/Abstractions/IQuery.cs
- [x] T008 [P] Define repository and storage ports in
      src/PixelSchubser.Application/Ports/IProjectRepository.cs and
      src/PixelSchubser.Application/Ports/ISettingsStore.cs
- [x] T009 [P] Define render and format ports in
      src/PixelSchubser.Application/Ports/IRenderPreviewService.cs and
      src/PixelSchubser.Application/Ports/IFormatAdapterRegistry.cs
- [x] T010 Implement application composition root for non-host services in
      src/PixelSchubser.Application/DependencyInjection/ApplicationServiceCollectionExtensions.cs
- [x] T011 Implement baseline undo/redo coordinator abstraction in
      src/PixelSchubser.Application/UndoRedo/IUndoRedoCoordinator.cs and
      src/PixelSchubser.Application/UndoRedo/UndoRedoCoordinator.cs
- [x] T012 [P] Create test project bootstrap files at
      tests/PixelSchubser.Domain.Tests/PixelSchubser.Domain.Tests.csproj and
      tests/PixelSchubser.Application.Tests/PixelSchubser.Application.Tests.csproj
- [x] T013 [P] Create adapter test bootstrap files at
      tests/PixelSchubser.Api.Tests/PixelSchubser.Api.Tests.csproj and
      tests/PixelSchubser.Avalonia.Tests/PixelSchubser.Avalonia.Tests.csproj and
      tests/PixelSchubser.Mcp.Tests/PixelSchubser.Mcp.Tests.csproj

**Checkpoint**: Foundation ready. User story implementation can start.

---

## Phase 3: User Story 1 - Sprite bearbeiten im Kernsystem (Priority: P1) MVP

**Goal**: Deliver UI-independent sprite editing core with domain rules, use
cases, rendering model, and format support.

**Independent Test**: Run a headless workflow that creates a project, edits
pixels, applies transforms, renders preview, and validates persisted state.

### Tests for User Story 1 (REQUIRED)

- [x] T014 [P] [US1] Add domain unit tests for SpriteProject invariants in
      tests/PixelSchubser.Domain.Tests/SpriteProjectTests.cs
- [x] T015 [P] [US1] Add domain unit tests for selection-bounded transforms in
      tests/PixelSchubser.Domain.Tests/SelectionTransformTests.cs
- [x] T016 [P] [US1] Add application use-case tests for set pixel and transform
      flows in tests/PixelSchubser.Application.Tests/EditSpriteUseCaseTests.cs
- [x] T017 [P] [US1] Add renderer snapshot tests for preview output in
      tests/PixelSchubser.Rendering.Tests/PreviewRendererSnapshotTests.cs
- [x] T018 [P] [US1] Add headless integration test for create-edit-render
      workflow in tests/PixelSchubser.Application.Tests/HeadlessWorkflowTests.cs
- [x] T068 [P] [US1] Add domain unit tests for animation invariants (range, fps,
      mode) in tests/PixelSchubser.Domain.Tests/AnimationInvariantTests.cs
- [x] T069 [P] [US1] Add application use-case tests for animation preview query
      and playback parameters in
      tests/PixelSchubser.Application.Tests/AnimationPreviewUseCaseTests.cs

### Implementation for User Story 1

- [x] T019 [P] [US1] Implement SpriteProject aggregate in
      src/PixelSchubser.Domain/Entities/SpriteProject.cs
- [x] T020 [P] [US1] Implement Sprite entity and pen grid behavior in
      src/PixelSchubser.Domain/Entities/Sprite.cs and
      src/PixelSchubser.Domain/ValueObjects/PenGrid.cs
- [x] T021 [P] [US1] Implement Animation and Selection value objects in
      src/PixelSchubser.Domain/Entities/Animation.cs and
      src/PixelSchubser.Domain/ValueObjects/SelectionBounds.cs
- [x] T022 [P] [US1] Implement PlatformProfile and C64 default profile in
      src/PixelSchubser.Domain/Entities/PlatformProfile.cs and
      src/PixelSchubser.Domain/Profiles/C64PlatformProfile.cs
- [x] T023 [US1] Implement edit commands and handlers in
      src/PixelSchubser.Application/UseCases/Commands/SetPixelCommand.cs and
      src/PixelSchubser.Application/UseCases/Handlers/SetPixelCommandHandler.cs
- [x] T024 [US1] Implement transformation command handlers in
      src/PixelSchubser.Application/UseCases/Handlers/TransformSpriteCommandHandler.cs
- [x] T025 [US1] Implement project lifecycle commands in
      src/PixelSchubser.Application/UseCases/Handlers/CreateLoadSaveProjectHandlers.cs
- [x] T026 [US1] Implement render model mapper and preview service in
      src/PixelSchubser.Rendering/RenderModel/RenderModelMapper.cs and
      src/PixelSchubser.Rendering/Preview/PreviewRenderer.cs
- [x] T070 [US1] Implement animation commands and queries (set range/fps/mode,
      get preview) in
      src/PixelSchubser.Application/UseCases/Commands/SetAnimationSettingsCommand.cs
      and
      src/PixelSchubser.Application/UseCases/Queries/GetAnimationPreviewQuery.cs
- [x] T071 [US1] Implement animation handlers in
      src/PixelSchubser.Application/UseCases/Handlers/SetAnimationSettingsCommandHandler.cs
      and
      src/PixelSchubser.Application/UseCases/Handlers/GetAnimationPreviewQueryHandler.cs
- [x] T072 [US1] Implement animation timeline preview renderer in
      src/PixelSchubser.Rendering/Preview/AnimationPreviewRenderer.cs
- [x] T027 [US1] Implement format adapters for SPM and SPD/SPR in
      src/PixelSchubser.Formats/Adapters/SpmFormatAdapter.cs and
      src/PixelSchubser.Formats/Adapters/SpritePadFormatAdapter.cs
- [x] T028 [US1] Implement infrastructure file repository and settings store in
      src/PixelSchubser.Infrastructure/Persistence/FileProjectRepository.cs and
      src/PixelSchubser.Infrastructure/Settings/FileSettingsStore.cs

**Checkpoint**: User Story 1 is fully functional and independently testable.

---

## Phase 4: User Story 2 - Automatisierung ueber Adapter (Priority: P2)

**Goal**: Expose core use cases via thin REST API and MCP adapters and ensure
behavior parity with the core.

**Independent Test**: Execute API requests and MCP tool calls for project and
edit operations and verify equivalence with direct application use-case
outcomes.

### Tests for User Story 2 (REQUIRED)

- [x] T029 [P] [US2] Add API contract tests for project endpoints in
      tests/PixelSchubser.Api.Tests/Contracts/ProjectEndpointsContractTests.cs
- [x] T030 [P] [US2] Add API contract tests for sprite edit endpoints in
      tests/PixelSchubser.Api.Tests/Contracts/SpriteEditEndpointsContractTests.cs
- [x] T031 [P] [US2] Add API integration tests for validation and error mapping
      in
      tests/PixelSchubser.Api.Tests/Integration/ValidationAndErrorMappingTests.cs
- [x] T032 [P] [US2] Add cross-adapter parity tests between API and application
      handlers in
      tests/PixelSchubser.Api.Tests/Integration/ApiCoreParityTests.cs
- [x] T033 [P] [US2] Add regression test for unsupported format error behavior
      in
      tests/PixelSchubser.Application.Tests/UnsupportedFormatRegressionTests.cs
- [x] T073 [P] [US2] Add API contract tests for animation preview and settings
      endpoints in
      tests/PixelSchubser.Api.Tests/Contracts/AnimationEndpointsContractTests.cs
- [x] T059 [P] [US2] Add MCP contract tests for project and sprite tools in
      tests/PixelSchubser.Mcp.Tests/Contracts/McpToolsContractTests.cs
- [x] T060 [P] [US2] Add MCP integration tests for parity and validation in
      tests/PixelSchubser.Mcp.Tests/Integration/McpCoreParityTests.cs
- [x] T061 [P] [US2] Add self-contained MCP publish verification test in
      tests/PixelSchubser.Mcp.Tests/Deployment/SelfContainedPublishVerificationTests.cs
- [x] T074 [P] [US2] Add MCP contract tests for animation tools in
      tests/PixelSchubser.Mcp.Tests/Contracts/McpAnimationToolsContractTests.cs
- [x] T075 [P] [US2] Add cross-adapter animation parity tests (Core/API/MCP) in
      tests/PixelSchubser.Application.Tests/AnimationParityIntegrationTests.cs
- [x] T081 [P] [US2] Add API contract versioning tests for route groups and
      backward-compatible payload evolution in
      tests/PixelSchubser.Api.Tests/Contracts/ApiVersioningContractTests.cs
- [x] T082 [P] [US2] Add MCP contract versioning tests for tool namespace and
      version negotiation behavior in
      tests/PixelSchubser.Mcp.Tests/Contracts/McpVersioningContractTests.cs

### Implementation for User Story 2

- [x] T034 [US2] Create API composition root and dependency wiring in
      src/PixelSchubser.Api/Program.cs
- [x] T035 [US2] Implement project management endpoints in
      src/PixelSchubser.Api/Endpoints/ProjectEndpoints.cs
- [x] T036 [US2] Implement sprite editing endpoints in
      src/PixelSchubser.Api/Endpoints/SpriteEditEndpoints.cs
- [x] T037 [US2] Implement rendering and preview endpoints in
      src/PixelSchubser.Api/Endpoints/PreviewEndpoints.cs
- [x] T038 [US2] Implement import/export endpoints in
      src/PixelSchubser.Api/Endpoints/FormatEndpoints.cs
- [x] T076 [US2] Implement API animation endpoints in
      src/PixelSchubser.Api/Endpoints/AnimationEndpoints.cs
- [x] T083 [US2] Implement API contract versioning strategy (v1 route group,
      version policy, and deprecation metadata) in
      src/PixelSchubser.Api/Program.cs and
      src/PixelSchubser.Api/Configuration/ApiVersioningOptions.cs
- [x] T039 [US2] Implement API DTO contracts in
      src/PixelSchubser.Api/Contracts/Requests/EditSpriteRequest.cs and
      src/PixelSchubser.Api/Contracts/Responses/ProjectResponse.cs
- [x] T040 [US2] Implement consistent problem details mapping in
      src/PixelSchubser.Api/Errors/ProblemDetailsMapper.cs
- [x] T041 [US2] Add API host configuration limits for payload and path
      validation in src/PixelSchubser.Api/Configuration/ApiHostOptions.cs
- [x] T062 [US2] Create MCP composition root and dependency wiring in
      src/PixelSchubser.Mcp/Program.cs
- [x] T063 [US2] Implement MCP project and lifecycle tools in
      src/PixelSchubser.Mcp/Tools/ProjectTools.cs
- [x] T064 [US2] Implement MCP sprite edit and transform tools in
      src/PixelSchubser.Mcp/Tools/SpriteEditTools.cs
- [x] T065 [US2] Implement MCP render and export tools in
      src/PixelSchubser.Mcp/Tools/RenderExportTools.cs
- [x] T077 [US2] Implement MCP animation tools in
      src/PixelSchubser.Mcp/Tools/AnimationTools.cs
- [x] T084 [US2] Implement MCP tool contract versioning and compatibility
      manifest in src/PixelSchubser.Mcp/Contracts/McpContractVersion.cs and
      src/PixelSchubser.Mcp/Configuration/McpContractCompatibilityOptions.cs
- [x] T066 [US2] Implement MCP error and validation mapping in
      src/PixelSchubser.Mcp/Errors/McpErrorMapper.cs
- [x] T067 [US2] Add self-contained single-file publish profile in
      src/PixelSchubser.Mcp/Properties/PublishProfiles/SelfContained.pubxml

**Checkpoint**: User Stories 1 and 2 are both independently functional.

---

## Phase 5: User Story 3 - Erweiterbare Plattformunterstuetzung (Priority: P3)

**Goal**: Enable extension points for platform profiles and format adapters
without changing core behavior.

**Independent Test**: Add a new platform profile and adapter through registry
extension and confirm existing core behavior remains unchanged.

### Tests for User Story 3 (REQUIRED)

- [x] T042 [P] [US3] Add unit tests for profile registry behavior in
      tests/PixelSchubser.Application.Tests/ProfileRegistryTests.cs
- [x] T043 [P] [US3] Add unit tests for format adapter registry and priority in
      tests/PixelSchubser.Formats.Tests/FormatAdapterRegistryTests.cs
- [x] T044 [P] [US3] Add integration tests for adding a new export adapter
      without core changes in
      tests/PixelSchubser.Formats.Tests/Integration/NewAdapterIntegrationTests.cs
- [x] T045 [P] [US3] Add API integration tests for profile selection and
      validation in
      tests/PixelSchubser.Api.Tests/Integration/ProfileSelectionTests.cs

### Implementation for User Story 3

- [x] T046 [US3] Implement platform profile registry in
      src/PixelSchubser.Application/Profiles/IPlatformProfileRegistry.cs and
      src/PixelSchubser.Application/Profiles/PlatformProfileRegistry.cs
- [x] T047 [US3] Implement format adapter registry in
      src/PixelSchubser.Formats/Registry/FormatAdapterRegistry.cs
- [x] T048 [US3] Implement pluggable export pipeline in
      src/PixelSchubser.Application/UseCases/Handlers/ExportProjectCommandHandler.cs
- [x] T049 [US3] Add extension registration module for formats and profiles in
      src/PixelSchubser.Infrastructure/DependencyInjection/ExtensionRegistration.cs
- [x] T050 [US3] Implement sample additional platform profile in
      src/PixelSchubser.Domain/Profiles/NesPlatformProfile.cs
- [x] T051 [US3] Implement sample additional exporter adapter in
      src/PixelSchubser.Formats/Adapters/NesSpriteFormatAdapter.cs
- [x] T052 [US3] Add API endpoints for profile discovery and selection in
      src/PixelSchubser.Api/Endpoints/ProfileEndpoints.cs

**Checkpoint**: All three user stories are independently functional.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final quality hardening across all stories.

- [x] T053 [P] Add architecture guard tests for forbidden layer dependencies in
      tests/PixelSchubser.Application.Tests/Architecture/DependencyRuleTests.cs
- [x] T054 Add performance benchmark tests for edit and preview budgets in
      tests/PixelSchubser.Application.Tests/Performance/EditAndPreviewPerformanceTests.cs
- [x] T079 [P] Add performance benchmark tests for import and export paths in
      tests/PixelSchubser.Application.Tests/Performance/ImportExportPerformanceTests.cs
- [x] T080 Add import/export performance threshold validation and reporting in
      specs/001-sprite-editing-platform/quickstart.md
- [x] T055 [P] Add documentation for API and Avalonia adapter boundaries in
      specs/001-sprite-editing-platform/quickstart.md
- [x] T056 Add structured logging adapter in
      src/PixelSchubser.Infrastructure/Logging/StructuredLoggerAdapter.cs
- [x] T057 [P] Add final end-to-end regression scenario in
      tests/PixelSchubser.Application.Tests/HeadlessEndToEndRegressionTests.cs
- [x] T058 Run quickstart validation and record results in
      specs/001-sprite-editing-platform/quickstart.md
- [x] T078 Define deferred web adapter parity contract and acceptance checklist
      in specs/001-sprite-editing-platform/implementation-sequence.md

---

## Dependencies & Execution Order

### Phase Dependencies

- Setup (Phase 1) has no dependencies and starts immediately.
- Foundational (Phase 2) depends on Setup and blocks all story work.
- User Story phases (Phase 3-5) depend on Foundational completion.
- Polish (Phase 6) depends on completion of all selected user stories.

### User Story Dependencies

- US1 (P1) depends only on Foundational and is the MVP slice.
- US2 (P2) depends on US1 core use cases and can then proceed independently as
  API/MCP adapter work.
- US3 (P3) depends on US1 registry-ready core and can proceed after US2 or in
  parallel with late US2 hardening.

### Within Each User Story

- Tests are written first and must fail before implementation.
- Domain and models before handlers/services.
- Handlers/services before adapter endpoints.
- Story-specific validation and parity checks before checkpoint.

## Parallel Opportunities

- Phase 1: T003 and T004 can run in parallel after T001/T002.
- Phase 2: T008, T009, T012, and T013 can run in parallel after T006/T007.
- US1: T014-T018 and T068-T069 can run in parallel; T019-T022 can run in
  parallel.
- US2: T029-T033, T059-T061, T073-T075, and T081-T082 can run in parallel;
  T035-T039, T063-T065, T076, T077, T083, and T084 can run in parallel after
  T034 and T062.
- US3: T042-T045 can run in parallel; T050 and T051 can run in parallel after
  T046/T047.
- Polish: T053, T055, T057, and T079 can run in parallel.

## Parallel Example: User Story 1

```bash
# Parallel test start for US1
Task: "T014 Domain invariants tests in tests/PixelSchubser.Domain.Tests/SpriteProjectTests.cs"
Task: "T015 Selection transform tests in tests/PixelSchubser.Domain.Tests/SelectionTransformTests.cs"
Task: "T016 Use-case tests in tests/PixelSchubser.Application.Tests/EditSpriteUseCaseTests.cs"

# Parallel model implementation for US1
Task: "T019 SpriteProject aggregate in src/PixelSchubser.Domain/Entities/SpriteProject.cs"
Task: "T020 Sprite and PenGrid in src/PixelSchubser.Domain/Entities/Sprite.cs"
Task: "T021 Animation and Selection in src/PixelSchubser.Domain/Entities/Animation.cs"
Task: "T022 PlatformProfile in src/PixelSchubser.Domain/Entities/PlatformProfile.cs"
```

## Implementation Strategy

### MVP First (US1 only)

1. Complete Phase 1 and Phase 2.
2. Deliver US1 through T028.
3. Validate headless workflow and rendering checkpoints.
4. Demo the core without API coupling.

### Incremental Delivery

1. Deliver US1 core.
2. Add US2 REST API and MCP adapters while keeping parity tests green.
3. Add US3 extension points and sample adapters.
4. Finish with Phase 6 performance and architecture guards.

### Parallel Team Strategy

1. Core team delivers Foundational and US1 domain/application.
2. API and MCP team starts US2 after core handlers stabilize.
3. Extensibility team starts US3 registry work once US1 abstractions are
   available.
4. Shared QA stream runs parity/performance checks continuously.
