# Implementation Sequence: PixelSchubser Plattformkern

Dieses Dokument beschreibt die empfohlene Umsetzungsabfolge und verweist pro
Schritt auf die relevanten Stellen in den vorhandenen Implementierungsdetails.

## 1. Scope und Architektur fixieren

- Ziel: Sicherstellen, dass Avalonia und API als getrennte Adapter auf denselben
  Application-Kern umgesetzt werden.
- Referenzen:
  - `plan.md` -> Summary
  - `plan.md` -> Constraints
  - `plan.md` -> Structure Decision
  - `research.md` -> Decision 2 (Thin Avalonia Shell)
  - `research.md` -> Decision 2b (Thin REST API Host)

## 2. Solution- und Projektstruktur anlegen

- Ziel: Alle Kern- und Adapterprojekte sowie Testprojekte in der vorgesehenen
  Topologie erstellen.
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

## 9. Avalonia-Adapter umsetzen

- Ziel: Views/ViewModels als dünne Adapter umsetzen, ohne Fachlogik in UI.
- Referenzen:
  - `research.md` -> Decision 2 (Thin Avalonia Shell)
  - `plan.md` -> Note
  - `spec.md` -> FR-001, FR-002, FR-007
  - `quickstart.md` -> Setup (Avalonia starten)

## 10. Testpyramide und Konformitätsnachweis abschließen

- Ziel: Alle geforderten Testebenen und Performance-Budgets nachweisen.
- Referenzen:
  - `plan.md` -> Testing
  - `spec.md` -> CA-002
  - `spec.md` -> CA-004
  - `spec.md` -> SC-001, SC-002, SC-003, SC-005, SC-006
  - `quickstart.md` -> What to Verify

## Festgestellte Lücken aus der aktuellen Dokumentlage

- Es gibt aktuell noch kein `tasks.md` mit konkreten, dateibasierten
  Arbeitspaketen.
- Die Reihenfolge war zuvor auf mehrere Dateien verteilt, aber nicht als
  explizite Umsetzungsabfolge dokumentiert.

## Nächster Schritt

- `speckit.tasks` ausführen, damit die obige Sequenz in konkrete Aufgaben (mit
  Datei- und Testbezug) überführt wird.
