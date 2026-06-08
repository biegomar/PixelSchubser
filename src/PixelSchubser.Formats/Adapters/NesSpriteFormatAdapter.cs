namespace PixelSchubser.Formats.Adapters;

public sealed class NesSpriteFormatAdapter
{
    public const string FormatId = "nes-sprite";

    public string Export(string payload) => $"NESSPR:{payload}";
}
