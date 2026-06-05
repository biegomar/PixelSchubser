# Data Model: PixelSchubser Plattformkern

## SpriteProject
- **Purpose**: Root aggregate for a sprite editing project.
- **Fields**: projectId, name, platformProfile, sprites, animations, globalColors,
  metadata, activeSpriteIndex, currentSelection, undoState.
- **Relationships**: Contains many Sprites and Animations; references one
  PlatformProfile.
- **Validation**: Must have at least one Sprite; must obey platform sprite size
  and color constraints.

## Sprite
- **Purpose**: A single editable sprite.
- **Fields**: spriteId, name, width, height, penGrid, color, multicolor,
  doubleWidth, doubleHeight, overlayEnabled.
- **Relationships**: Belongs to one SpriteProject.
- **Validation**: Pen grid dimensions must match the platform profile; pen values
  must be valid logical pens.

## Animation
- **Purpose**: A sequence definition used for playback and preview.
- **Fields**: animationId, name, startSpriteIndex, endSpriteIndex, fps, mode,
  doubleWidth, doubleHeight.
- **Relationships**: Belongs to one SpriteProject and references sprites by
  index.
- **Validation**: Start index must be less than or equal to end index; FPS must
  be positive.

## Selection
- **Purpose**: A rectangular active region used to bound transformations.
- **Fields**: x1, y1, x2, y2, isActive.
- **Relationships**: Exists as transient project/editor state.
- **Validation**: Coordinates must define a valid rectangle within sprite bounds.

## PlatformProfile
- **Purpose**: Defines the sprite rules for a target platform.
- **Fields**: profileId, name, spriteWidth, spriteHeight, supportedColorModes,
  palette, multicolorRules.
- **Relationships**: Referenced by SpriteProject.
- **Validation**: Must define a consistent size and color configuration.

## RenderModel
- **Purpose**: Presentation model derived from domain state for previews and
  export output.
- **Fields**: renderedSprites, paletteMapping, previewMetadata.
- **Relationships**: Derived from SpriteProject; not persisted as domain state.
- **Validation**: Must resolve pen values to concrete display colors before
  rendering.

## ImportExportAdapter
- **Purpose**: Adapter contract for format-specific import/export behavior.
- **Fields**: adapterName, supportedFormats, direction, priority.
- **Relationships**: Consumes and produces SpriteProject or render/export data.
- **Validation**: Must not mutate global state outside the project boundary.

## State Transitions
- Create project -> empty project with platform profile defaults
- Edit sprite -> update pen grid and/or sprite properties
- Apply selection-based transform -> mutate only selected bounds
- Undo -> restore previous snapshot or delta
- Redo -> reapply the reverted state
- Import -> replace or merge project state according to adapter rules
- Export -> emit a format-specific artifact without changing domain state
