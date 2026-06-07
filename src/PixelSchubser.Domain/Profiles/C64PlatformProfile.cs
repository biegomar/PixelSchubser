using PixelSchubser.Domain.Entities;

namespace PixelSchubser.Domain.Profiles;

public static class C64PlatformProfile
{
    public static PlatformProfile CreateDefault() => new(
        ProfileId: "c64",
        Name: "Commodore 64",
        SpriteWidth: 24,
        SpriteHeight: 21,
        SupportedColorModes: new[] { "singlecolor", "multicolor" },
        Palette: new[]
        {
            "#000000", "#FFFFFF", "#813338", "#75CEC8", "#8E3C97", "#56AC4D", "#2E2C9B", "#EDF171",
            "#8E5029", "#553800", "#C46C71", "#4A4A4A", "#7B7B7B", "#A9FF9F", "#706DEB", "#B2B2B2"
        });
}
