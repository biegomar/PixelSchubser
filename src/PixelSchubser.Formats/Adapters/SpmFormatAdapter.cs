namespace PixelSchubser.Formats.Adapters;

public sealed class SpmFormatAdapter
{
    public const string FormatId = "spm";

    public string Export(string payload) => $"SPM:{payload}";

    public string Import(string serialized) => serialized.Replace("SPM:", string.Empty, StringComparison.Ordinal);
}
