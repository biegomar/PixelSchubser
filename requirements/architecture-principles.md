# PixelSchubser – Architecture Principles

## Purpose

Dieses Dokument beschreibt die verbindlichen Architekturprinzipien für
PixelSchubser.

PixelSchubser soll nicht als UI-zentrierter Port von Spritemate entstehen,
sondern als moderne, erweiterbare und automatisierbare .NET-Plattform für
Sprite-Editing.

Die Architektur muss ermöglichen:

- Avalonia Desktop UI
- späteres Web Frontend
- REST API
- MCP Server
- Headless Batch Processing
- automatisierte und KI-gestützte Workflows

---

# Architecture Vision

PixelSchubser folgt einer klaren Trennung zwischen Domäne, Anwendungsschicht,
Infrastruktur und Frontends.

Die Fachlogik muss unabhängig von jeder konkreten UI-Technologie bleiben.

Das System soll so aufgebaut sein, dass neue Frontends und
Automatisierungsschnittstellen ergänzt werden können, ohne die fachliche
Kernlogik zu verändern.

---

# Core Architectural Style

## Hexagonal Architecture

PixelSchubser wird nach dem Ports-and-Adapters-Prinzip aufgebaut.

Die Kernlogik befindet sich in der Mitte des Systems.

Äußere Technologien kommunizieren ausschließlich über definierte Ports mit dem
Kern.

```text
UI / API / MCP / CLI
        |
Application Ports
        |
Application Layer
        |
Domain Layer
        |
Infrastructure Adapters
```

---

# Layering Principles

## Domain Layer

Die Domain Layer enthält ausschließlich fachliche Konzepte und Regeln.

Erlaubt:

- Sprite
- SpriteProject
- PixelGrid
- Palette
- Animation
- Selection
- ColorMode
- SpriteMode
- Domain Services

Nicht erlaubt:

- Avalonia
- ASP.NET Core
- MCP SDK
- Dateisystemzugriffe
- HTTP
- JSON-Serialisierung
- Logging-Frameworks
- Dependency Injection Frameworks

---

## Application Layer

Die Application Layer koordiniert Use Cases.

Sie enthält:

- Commands
- Queries
- Use Cases
- Application Services
- Ports
- Undo/Redo-Koordination
- Validierung auf Anwendungsfall-Ebene

Die Application Layer kennt die Domain Layer, aber keine konkreten UI- oder
Infrastrukturtechnologien.

---

## Infrastructure Layer

Die Infrastructure Layer enthält technische Implementierungen.

Beispiele:

- Dateisystemzugriff
- Projektablage
- Einstellungen
- PNG-Encoding
- JSON-Encoding
- Logging
- externe Bibliotheken
- konkrete Import-/Export-Adapter

---

## Frontend Layers

Frontends sind Adapter.

Geplante Frontends:

- PixelSchubser.Avalonia
- PixelSchubser.Web
- PixelSchubser.Api
- PixelSchubser.Mcp
- optional PixelSchubser.Cli

Frontends dürfen keine fachliche Logik enthalten.

---

# Dependency Rule

Abhängigkeiten zeigen immer nach innen.

Erlaubt:

```text
Avalonia -> Application -> Domain
Api      -> Application -> Domain
Mcp      -> Application -> Domain
Infra    -> Application -> Domain
```

Nicht erlaubt:

```text
Domain -> Avalonia
Domain -> Api
Domain -> Mcp
Application -> Avalonia
Application -> ASP.NET Core
Application -> File System
Application -> JSON Library
```

---

# UI Independence

Alle fachlichen Funktionen müssen ohne UI ausführbar sein.

Eine Operation wie `SetPixel` darf nicht davon abhängen, ob sie durch:

- Mausinteraktion
- Tastaturkürzel
- REST API
- MCP Tool
- CLI Command
- Batch Job

ausgelöst wurde.

---

# Automation First

PixelSchubser wird so entworfen, dass alle Kernfunktionen automatisierbar sind.

Jede zentrale Benutzeraktion muss als Application Use Case existieren.

Beispiele:

- create_project
- load_project
- save_project
- set_pixel
- clear_pixel
- draw_line
- flood_fill
- flip_horizontal
- flip_vertical
- shift_left
- shift_right
- import_project
- export_project
- render_preview

---

# MCP Readiness

Der MCP Server darf keine eigene Fachlogik enthalten.

MCP Tools sind dünne Adapter auf Application Use Cases.

Beispiel:

```text
MCP Tool: set_pixel
        |
Application Use Case: SetPixelCommandHandler
        |
Domain: Sprite.SetPixel()
```

Jedes MCP Tool muss deterministisch, testbar und dokumentierbar sein.

---

# API Readiness

Die REST API darf keine eigene Fachlogik enthalten.

Controller oder Minimal API Endpoints rufen ausschließlich Application Use Cases
auf.

Die API muss dieselben Operationen bereitstellen können wie:

- Desktop UI
- MCP Server
- CLI

---

# CQRS Light

PixelSchubser verwendet kein schwergewichtiges CQRS/Event-Sourcing-Modell.

Es soll jedoch eine klare Trennung zwischen schreibenden und lesenden
Anwendungsfällen geben.

## Commands

Commands verändern Zustand.

Beispiele:

- SetPixelCommand
- FillAreaCommand
- FlipSpriteCommand
- ImportProjectCommand
- SaveProjectCommand

## Queries

Queries liefern Zustand oder Projektionen.

Beispiele:

- GetCurrentProjectQuery
- GetSpritePreviewQuery
- GetPaletteQuery
- GetAnimationPreviewQuery

---

# Undo/Redo Principle

Undo/Redo ist Teil der Application Layer.

Fachliche Operationen müssen so modelliert werden, dass sie rückgängig gemacht
werden können.

Erlaubte Ansätze:

- Command History
- Memento Snapshots
- Delta-basierte Undo-Operationen

Die konkrete Strategie kann pro Operation variieren, muss aber einheitlich über
einen Application Service bereitgestellt werden.

---

# Rendering Principle

Rendering ist von der Domäne getrennt.

Die Domain beschreibt, was gerendert werden soll.

Renderer entscheiden, wie es dargestellt wird.

```text
Domain Model -> Render Model -> Renderer
```

Geplante Renderer:

- Avalonia Renderer
- PNG Renderer
- Web Renderer
- Headless Preview Renderer

---

# Import / Export Principle

Import und Export sind Adapter auf fachliche Modelle.

Formatlogik wird getrennt von UI und Persistenz implementiert.

Beispiele:

- SPM Importer
- SPD Importer
- SPD Exporter
- PNG Exporter
- Assembler Exporter
- BASIC Exporter

Ein Exporter darf keine UI-Dialoge öffnen.

Ein Importer darf keine globalen Zustände verändern.

---

# Persistence Principle

Persistenz ist austauschbar.

Die Anwendung darf nicht direkt von einem konkreten Speichermechanismus
abhängen.

Geplante Speichermechanismen:

- lokale Projektdateien
- Autosave-Dateien
- Settings Store
- In-Memory Store für Tests
- optional SQLite
- optional Browser Storage für Web

---

# Testing Principle

Die Architektur muss testgetrieben nutzbar sein.

Pflichttests:

- Domain Unit Tests
- Application Use Case Tests
- Import/Export Golden Master Tests
- Renderer Snapshot Tests
- API Contract Tests
- MCP Tool Tests

Domain und Application müssen ohne UI testbar sein.

---

# Error Handling Principle

Fehler werden explizit modelliert.

Fachliche Fehler sollen nicht primär über Exceptions transportiert werden.

Beispiele:

- InvalidColorMode
- PixelOutOfBounds
- UnsupportedFileFormat
- InvalidSpriteDimensions
- ExportNotSupported

Exceptions sind für technische Fehler reserviert.

---

# Validation Principle

Validierung findet auf mehreren Ebenen statt.

## Domain Validation

Sichert fachliche Invarianten.

Beispiel:

- Spritebreite
- Spritehöhe
- gültige Farbwerte
- gültiger Sprite-Modus

## Application Validation

Sichert Use-Case-Anforderungen.

Beispiel:

- Projekt muss geladen sein
- Spriteindex muss existieren
- Exportformat muss unterstützt werden

## UI Validation

Sichert Benutzerführung.

Beispiel:

- deaktivierte Buttons
- Eingabefehler
- Dialoghinweise

---

# Extensibility Principle

Neue Formate, Plattformen und Frontends müssen ergänzbar sein, ohne bestehende
Kernlogik zu verändern.

Erweiterungspunkte:

- Importer
- Exporter
- Renderer
- Paletten
- Spriteformate
- Plattformprofile
- Tools
- MCP Tools

---

# Platform Profile Principle

C64-spezifische Regeln werden über ein Plattformprofil modelliert.

Beispiel:

```text
PlatformProfile
  Name: Commodore 64
  SpriteWidth: 24
  SpriteHeight: 21
  SupportedColorModes:
    - SingleColor
    - MultiColor
  Palette:
    - 16 C64 colors
```

Dadurch können später weitere Plattformen ergänzt werden.

Mögliche zukünftige Plattformen:

- Amiga
- Atari ST
- NES
- SNES
- Mega Drive
- PC Engine

---

# Configuration Principle

Konfiguration ist kein globaler Zustand.

Settings werden über klar definierte Services verwaltet.

Beispiele:

- Grid sichtbar
- Zoom-Level
- zuletzt geöffnete Dateien
- Standard-Exportformat
- Theme

---

# No Global State

Globaler Zustand ist zu vermeiden.

Nicht erlaubt:

- statische App-Instanzen
- globale CurrentProject-Variablen
- globale Service-Locator-Zugriffe
- direkte Singleton-Abhängigkeiten in der Domain

---

# Logging Principle

Logging ist Infrastruktur.

Domain Layer enthält kein Logging.

Application Layer darf abstrakte Logging Ports verwenden, wenn erforderlich.

Konkrete Logging-Frameworks werden ausschließlich in Infrastructure oder
Host-Projekten konfiguriert.

---

# Security Principle

APIs und MCP-Zugriffe müssen später absicherbar sein.

Mindestanforderungen:

- keine impliziten Dateisystemzugriffe ohne Berechtigungskonzept
- klare Workspace-Grenzen
- validierte Dateipfade
- keine beliebige Codeausführung
- klare Input-Limits
- nachvollziehbare Operationen

---

# Performance Principle

Interaktive Bearbeitung muss verzögerungsfrei möglich sein.

Performancekritische Bereiche:

- Pixelbearbeitung
- Flood Fill
- Undo/Redo
- Rendering
- Animation Preview
- Import/Export großer Projekte

Optimierungen dürfen die fachliche Trennung nicht aufheben.

---

# Documentation Principle

Architekturentscheidungen werden dokumentiert.

Zu dokumentieren sind insbesondere:

- Schichtenmodell
- Dependency Rule
- Rendering-Ansatz
- Undo/Redo-Konzept
- Import-/Export-Konzept
- MCP-Konzept
- API-Konzept
- Plattformprofil-Konzept

Geeignete Formate:

- ADR
- arc42
- C4
- Mermaid
- Spec-Kit Tasks

---

# Architecture Objectives

## AO-1 UI-Unabhängigkeit

Die Fachlogik muss unabhängig von UI-Technologien bleiben.

## AO-2 Automatisierbarkeit

Alle zentralen Bearbeitungsfunktionen müssen über API und MCP nutzbar sein.

## AO-3 Erweiterbarkeit

Neue Exportformate, Plattformprofile und Frontends müssen ohne Änderung
bestehender Kernlogik ergänzt werden können.

## AO-4 Testbarkeit

Domain und Application müssen vollständig ohne UI testbar sein.

## AO-5 Plattformunabhängigkeit

Die Anwendung muss auf Windows, Linux und macOS ausführbar sein.

---

# Architecture Key Results

## AKR-1

Domain Layer referenziert keine externen UI-, Web- oder
Infrastruktur-Frameworks.

## AKR-2

Alle zentralen Bearbeitungsaktionen existieren als Application Commands.

## AKR-3

MCP Tools rufen ausschließlich Application Use Cases auf.

## AKR-4

REST Endpoints rufen ausschließlich Application Use Cases auf.

## AKR-5

Mindestens ein Headless-Integrationstest führt einen vollständigen Workflow ohne
UI aus:

```text
Create Project
Set Pixels
Render Preview
Export PNG
Export Assembler
```

## AKR-6

Import- und Exportformate sind über registrierbare Adapter implementiert.

## AKR-7

Renderer sind austauschbar und nicht Teil der Domain Layer.

---

# Prohibited Architecture Smells

Folgende Muster sind zu vermeiden:

- God Object
- UI-zentrierte Fachlogik
- direkte Dateisystemzugriffe aus der Domain
- direkte Framework-Abhängigkeiten in der Domain
- globale App-Instanz
- statischer CurrentProject-Zustand
- Exportlogik in ViewModels
- Importlogik in Views
- MCP-spezifische Logik in der Domain
- API-spezifische Logik in der Domain

---

# Initial Project Layout

```text
src/
  PixelSchubser.Domain/
  PixelSchubser.Application/
  PixelSchubser.Formats/
  PixelSchubser.Rendering/
  PixelSchubser.Infrastructure/
  PixelSchubser.Avalonia/
  PixelSchubser.Api/
  PixelSchubser.Mcp/
  PixelSchubser.Cli/

tests/
  PixelSchubser.Domain.Tests/
  PixelSchubser.Application.Tests/
  PixelSchubser.Formats.Tests/
  PixelSchubser.Rendering.Tests/
  PixelSchubser.Api.Tests/
  PixelSchubser.Mcp.Tests/
```

---

# Definition of Done for Architecture

Eine Funktion gilt architekturell als sauber umgesetzt, wenn:

- sie über einen Application Use Case erreichbar ist
- sie ohne UI getestet werden kann
- sie keine unerlaubten Abhängigkeiten einführt
- sie über mindestens ein Frontend nutzbar ist
- sie später über API oder MCP exponierbar ist
- Fehler explizit behandelt werden
- relevante Tests vorhanden sind

---

# Final Principle

PixelSchubser ist keine Avalonia-Anwendung mit eingebauter Fachlogik.

PixelSchubser ist ein fachlicher Sprite-Editing-Kern mit mehreren austauschbaren
Bedien- und Automatisierungsadaptern.

---

# Lessons from Spritemate Reference Analysis

Die Analyse der Referenzimplementierung Spritemate (siehe
`requirements.md`, Abschnitt „Spritemate – Funktionsanalyse") hat mehrere
Konzepte bestätigt oder verfeinert, die für PixelSchubser verbindlich werden.

## LP-1 Pen-Indirektion (logische Farbwerte)

Pixel speichern **keine konkreten Farbindizes**, sondern logische Pen-Werte
(0 = transparent, 1 = Individualfarbe, 2 = Multicolor 1, 3 = Multicolor 2). Die
Auflösung Pen → konkrete Farbe ist eine **eigene Verantwortlichkeit** und gehört
in das Render Model, nicht in das Pixelraster.

Konsequenz:

- Die Domain modelliert `PixelGrid` über Pen-Werte.
- Farbauflösung erfolgt erst beim Erzeugen des Render Models.
- Dadurch bleiben Farbänderungen (global wie pro Sprite) trivial und müssen das
  Pixelraster nicht anfassen.

## LP-2 Geteilte vs. lokale Farben (Invarianten)

C64-Sprites teilen sich Hintergrund- und beide Multicolor-Farben **global**,
während die Individualfarbe (Pen 1) **pro Sprite** gilt.

- `colors[0]`, `colors[2]`, `colors[3]` sind Projektzustand (global).
- `sprite.color` ist Sprite-Zustand (lokal).
- Ein `SetPenColor`-Use-Case muss diese Unterscheidung explizit treffen.

Diese Asymmetrie ist eine fachliche Invariante und gehört in die Domain.

## LP-3 Selektionsbewusste Operationen

Transformationen (Flip, Shift, Fill, Invert) müssen optional auf eine
**rechteckige Region** (`Selection.bounds`) begrenzbar sein. Operationen
erhalten daher einen optionalen Bounds-Parameter; ohne aktive Auswahl gilt das
gesamte Sprite.

Empfehlung: Region/Bounds als Wertobjekt modellieren und als Parameter der
Commands führen, nicht als globalen UI-Zustand.

## LP-4 Multicolor als Domänenregel

Der Multicolor-Modus halbiert die horizontale Auflösung (2er-Schrittweite,
doppelte Pixelbreite, Snapping ungerader x-Koordinaten). Diese Regel ist
fachlich, nicht UI-spezifisch, und gehört in die Domain bzw. das
Plattformprofil – nicht in den Editor.

## LP-5 Byte-Encoding gehört in die Formats-/Profil-Schicht

Die C64-spezifische Kodierung (Multicolor-Bitpaare 00/10/01/11, Byte 64 mit
Multicolor-Bit 7, Overlay-Bit 4, Farbe in den unteren 4 Bits, SPD-Header,
Legacy-`.spr`-Layout) ist reine Format-/Plattformlogik.

- Pen-↔-Bit-Mapping gehört nicht in die Domain-Entities.
- Import/Export sind registrierbare Adapter (siehe „Import / Export Principle").
- Pflicht: Golden-Master-Tests gegen echte Spritemate-/SpritePad-Dateien.

## LP-6 Undo als Whole-Project-Memento (mit Vorbehalt)

Spritemate nutzt ein einfaches, robustes Memento: bei jeder Änderung wird das
komplette Projekt geklont. Das ist korrekt und gut testbar, aber speicher- und
kopierintensiv.

Richtlinie für PixelSchubser:

- Memento-Snapshots sind als Default zulässig (Korrektheit vor Optimierung).
- Bei Performanceproblemen auf Delta-/Command-basiertes Undo umstellen, jedoch
  weiterhin einheitlich über den Application-Undo-Service (siehe
  „Undo/Redo Principle").

## LP-7 Fenster-GUI als reiner Adapter

Spritemates frei verschiebbare Fenster sind ausschließlich Präsentationsbelang.
In PixelSchubser werden Panel-/Docking-Layout, Zoomstufen und Grid-Sichtbarkeit
über den Settings-Service verwaltet und sind **kein** Domänenzustand.

## LP-8 Pixelraster ist Plattformprofil-getrieben

Spritegeometrie (24×21), unterstützte Farbmodi und das Byte-Layout stammen aus
dem C64-Plattformprofil. Künftige Plattformen ändern ausschließlich das Profil,
nicht die Editier-Use-Cases.
