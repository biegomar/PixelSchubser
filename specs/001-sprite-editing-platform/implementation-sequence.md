# Implementation Sequence: PixelSchubser Plattformkern

Dieses Dokument beschreibt die empfohlene Umsetzungsabfolge und verweist pro
Schritt auf die relevanten Stellen in den vorhandenen Implementierungsdetails.

## 1. Scope und Architektur fixieren

- Ziel: Sicherstellen, dass Avalonia, API und MCP als getrennte Adapter auf
  denselben Application-Kern umgesetzt werden.
- Referenzen:
  - `plan.md` -> Summary
  - `plan.md` -> Constraints
  - `plan.md` -> Structure Decision
  - `research.md` -> Decision 2 (Thin Avalonia Shell)
  - `research.md` -> Decision 2b (Thin REST API Host)
  - `research.md` -> Decision 6 (API and MCP Included as First-Class Adapters)

## 2. Solution- und Projektstruktur anlegen

- Ziel: Alle Kern- und Adapterprojekte sowie Testprojekte in der vorgesehenen
  Topologie erstellen, inklusive MCP-Host.
- Referenzen:
  - `plan.md` -> Source Code (repository root)
  - `quickstart.md` -> Setup
  - `quickstart.md` -> Expected Commands

## 3. Domain-Kern implementieren

- Ziel: Fachliche Regeln ohne UI/API/Infra-Abhängigkeiten modellieren.
- Umfänge:
  - Aggregate und Entitäten
  - Invarianten und Validierung
  - State-Übergänge
- Referenzen:
  - `data-model.md` -> SpriteProject
  - `data-model.md` -> Sprite
  - `data-model.md` -> Animation
  - `data-model.md` -> Selection
  - `data-model.md` -> PlatformProfile
  - `data-model.md` -> State Transitions
  - `spec.md` -> FR-001, FR-002, FR-003, FR-004, FR-010, FR-012, FR-013, FR-014

## 4. Application Layer (Commands/Queries) implementieren

- Ziel: Use Cases, Orchestrierung, Undo/Redo-Koordination und anwendungsseitige
  Validierung aufsetzen.
- Referenzen:
  - `research.md` -> Decision 4 (Command/Query Application Model)
  - `data-model.md` -> State Transitions
  - `spec.md` -> FR-002, FR-006, FR-008, FR-011, FR-013
  - `plan.md` -> Testing

## 5. Rendering-Schicht und RenderModel umsetzen

- Ziel: Fachzustand in ein UI-/Export-fähiges RenderModel transformieren, ohne
  Renderinglogik in der Domain.
- Referenzen:
  - `research.md` -> Decision 5 (Dedicated Rendering Model)
  - `data-model.md` -> RenderModel
  - `spec.md` -> FR-007
  - `quickstart.md` -> What to Verify (Preview rendering)

## 6. Formatadapter (Import/Export) implementieren

- Ziel: SPM/SPD/SPR/PNG/ASM/BASIC als registrierbare Adapter umsetzen.
- Referenzen:
  - `data-model.md` -> ImportExportAdapter
  - `spec.md` -> FR-005
  - `spec.md` -> SC-004
  - `plan.md` -> Testing (golden master tests)

## 7. Infrastructure implementieren

- Ziel: Dateiablage, Autosave, Settings Store und Test-Doubles bereitstellen.
- Referenzen:
  - `research.md` -> Decision 3 (File-Based Persistence)
  - `plan.md` -> Storage
  - `spec.md` -> FR-006

## 8. API-Adapter umsetzen

- Ziel: Dünnen REST-Host erstellen, der nur in Application-Use-Cases delegiert.
- Referenzen:
  - `research.md` -> Decision 2b (Thin REST API Host)
  - `research.md` -> Decision 6 (API Included as First-Class Adapter)
  - `plan.md` -> Note
  - `spec.md` -> FR-008
  - `quickstart.md` -> Setup (REST API host starten)

## 9. MCP-Adapter umsetzen

- Ziel: Dünnen MCP-Host erstellen, der nur in Application-Use-Cases delegiert,
  und als self-contained single-file Artifact ausgeliefert wird.
- Referenzen:
  - `research.md` -> Decision 6 (API and MCP Included as First-Class Adapters)
  - `research.md` -> Decision 7 (Self-Contained MCP Delivery)
  - `plan.md` -> Constraints
  - `spec.md` -> FR-008

## 10. Avalonia-Adapter umsetzen

- Ziel: Views/ViewModels als dünne Adapter umsetzen, ohne Fachlogik in UI.
- Referenzen:
  - `research.md` -> Decision 2 (Thin Avalonia Shell)
  - `plan.md` -> Note
  - `spec.md` -> FR-001, FR-002, FR-007
  - `quickstart.md` -> Setup (Avalonia starten)

## 11. Testpyramide und Konformitätsnachweis abschließen

- Ziel: Alle geforderten Testebenen und Performance-Budgets nachweisen.
- Referenzen:
  - `plan.md` -> Testing
  - `spec.md` -> CA-002
  - `spec.md` -> CA-004
  - `spec.md` -> SC-001, SC-002, SC-003, SC-005, SC-006
  - `quickstart.md` -> What to Verify

## Festgestellte Lücken aus der aktuellen Dokumentlage

- Der konkrete Taskschnitt fuer Animation-Use-Cases (Playback/FPS-Preview) muss
  waehrend der Implementierung weiter granularisiert werden.
- Bei API/MCP-Adapterparitaet sollten gemeinsame Test-Fixtures genutzt werden,
  um doppelte Pflege zu vermeiden.

## Naechster Schritt

- Umsetzung in der Reihenfolge aus `tasks.md` starten und nach jedem
  Story-Checkpoint die Paritaets- und Performancekriterien pruefen.

## Deferred Web Adapter Parity Contract (Follow-up)

Diese Checkliste definiert die Mindestkriterien, bevor ein Web-Adapter als
gleichwertiger Adapter akzeptiert werden kann.

- [ ] Web adapter delegates all business behavior to Application use cases and
      keeps transport/UI concerns isolated.
- [ ] Command names and required parameters are semantically equivalent to
      Desktop/API/MCP flows.
- [ ] Validation semantics match API/MCP conventions (same error categories and
      stable error codes).
- [ ] Error payload contract includes explicit version marker and maps to the
      same domain/application failure reasons.
- [ ] Project lifecycle flows (create/load/save/import/export) produce parity
      results with existing adapters for equivalent inputs.
- [ ] Editing flows (set pixel, selection fill, transform) match parity outputs
      for deterministic regression fixtures.
- [ ] Animation flows (set settings, preview) match parity outputs for frame
      lists and mode handling.
- [ ] Contract tests include versioned Web adapter endpoints/messages and a
      compatibility policy for non-breaking evolution.
- [ ] Performance acceptance checks include edit/preview and import/export paths
      with the same p95 budgets used in the current platform slice.
- [ ] Adapter boundary review confirms no direct dependency from Web adapter to
      Avalonia, API host internals, or MCP host internals.
