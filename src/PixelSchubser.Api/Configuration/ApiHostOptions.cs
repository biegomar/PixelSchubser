namespace PixelSchubser.Api.Configuration;

public sealed class ApiHostOptions
{
    public const string SectionName = "ApiHost";

    public int MaxPayloadLength { get; set; } = 128_000;

    public int MaxPathLength { get; set; } = 260;
}
