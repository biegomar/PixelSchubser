namespace PixelSchubser.Application.Services;

public sealed record WorkspaceSnapshot(
    string ProjectId,
    string Name,
    string ProfileId,
    string ProfileName,
    int SpriteWidth,
    int SpriteHeight,
    IReadOnlyList<string> Palette,
    IReadOnlyList<WorkspaceSpriteSnapshot> Sprites,
    IReadOnlyList<WorkspaceAnimationSnapshot> Animations);

public sealed record WorkspaceSpriteSnapshot(
    string Name,
    int Width,
    int Height,
    IReadOnlyList<int> Pixels);

public sealed record WorkspaceAnimationSnapshot(
    string Name,
    int StartSpriteIndex,
    int EndSpriteIndex,
    int Fps,
    string Mode);
