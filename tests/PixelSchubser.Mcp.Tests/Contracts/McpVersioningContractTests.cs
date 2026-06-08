using PixelSchubser.Mcp.Contracts;

namespace PixelSchubser.Mcp.Tests.Contracts;

public class McpVersioningContractTests
{
    [Fact]
    public void ContractVersion_IsStableV1()
    {
        Assert.Equal("1.0", McpContractVersion.Current.Version);
        Assert.Equal("pixelschubser.tools", McpContractVersion.Current.Namespace);
    }
}
