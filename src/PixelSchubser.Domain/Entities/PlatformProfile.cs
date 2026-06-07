namespace PixelSchubser.Domain.Entities;

public sealed record PlatformProfile(
    string ProfileId,
    string Name,
    int SpriteWidth,
    int SpriteHeight,
    IReadOnlyList<string> SupportedColorModes,
    IReadOnlyList<string> Palette);
