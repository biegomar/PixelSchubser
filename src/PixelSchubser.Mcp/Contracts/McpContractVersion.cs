namespace PixelSchubser.Mcp.Contracts;

public sealed record McpContractVersion(string Namespace, string Version)
{
    public static McpContractVersion Current { get; } = new("pixelschubser.tools", "1.0");
}
