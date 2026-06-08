namespace PixelSchubser.Mcp.Configuration;

public sealed class McpContractCompatibilityOptions
{
    public const string SectionName = "McpCompatibility";

    public string[] SupportedVersions { get; set; } = ["1.0"];
}
