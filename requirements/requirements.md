# PixelSchubser – Product & Architecture Specification

## Overview

PixelSchubser ist ein plattformübergreifender Sprite-Editor mit Schwerpunkt auf
Commodore-64-Spritegrafiken.

Das Projekt ist von Spritemate inspiriert, wird jedoch nicht als direkte
Architekturportierung umgesetzt. Ziel ist eine moderne, erweiterbare und
automatisierbare Softwareplattform auf Basis von .NET 10.

PixelSchubser soll sowohl als Desktop-Anwendung als auch als Web-Anwendung
betrieben werden können. Zusätzlich soll die gesamte Funktionalität über APIs
und einen MCP-Server automatisierbar sein.

---

# Problem Statement

Spritemate ist eine erfolgreiche browserbasierte Anwendung für die Bearbeitung
von C64-Sprites.

Die bestehende Architektur ist stark UI-zentriert und auf die Browserplattform
zugeschnitten:

- Fachlogik und UI sind eng gekoppelt
- Browser-APIs werden direkt verwendet
- Persistenz erfolgt über Local Storage
- Import- und Exportlogik sind teilweise mit UI-Code vermischt
- Keine Headless-Nutzung möglich
- Keine Automatisierung über APIs
- Keine Agentenintegration möglich

Diese Architektur erschwert:

- Desktop-Anwendungen
- Wiederverwendung der Fachlogik
- Automatisierung
- KI-gestützte Workflows
- Web-API-Bereitstellung
- MCP-Integration

---

# Vision

PixelSchubser soll eine moderne Sprite-Editing-Plattform werden.

Die Spritebearbeitung soll unabhängig von der Benutzeroberfläche implementiert
werden.

Alle Funktionen müssen gleichermaßen nutzbar sein über:

- Avalonia Desktop UI
- Web Frontend
- REST API
- MCP Server
- Headless Batch Processing

Die UI darf lediglich ein Adapter auf die Fachlogik sein.

---

# Business Goals

## BG-1

Bereitstellung eines modernen Sprite-Editors für Retro-Plattformen.

## BG-2

Wiederverwendbare Fachlogik unabhängig von der UI.

## BG-3

Vollständige Automatisierbarkeit aller Bearbeitungsfunktionen.

## BG-4

Unterstützung KI-gestützter Workflows über MCP.

## BG-5

Plattformübergreifende Nutzung auf Windows, Linux und macOS.

## BG-6

Erweiterbarkeit für weitere Retro-Systeme.

---

# Functional Requirements

## FR-1 Sprite Editing

Das System muss die Bearbeitung von C64-Sprites unterstützen.

### Features

- Pixel setzen
- Pixel löschen (auch Shift+Maus)
- Zeichnen
- Eraser
- Auswahlwerkzeug (rechteckig, begrenzt alle Transformationen)
- Move
- Flood Fill
- Clear / Fill
- Linien
- Spiegeln (horizontal/vertikal)
- Verschieben/Rotieren (links/rechts/oben/unten)
- Invertieren
- Single-/Multicolor-Umschaltung
- Double Width / Double Height
- Overlay (Onion-Skinning)
- Copy / Paste / Duplicate
- Sprite-Sortierung (Drag & Drop)
- Undo
- Redo

---

## FR-2 Color Management

Das System muss die originale C64-Farbpalette unterstützen.

### Features

- Hintergrundfarbe (global, Pen 0)
- Individualfarbe (pro Sprite, Pen 1)
- Multicolor 1 / Multicolor 2 (global, Pen 2/3)
- Mehrere Palettenvarianten (Colodore, PALette, Pepto, Custom)
- Pen-Indirektion (Pixel speichern Pen-Werte, nicht Farbindizes)
- Multicolor-Modus
- Farbvalidierung (C64-Restriktionen)

---

## FR-3 Animation Support

Das System muss Spriteanimationen unterstützen.

### Features

- Animationsdefinition
- Vorschau
- Abspielgeschwindigkeit
- Mehrere Animationen pro Projekt

---

## FR-4 Import

Das System muss Sprite-Projekte importieren können.

### Initiale Formate

- Spritemate SPM
- SpritePad SPD (2.0)
- SpritePad SPR (1.8.1, legacy)
- VICE Snapshot VSF (Stretch Goal)

---

## FR-5 Export

Das System muss Sprite-Projekte exportieren können.

### Initiale Formate

- Spritemate SPM
- SpritePad SPD (2.0 & 1.8.1)
- PNG (Einzel-Sprite)
- PNG Spritesheet
- Assembler Source (KICK ASS & ACME, Hex/Binär)
- BASIC 2.0 DATA Listing

---

## FR-6 Project Management

Das System muss Sprite-Projekte verwalten können.

### Features

- Neues Projekt
- Laden
- Speichern
- Autosave
- Recent Files

---

## FR-7 Rendering

Das System muss Spritegrafiken rendern können.

### Ausgabeziele

- Desktop UI
- Browser UI
- PNG Export
- API Preview
- MCP Preview

---

## FR-8 Automation

Alle Bearbeitungsfunktionen müssen programmatisch nutzbar sein.

### Beispiele

- Sprite generieren
- Import durchführen
- Export durchführen
- Rendern
- Batch-Bearbeitung

---

# MCP Requirements

## MCP-1

Ein MCP-Server muss bereitgestellt werden.

## MCP-2

Der MCP-Server muss die Fachlogik direkt nutzen.

## MCP-3

Es darf keine UI-Abhängigkeiten geben.

## MCP-4

Folgende Werkzeuge sollen bereitgestellt werden:

- create_project
- load_project
- save_project
- import_spd
- export_spd
- export_png
- export_asm
- set_pixel
- clear_pixel
- draw_line
- flood_fill
- flip_horizontal
- flip_vertical
- render_preview

---

# API Requirements

## API-1

Eine REST API muss bereitgestellt werden.

## API-2

Alle Fachfunktionen müssen über die API erreichbar sein.

## API-3

Die API muss dieselben Use Cases verwenden wie die Desktop-Anwendung.

---

# Architectural Requirements

## AR-1 Clean Separation

Fachlogik darf keine Kenntnisse besitzen über:

- Avalonia
- Browser APIs
- REST APIs
- MCP APIs

---

## AR-2 Headless Core

Der Anwendungskern muss ohne UI ausführbar sein.

---

## AR-3 Hexagonal Architecture

Das System soll nach dem Ports-and-Adapters-Prinzip aufgebaut werden.

---

## AR-4 Domain-Centric Design

Spritebearbeitung muss innerhalb der Domäne implementiert werden.

---

## AR-5 Replaceable Frontends

Frontends müssen austauschbar sein.

---

# Proposed Solution Structure

```text
PixelSchubser.Domain
PixelSchubser.Application
PixelSchubser.Formats
PixelSchubser.Rendering
PixelSchubser.Infrastructure
PixelSchubser.Avalonia
PixelSchubser.Api
PixelSchubser.Mcp
```

---

# Domain Model

## SpriteProject

Enthält:

- Sprites
- Animationen
- Farbdefinitionen
- Metadaten

## Sprite

Enthält:

- Pixelmatrix
- Farben
- Spriteeigenschaften

## Animation

Enthält:

- Startsprite
- Endsprite
- Timing

## Selection

Enthält:

- Aktuelle Auswahl

---

# Non Functional Requirements

## NFR-1 Testability

Mindestens 80% Testabdeckung in:

- Domain
- Application

---

## NFR-2 Platform Independence

Unterstützte Plattformen:

- Windows
- Linux
- macOS

---

## NFR-3 Performance

Bearbeitung muss ohne wahrnehmbare Verzögerung erfolgen.

---

## NFR-4 Maintainability

Alle Fachlogik muss unabhängig von UI-Technologien bleiben.

---

## NFR-5 Extensibility

Neue Exportformate müssen ohne Änderungen an bestehenden Frontends integrierbar
sein.

---

# Future Roadmap

## Phase 1

- Domain Model
- Import/Export
- Rendering Core

## Phase 2

- Avalonia Desktop Application (siehe „Avalonia UI App – Vollständige
  Spezifikation"): erstes vollständiges Referenz-Frontend mit
  Spritemate-Funktionsparität

## Phase 3

- REST API

## Phase 4

- MCP Server

## Phase 5

- Web Frontend

## Phase 6

- Unterstützung weiterer Plattformen

Mögliche Plattformen:

- Amiga
- Atari ST
- NES
- SNES
- Mega Drive
- PC Engine

---

# Spritemate – Funktionsanalyse (Referenzimplementierung)

Dieser Abschnitt dokumentiert das Ergebnis der Analyse der Web-Anwendung
Spritemate (<https://www.spritemate.com>, Repository
<https://github.com/Esshahn/spritemate>). Er dient als fachliche Referenz für
die PixelSchubser-Domäne und für die erste vollständige Avalonia-UI-App.

Spritemate ist eine reine Client-Anwendung (TypeScript + Vite, HTML5 Canvas, kein
Framework). Die gesamte Fachlogik liegt in ca. 18 TypeScript-Modulen. Persistenz
erfolgt ausschließlich über `localStorage` (Fenster-Layout, Settings, Autosave)
und Datei-Download/-Upload. Es gibt keinen Server.

## SA-1 Datenmodell (Source of Truth)

Spritemate hält den gesamten Zustand in einem `SpriteCollection`-Objekt (im Code
`all`).

```text
SpriteCollection
  version          : string
  filename         : string                 (ohne Endung)
  colors           : { 0: int, 2: int, 3: int }   global geteilt
  sprites          : SpriteData[]
  current_sprite   : int                     Index in sprites
  pen              : int                      0..3 (aktueller Zeichenwert)
  animation        : AnimationData            (global, Legacy-Fallback)
  playfield        : { backgroundColor: int, sprites: [...] }

SpriteData
  name       : string
  color      : int        individuelle Farbe (Pen 1), Paletten-Index 0..15
  multicolor : bool
  double_x   : bool
  double_y   : bool
  overlay    : bool
  pixels     : int[21][24]   [y][x], Werte 0..3
  animation? : AnimationData  (pro Sprite, optional)

AnimationData
  startSprite : int
  endSprite   : int
  fps         : int
  mode        : "restart" | "pingpong"
  doubleX     : bool
  doubleY     : bool
```

### Pixelwerte (Pen-Modell)

Jeder Pixel speichert **keinen Farbindex**, sondern eine logische Pen-Nummer:

| Pen | Bedeutung               | Farbe stammt aus           |
| --- | ----------------------- | -------------------------- |
| 0   | transparent/Hintergrund | `colors[0]` (global)       |
| 1   | Individualfarbe         | `sprite.color` (pro Sprite)|
| 2   | Multicolor 1            | `colors[2]` (global)       |
| 3   | Multicolor 2            | `colors[3]` (global)       |

Wichtige Konsequenz für die Domäne:

- Pen 0/2/3 verweisen auf **global geteilte** Farben. Ändert man Multicolor 1,
  ändern sich alle Multicolor-Sprites.
- Pen 1 ist **pro Sprite** unterschiedlich.
- `set_pen_color`: bei Pen 1 wird `sprite.color` gesetzt, sonst `colors[pen]`.

### Single- vs. Multicolor

- Singlecolor: 24×21 Pixel, jeder Pixel ist 0 oder 1.
- Multicolor: logisch 12×21, technisch werden im 24er-Raster **Pixelpaare**
  benutzt (Schrittweite 2, ungerades x wird auf das vorige gerade x gesnappt).
  Pen-Werte 0..3 sind möglich; ein Multicolor-Pixel ist doppelt breit.

## SA-2 C64-Farbpalette

- 16 Farben, Index 0..15.
- Vier auswählbare Palettenvarianten mit identischer Semantik, nur andere
  RGB-Werte: **Colodore**, **PALette**, **Pepto** (Default), **Custom**.
- Farbnamen (Index 0..15): black, white, red, cyan, purple, green, blue, yellow,
  orange, brown, pink, dark grey, grey, light green, light blue, light grey.
- Default-Farben: Hintergrund 11 (light grey), Multicolor 1 = 8 (orange),
  Multicolor 2 = 6 (blue), Individualfarbe 1 (white), Default-Pen = 1.

## SA-3 Werkzeuge und Bearbeitungsoperationen

Alle Operationen wirken auf das aktuelle Sprite und sind selektionsbewusst:
existiert eine aktive Rechteck-Auswahl (`selection.bounds = {x1,y1,x2,y2}`),
wird die Operation auf diese Region beschränkt, sonst auf das ganze Sprite.

| Operation                | Verhalten                                                            |
| ------------------------ | ------------------------------------------------------------------- |
| Draw (`set_pixel`)       | setzt Pen-Wert; Shift = transparent (löschen); Multicolor-Snap auf x|
| Eraser                   | wie Draw mit Pen 0                                                   |
| Flood Fill (`floodfill`) | 4er-Nachbarschaft, Multicolor-Schrittweite 2, selektionsbegrenzt    |
| Clear                    | füllt Sprite mit 0                                                   |
| Fill                     | füllt Sprite/Selektion mit aktuellem Pen                            |
| Flip horizontal          | spiegelt Zeilen (Multicolor: paarweise)                             |
| Flip vertical            | spiegelt Zeilenreihenfolge                                          |
| Shift left/right         | rotierende Verschiebung (Multicolor: 2er-Schritte)                  |
| Shift up/down            | rotierende Zeilenverschiebung                                       |
| Invert                   | Pen-Mapping 0↔1, 2↔3                                                 |
| Move                     | freies Verschieben des Sprite-Inhalts                               |
| Select                   | rechteckige Auswahl; begrenzt alle Transformationen                 |
| Toggle Multicolor        | Umschalten Single/Multicolor (Pen 2/3 → Pen 1, wenn nötig)          |
| Toggle Double X / Y      | Sprite-Streckung (VIC-Register)                                     |
| Toggle Overlay           | Sprite als Overlay-Layer markieren (Onion-Skinning)                 |

## SA-4 Sprite-Management

- Mehrere Sprites pro Projekt, `current_sprite` als aktiver Index.
- New, Delete (mind. 1 Sprite bleibt), Copy, Paste, Duplicate.
- Navigation: cursor links/rechts zyklisch durch die Liste.
- Sortierung per Drag & Drop (Sortable), `sort_spritelist` ordnet das Array um
  und hält `current_sprite` konsistent.
- Sprite-Namen sind editierbar und werden als Labels in ASM/BASIC-Export genutzt.

## SA-5 Undo/Redo

- Vollständiges Snapshot-Modell: bei jeder zustandsändernden Operation wird das
  komplette `all`-Objekt per Deep Clone in einen Backup-Stack geschoben
  (`backup[]`, `backup_position`).
- Undo/Redo bewegt nur den Zeiger und klont den Snapshot zurück.
- `can_undo` / `can_redo` steuern UI-Buttonzustände.

## SA-6 Fenster-basierte GUI

Spritemate besteht aus frei verschiebbaren Fenstern (Drag, Zoom, teils
schließbar). Layout, Positionen und Zoomstufen werden in `localStorage`
persistiert.

| Fenster        | Inhalt / Funktion                                                       |
| -------------- | ---------------------------------------------------------------------- |
| Tools          | Draw, Fill, Eraser, Move, Select, Undo, Redo, Load, Save               |
| Editor         | 24×21-Zeichenfläche, Grid-Toggle, Zoom, Multicolor-Toggle, Shift/Flip  |
| Preview        | 1:1-Vorschau, Double-X/Y-Stretch, Overlay nächstes Sprite, PNG via RMB |
| Sprite List    | Thumbnails aller Sprites, New/Delete/Copy/Paste, Drag-Sort, Zoom       |
| Palette        | 16 C64-Farben, Pen-Auswahl (1=Farbe,2=transparent,3=mc1,4=mc2)         |
| Animation      | startSprite, endSprite, fps, mode (restart/pingpong), Double-X/Y, Play |
| Playfield      | Komposition mehrerer Sprites auf Hintergrundfarbe                      |
| Snapshot (VICE)| Speichermonitor zum Extrahieren von Sprites aus `.vsf`-Dumps           |

## SA-7 Datei-Formate (präzise)

### SPM (Spritemate, nativ)

- JSON-Serialisierung von `SpriteCollection`.
- Legacy-Migration beim Laden: Schlüssel `t/i/m1/m2` → `0/1/2/3`.

### SPD (SpritePad 2.0, „new")

```text
00..02  "SPD" (83,80,68)
03      Version (1)
04      Anzahl Sprites - 1
05      Anzahl Animationen (0)
06      Farbe transparent (colors[0])
07      Farbe Multicolor 1 (colors[2])
08      Farbe Multicolor 2 (colors[3])
09..    je Sprite 63 Bytes Pixeldaten + 1 Byte Flags/Farbe (= 64)
...     Trailer 00,00,01,00 (SpritePad-Animationsinfo, ungenutzt)
```

### SPR / SPD 1.8.1 (legacy, „old")

```text
00      Farbe transparent
01      Farbe Multicolor 1
02      Farbe Multicolor 2
03..    je Sprite 64 Bytes (Anzahl = (len-3)/64)
```

### Byte 64 (Flags/Farbe je Sprite)

```text
Bit 7      Multicolor (1 = multicolor)
Bit 4      Overlay
Bits 0..3  Individualfarbe (low nibble)
```

### Multicolor-Bit-Encoding (Export/Import)

```text
Pen 0 -> 00    Pen 1 -> 10    Pen 2 -> 01    Pen 3 -> 11
Singlecolor: Pen != 0 -> 1, Pen 0 -> 0
```

### Weitere Formate

- **VSF**: VICE-Snapshot-Import zur Sprite-Extraktion.
- **ASM**: KICK ASS (`//` Kommentar, `.byte`, Label `name:`) und ACME (`;`
  Kommentar, `!byte`, Label `name`), Hex- oder Binärnotation. Enthält
  `LDA/STA $D025/$D026` für Multicolor und pro Sprite Byte 64.
- **BASIC 2.0**: lauffähiges Listing mit `POKE`/`DATA`, max. 8 Sprites sichtbar.
- **PNG**: Einzel-Sprite (RMB auf Preview) und Spritesheet-Export.

## SA-8 VICE Snapshot Monitor

Ein Speichermonitor (Autor Elliot Tanner) zum Extrahieren von Sprites aus
VICE-`.vsf`-Snapshots. Kommandos: `help`, `mem n`, `edit n v...`, `vic`,
`vid <bank>`, `cia`, `sprites`, `grab <n>`, `grabmem <n>`, `grabcols`.

> Hinweis: Für die erste PixelSchubser-Avalonia-App ist der Snapshot-Monitor
> optional (Stretch Goal). Er ist als eigenständiger Application-Use-Case
> modellierbar, aber nicht Teil des MVP.

## SA-9 Tastaturkürzel (Auszug)

| Taste         | Funktion                                       |
| ------------- | ---------------------------------------------- |
| D             | Draw                                            |
| F             | Flood Fill                                      |
| E             | Eraser                                          |
| M             | Move / Multicolor-Toggle (kontextabhängig)     |
| Z / Shift+Z   | Undo / Redo                                     |
| 1,2,3,4       | Pen: Individualfarbe, transparent, mc1, mc2     |
| Shift + Maus  | Pixel löschen                                   |
| Cursor ←/→    | Vorheriges / nächstes Sprite                    |

---

# Avalonia UI App – Vollständige Spezifikation (erstes Referenz-Frontend)

Dieses Kapitel definiert die **erste vollständige PixelSchubser-Avalonia-App**.
Sie deckt die Funktionalität von Spritemate sehr genau ab und ist gleichzeitig
das erste lauffähige Beispiel der hexagonalen Architektur: Die UI ist ein reiner
Adapter auf die Application-Use-Cases. Sie enthält **keine** Fachlogik (siehe
`architecture-principles.md`, „Final Principle").

## AV-0 Zielsetzung

- Funktionsparität mit Spritemate für den C64-Sprite-Editing-Kern.
- MVVM mit strikter Trennung: Views (AXAML) → ViewModels → Application Ports.
- Jede Benutzeraktion ruft genau einen Application Command/Query auf.
- Vollständig ohne UI testbar: jede UI-Aktion existiert auch als Use Case.

## AV-1 Technischer Rahmen

- .NET 10, Avalonia UI (aktuelles Stable), MVVM (z. B. CommunityToolkit.Mvvm).
- Rendering der Zeichenfläche über `Canvas` / eigenes `Control` mit
  `OnRender`/`DrawingContext` oder `WriteableBitmap` für Performance.
- DI über `Microsoft.Extensions.DependencyInjection` im Host-Projekt
  `PixelSchubser.Avalonia` (nicht in Domain/Application).
- Plattformen: Windows, Linux, macOS (Desktop-Profil).

## AV-2 Fensterlayout / Panels

Spritemates frei verschiebbare Fenster werden in Avalonia als **Dock-/Panel-
basiertes Layout** umgesetzt (z. B. Dock.Avalonia oder ein eigenes Grid mit
verschiebbaren `Window`-/`Flyout`-Panels). MVP: feste Docking-Bereiche mit
optionaler Floating-Erweiterung.

Erforderliche Panels:

1. **Toolbar/Menu** – Datei (New, Load, Save/Export), Undo/Redo, Werkzeugauswahl.
2. **Editor** – Hauptzeichenfläche 24×21, Zoom, Grid, Multicolor-Toggle,
   Shift/Flip/Invert-Buttons.
3. **Preview** – 1:1-Vorschau inkl. Double-X/Y, Overlay, PNG-Export (Kontextmenü).
4. **Sprite List** – Thumbnails, New/Delete/Copy/Paste/Duplicate, Drag-Reorder.
5. **Palette** – 16 Farben, Pen-Auswahl, Anzeige der aktuellen Sprite-Farben.
6. **Animation** – Start/End-Sprite, FPS, Mode, Double-X/Y, Play/Stop-Vorschau.
7. **Playfield** (optional MVP+) – Komposition mehrerer Sprites.
8. **Snapshot Monitor** (Stretch Goal) – VICE-`.vsf`-Extraktion.

## AV-3 Funktionale Anforderungen der Avalonia-App

### FR-AV-1 Editor / Zeichnen

- Pixelgenaues Zeichnen per Maus (Klick + Ziehen).
- Werkzeuge: Draw, Eraser, Flood Fill, Move, Select (Rechteck).
- Shift+Maus = löschen (Pen 0).
- Multicolor-Modus mit korrektem 2er-Pixel-Snapping und doppelter Pixelbreite.
- Grid ein/aus, Zoomstufen (analog Spritemate-Limits) mit gespeichertem Zustand.
- Live-Aktualisierung von Preview und Sprite-List-Thumbnail bei jeder Änderung.

### FR-AV-2 Farb- und Pen-Verwaltung

- Palettenauswahl (Colodore, PALette, Pepto, Custom).
- Pen-Auswahl 0..3 (transparent, Individualfarbe, mc1, mc2).
- Farbzuweisung gemäß Pen-Semantik: Pen 1 → Sprite-Farbe, Pen 0/2/3 → globale
  Farben.
- C64-Restriktionen sichtbar machen (Anzahl Farben je Sprite, globale Farben).

### FR-AV-3 Transformationen

- Flip horizontal/vertical, Shift left/right/up/down (rotierend), Invert,
  Clear, Fill – jeweils selektionsbewusst.
- Toggle Double X / Double Y, Toggle Multicolor, Toggle Overlay.

### FR-AV-4 Sprite-Verwaltung

- New, Delete, Copy, Paste, Duplicate, Rename.
- Auswahl des aktiven Sprites per Klick und per Cursor ←/→.
- Drag-&-Drop-Sortierung der Sprite-Liste.

### FR-AV-5 Undo/Redo

- Globaler Undo/Redo-Stack über einen Application-Service (nicht im ViewModel).
- Button-/Menüzustände aus `CanUndo`/`CanRedo`.

### FR-AV-6 Animation

- Animationsbereich (Start/End-Sprite), FPS, Mode (restart/pingpong),
  Double-X/Y.
- Echtzeit-Vorschau mit Play/Stop, einstellbarer Abspielgeschwindigkeit.

### FR-AV-7 Import

- SPM, SPD 2.0, SPD 1.8.1 (SPR). VSF und PNG als Stretch Goal.
- Datei-Auswahl über Avalonia `StorageProvider` (kein direkter Domain-Zugriff
  auf das Dateisystem).

### FR-AV-8 Export

- SPM, SPD 2.0, SPD 1.8.1, ASM (KICK + ACME, Hex/Binär), BASIC 2.0, PNG,
  Spritesheet-PNG.
- Speicher-Dialog über `StorageProvider`; Formatlogik aus
  `PixelSchubser.Formats`.

### FR-AV-9 Persistenz / Settings

- Fenster-/Panel-Layout, Zoomstufen, gewählte Palette, Default-Exportformat und
  Recent Files persistent (Infrastructure Settings Store, nicht localStorage).
- Autosave des aktuellen Projekts.

### FR-AV-10 Tastatursteuerung

- Vollständige Shortcut-Abdeckung gemäß SA-9 über Avalonia `KeyBindings`.

## AV-4 MVVM-Abbildung auf die Architektur

```text
View (AXAML)            ->  ViewModel (Avalonia)       ->  Application Use Case
EditorView pointer down ->  EditorViewModel.Draw       ->  SetPixelCommand
Fill button             ->  EditorViewModel.Fill       ->  FloodFillCommand
Flip H button           ->  EditorViewModel.FlipH      ->  FlipHorizontalCommand
Pen 1..3 / color click  ->  PaletteViewModel.SetPen    ->  SetPenCommand / SetPenColorCommand
New sprite              ->  SpriteListViewModel.New     ->  NewSpriteCommand
Reorder drag            ->  SpriteListViewModel.Sort    ->  SortSpritesCommand
Undo / Redo             ->  ShellViewModel.Undo/Redo    ->  UndoCommand / RedoCommand
Animation play          ->  AnimationViewModel.Play     ->  GetAnimationPreviewQuery
Load / Save             ->  ShellViewModel.Load/Save    ->  ImportProjectCommand / ExportProjectCommand
Editor/Preview render   ->  *ViewModel.Render           ->  RenderPreviewQuery (Render Model)
```

Regeln:

- ViewModels halten **keine** Spielregeln; sie übersetzen UI-Ereignisse in
  Commands/Queries und projizieren Ergebnis-Modelle für das Binding.
- Rendering nutzt ein UI-unabhängiges **Render Model** (siehe
  „Rendering Principle"); der Avalonia-Renderer zeichnet nur.
- Keine Import-/Exportlogik in Views/ViewModels (siehe „Prohibited Smells").

## AV-5 Definition of Done (Avalonia MVP)

Die Avalonia-App gilt als fertig, wenn:

- alle FR-AV-1..FR-AV-8 über Application-Use-Cases erreichbar sind,
- ein Sprite vollständig erstellt, transformiert, animiert und exportiert werden
  kann (SPM, SPD, ASM, BASIC, PNG),
- Import von SPM/SPD/SPR funktioniert und mit Spritemate-Dateien kompatibel ist
  (Golden-Master-Tests),
- Undo/Redo zuverlässig über alle Operationen funktioniert,
- kein ViewModel/keine View Fachlogik enthält,
- dieselben Use Cases später unverändert von API und MCP nutzbar sind.

## AV-6 Scope-Abgrenzung MVP

| In Scope (MVP)                                   | Stretch Goal / später          |
| ------------------------------------------------ | ------------------------------ |
| Editor, Palette, Preview, Sprite List, Animation | VICE Snapshot Monitor          |
| Single-/Multicolor, alle Transformationen        | Playfield-Komposition          |
| Import SPM/SPD/SPR, Export SPM/SPD/ASM/BASIC/PNG  | Import VSF, Import PNG          |
| Undo/Redo, Copy/Paste/Duplicate, Sortierung      | Floating-/frei verschiebbare Fenster |
| Docking-Layout mit gespeicherten Zoom-/Grid-States| Onion-Skinning-Multi-Overlay   |
