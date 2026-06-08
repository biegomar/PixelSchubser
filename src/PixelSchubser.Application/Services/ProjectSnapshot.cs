namespace PixelSchubser.Application.Services;

public sealed record ProjectSnapshot(
    string ProjectId,
    string Name,
    int SpriteCount,
    int AnimationCount,
    int Width,
    int Height);
