namespace PixelSchubser.Api.Configuration;

public sealed class ApiVersioningOptions
{
    public const string SectionName = "ApiVersioning";

    public string CurrentVersion { get; set; } = "v1";

    public string DeprecatedVersionHeader { get; set; } = "X-Api-Deprecated";

    public string VersionResponseHeader { get; set; } = "X-Api-Version";
}
