# Feature Specification: PixelSchubser Plattformkern

**Feature Branch**: `[001-specification]`

**Created**: 2026-06-06

**Status**: Draft

**Input**: User description: "erstelle eine Spezifikation unter Berücksichtigung
der Anforderungen und der Architektur Prinzipien"

## User Scenarios & Testing _(mandatory)_

### User Story 1 - Sprite bearbeiten im Kernsystem (Priority: P1)

Als Anwender möchte ich Sprite-Projekte unabhängig von einer konkreten
Benutzeroberfläche bearbeiten können, damit dieselben Bearbeitungsfunktionen in
Desktop, Web, API, MCP und Headless-Workflows identisch verfügbar sind.

**Why this priority**: Die fachliche Bearbeitung ist der primäre Wert des
Produkts und die Grundlage aller Adapter.

**Independent Test**: Kann vollständig über einen Headless-Workflow getestet
werden, der ein Projekt erstellt, Pixel setzt, transformiert und das Ergebnis
überprüft.

**Acceptance Scenarios**:

1. **Given** ein neues Projekt, **When** ich ein Pixel setze, **Then** ist der
   Projektzustand ohne UI-Abhängigkeit aktualisiert.
2. **Given** eine aktive Auswahl, **When** ich eine Transformation ausführe,
   **Then** wirkt die Operation nur innerhalb der Auswahl.

---

### User Story 2 - Automatisierung über Adapter (Priority: P2)

Als Integrator möchte ich dieselben Bearbeitungsfunktionen über REST API und MCP
nutzen können, damit externe Systeme, Skripte und KI-gestützte Workflows den
Sprite-Kern steuern können.

**Why this priority**: Automatisierung ist ein zentrales Produktziel und
erweitert den Nutzen über die Desktop-Oberfläche hinaus.

**Independent Test**: Kann durch Aufruf einer API-Operation und eines MCP-Tools
validiert werden, die denselben fachlichen Zustand erzeugen wie die
Kernfunktion.

**Acceptance Scenarios**:

1. **Given** ein geladenes Projekt, **When** ein Adapter eine Bearbeitungsaktion
   auslöst, **Then** ist das Ergebnis fachlich identisch mit der direkten
   Kernoperation.
2. **Given** ein ungültiger Eingabewert, **When** ein API- oder MCP-Aufruf
   erfolgt, **Then** wird derselbe fachliche Fehler zurückgegeben wie im Kern.

---

### User Story 3 - Erweiterbare Plattformunterstützung (Priority: P3)

Als Produktverantwortlicher möchte ich neue Formate, Plattformprofile und
Frontends ergänzen können, ohne die bestehende Fachlogik anzupassen, damit
PixelSchubser langfristig für weitere Retro-Plattformen nutzbar bleibt.

**Why this priority**: Erweiterbarkeit sichert die strategische Lebensdauer des
Produkts, ist aber nach dem Kern- und Automatisierungsumfang nachgelagert.

**Independent Test**: Kann durch das Hinzufügen eines neuen Adapters oder
Plattformprofils getestet werden, ohne bestehende Kernregeln zu ändern.

**Acceptance Scenarios**:

1. **Given** ein neues Plattformprofil, **When** es registriert wird, **Then**
   bleiben bestehende Projekte und Bearbeitungsregeln unverändert.
2. **Given** ein neues Exportformat, **When** es hinzugefügt wird, **Then**
   bleibt die Benutzeroberfläche unverändert und der Export ist über den Kern
   erreichbar.

---

### Edge Cases

- Was passiert, wenn eine Bearbeitung außerhalb der Projektgrenzen angefordert
  wird?
- Wie wird mit nicht unterstützten Datei- oder Formattypen umgegangen?
- Wie werden widersprüchliche Farb- oder Plattformregeln behandelt?
- Wie verhält sich Undo/Redo, wenn ein mehrstufiger Workflow teilweise
  fehlschlägt?

## Requirements _(mandatory)_

### Functional Requirements

- **FR-001**: Das System muss Sprite-Projekte als fachlichen Kern unabhängig von
  einer bestimmten Benutzeroberfläche bearbeitbar machen.
- **FR-002**: Das System muss die folgenden Bearbeitungsaktionen als zentrale
  Anwendungsfälle bereitstellen: Pixel setzen, Pixel löschen, Zeichnen, Eraser,
  Auswahl, Move, Flood Fill, Clear, Fill, Linien, Spiegeln, Verschieben,
  Invertieren, Farbmodus-Umschaltung, Double Width/Height, Copy, Paste,
  Duplicate, Sortierung sowie Undo und Redo.
- **FR-003**: Das System muss den C64-Farb- und Pen-Zustand so modellieren, dass
  logische Pen-Werte gespeichert und Farben getrennt davon aufgelöst werden.
- **FR-004**: Das System muss Sprite-Animationen mit Definition, Vorschau,
  Abspielgeschwindigkeit und mehreren Animationen pro Projekt unterstützen.
- **FR-005**: Das System muss Sprite-Projekte importieren und exportieren
  können, einschließlich Spritemate SPM, SpritePad SPD 2.0, SpritePad SPR 1.8.1,
  PNG, Spritesheet-PNG, Assembler-Source und BASIC-2.0-Listen.
- **FR-006**: Das System muss Projektverwaltung für neues Projekt, Laden,
  Speichern, Autosave und Recent Files bereitstellen.
- **FR-007**: Das System muss Rendering für Desktop UI, Web UI, API Preview, MCP
  Preview und PNG-Ausgabe unterstützen.
- **FR-008**: Das System muss alle zentralen Bearbeitungsfunktionen
  programmatisch über REST API und MCP-Tools zugänglich machen.
- **FR-009**: Das System muss neue Frontends, Formate, Renderer und
  Plattformprofile als austauschbare Erweiterungspunkte unterstützen.
- **FR-010**: Das System muss Fehler fachlich explizit modellieren und
  erwartbare Fehler ohne primäre Abhängigkeit von Exceptions zurückmelden.
- **FR-011**: Das System muss mehrstufige Validierung auf Domain-, Application-
  und UI-Ebene unterstützen.
- **FR-012**: Das System muss zur Bearbeitung von C64-spezifischen Sprite-Regeln
  ein Plattformprofil verwenden, das Spritegröße, Farbmodi und Palettenregeln
  definiert.
- **FR-013**: Das System muss Undo/Redo als Teil der Application Layer
  bereitstellen und die fachlichen Operationen rückgängig machbar modellieren.
- **FR-014**: Das System muss ohne UI ausführbar und testbar sein.

### Constitution Alignment _(mandatory)_

- **CA-001 Code Quality**: Die Spezifikation verlangt klare Schichten,
  Adaptertrennung und keine fachliche Logik in UI-, API- oder MCP-Schichten.
- **CA-002 Testing**: Jede Kernfunktion benötigt automatisierte Tests auf der
  passenden Ebene; Bugfixes benötigen einen Regressionstest.
- **CA-003 UX Consistency**: Dieselbe fachliche Aktion muss in Desktop, Web, API
  und MCP dieselben Resultate und Fehlertypen liefern.
- **CA-004 Performance**: Interaktive Bearbeitung, Undo/Redo, Rendering und
  Import/Export müssen ohne wahrnehmbare Verzögerung bleiben und messbar
  abgesichert werden.

### Key Entities _(include if feature involves data)_

- **SpriteProject**: Das Projekt als fachlicher Container für Sprites,
  Animationen, Farbdefinitionen und Metadaten.
- **Sprite**: Ein einzelnes bearbeitbares Sprite mit Pixelraster, Farbzustand
  und Spriteeigenschaften.
- **Animation**: Eine wiederverwendbare Animationsdefinition für Spritefolgen
  und Timing.
- **Selection**: Eine rechteckige Auswahl, die Transformationen begrenzen kann.
- **PlatformProfile**: Ein Satz fachlicher Regeln für Spritegröße, Farbmodi und
  unterstützte Farbbelegung.
- **RenderModel**: Ein von der Domäne abgeleitetes Modell für Vorschau- und
  Exportdarstellung.
- **ImportExportAdapter**: Ein registrierbarer Adapter für spezifische Datei-
  oder Ausgabeformate.

## Success Criteria _(mandatory)_

### Measurable Outcomes

- **SC-001**: Ein vollständiger Kern-Workflow aus Projekt erstellen, Pixel
  setzen, transformieren und Vorschau erzeugen kann ohne UI in einem einzigen
  automatisierten Lauf verifiziert werden.
- **SC-002**: Dieselbe Bearbeitungsaktion liefert in Desktop, API und MCP
  äquivalente fachliche Ergebnisse und Fehlermeldungen in 100% der abgedeckten
  Testfälle.
- **SC-003**: Mindestens 80% der fachlichen Kernregeln sind durch automatisierte
  Tests auf Domain- und Application-Ebene abgesichert.
- **SC-004**: Ein neuer Export- oder Import-Adapter kann ergänzt werden, ohne
  bestehende Frontends oder Kernregeln ändern zu müssen.
- **SC-005**: Interaktive Bearbeitungsaktionen bleiben für typische
  Sprite-Projekte ohne wahrnehmbare Verzögerung bedienbar.
- **SC-006**: Die erste vollständige Referenzimplementierung deckt Bearbeitung,
  Import, Export, Rendering und Undo/Redo über den Kern ohne UI-Abhängigkeit ab.

## Assumptions

- Die erste Spezifikation fokussiert auf den C64-Kern und bereitet spätere
  Plattformprofile vor.
- Desktop, Web, API und MCP sind gleichwertige Adapter auf denselben fachlichen
  Kern.
- C64-spezifische Import- und Exportformate haben Vorrang vor späteren
  optionalen Formaten.
- Performance wird primär aus Anwendersicht über spürbare Reaktionsfähigkeit und
  stabile Workflows bewertet.
- Die initiale Version nutzt die bestehende Architekturtrennung als verbindliche
  Leitplanke für alle Folgeentscheidungen.
