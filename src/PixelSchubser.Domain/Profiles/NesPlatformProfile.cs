using PixelSchubser.Domain.Entities;

namespace PixelSchubser.Domain.Profiles;

public static class NesPlatformProfile
{
    public static PlatformProfile CreateDefault() => new(
        ProfileId: "nes",
        Name: "Nintendo Entertainment System",
        SpriteWidth: 8,
        SpriteHeight: 8,
        SupportedColorModes: new[] { "indexed" },
        Palette: new[]
        {
            "#7C7C7C", "#0000FC", "#0000BC", "#4428BC", "#940084", "#A80020", "#A81000", "#881400",
            "#503000", "#007800", "#006800", "#005800", "#004058", "#000000", "#000000", "#000000"
        });
}
