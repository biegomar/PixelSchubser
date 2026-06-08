using PixelSchubser.Application.Services;
using PixelSchubser.Mcp.Errors;
using PixelSchubser.Mcp.Tools;

namespace PixelSchubser.Mcp.Tests.Integration;

public class McpCoreParityTests
{
    [Fact]
    public void McpSetPixel_MatchesCoreServiceState()
    {
        var core = new InMemoryProjectAutomationService();
        var mcpService = new InMemoryProjectAutomationService();
        var tools = new SpriteEditTools(mcpService, new McpErrorMapper());

        var coreId = core.CreateProject("shared").Value!;
        var mcpId = mcpService.CreateProject("shared").Value!;

        core.SetPixel(coreId, 0, 1, 1, 2);
        tools.SetPixel(mcpId, 0, 1, 1, 2);

        Assert.Equal(core.GetPreview(coreId).Value!, mcpService.GetPreview(mcpId).Value!);
    }

    [Fact]
    public void McpShiftLeft_MatchesCoreServiceState()
    {
        var core = new InMemoryProjectAutomationService();
        var mcpService = new InMemoryProjectAutomationService();
        var tools = new SpriteEditTools(mcpService, new McpErrorMapper());

        var coreId = core.CreateProject("shared").Value!;
        var mcpId = mcpService.CreateProject("shared").Value!;

        core.SetPixel(coreId, 0, 2, 0, 3);
        mcpService.SetPixel(mcpId, 0, 2, 0, 3);

        core.ShiftLeft(coreId, 0);
        tools.ShiftLeft(mcpId, 0);

        Assert.Equal(core.GetWorkspace(coreId).Value!.Sprites[0].Pixels, mcpService.GetWorkspace(mcpId).Value!.Sprites[0].Pixels);
    }

    [Fact]
    public void McpMoveSelection_MatchesCoreServiceState()
    {
        var core = new InMemoryProjectAutomationService();
        var mcpService = new InMemoryProjectAutomationService();
        var tools = new SpriteEditTools(mcpService, new McpErrorMapper());

        var coreId = core.CreateProject("shared").Value!;
        var mcpId = mcpService.CreateProject("shared").Value!;

        core.SetPixel(coreId, 0, 0, 0, 7);
        mcpService.SetPixel(mcpId, 0, 0, 0, 7);

        core.MoveSelection(coreId, 0, 0, 0, 0, 0, 1, 1);
        tools.MoveSelection(mcpId, 0, 0, 0, 0, 0, 1, 1);

        Assert.Equal(core.GetWorkspace(coreId).Value!.Sprites[0].Pixels, mcpService.GetWorkspace(mcpId).Value!.Sprites[0].Pixels);
    }
}
