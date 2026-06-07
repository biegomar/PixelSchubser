namespace PixelSchubser.Formats.Adapters;

public sealed class SpritePadFormatAdapter
{
    public const string SpdFormatId = "spd";
    public const string SprFormatId = "spr";

    public string ExportSpd(string payload) => $"SPD:{payload}";

    public string ExportSpr(string payload) => $"SPR:{payload}";
}
