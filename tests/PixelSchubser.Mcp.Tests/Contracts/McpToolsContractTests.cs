using PixelSchubser.Application.Services;
using PixelSchubser.Mcp.Errors;
using PixelSchubser.Mcp.Tools;

namespace PixelSchubser.Mcp.Tests.Contracts;

public class McpToolsContractTests
{
    [Fact]
    public void ProjectTool_CreateProject_ReturnsSuccessPayload()
    {
        var tools = new ProjectTools(new InMemoryProjectAutomationService(), new McpErrorMapper());

        var result = tools.CreateProject("mcp");

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
    }

    [Fact]
    public void SpriteEditTool_SetPixel_ReturnsSuccess()
    {
        var service = new InMemoryProjectAutomationService();
        var mapper = new McpErrorMapper();
        var projectId = service.CreateProject("mcp").Value!;
        var edit = new SpriteEditTools(service, mapper);

        var result = edit.SetPixel(projectId, 0, 1, 1, 9);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void SpriteEditTool_DrawLine_ReturnsSuccess()
    {
        var service = new InMemoryProjectAutomationService();
        var mapper = new McpErrorMapper();
        var projectId = service.CreateProject("mcp-line").Value!;
        var edit = new SpriteEditTools(service, mapper);

        var result = edit.DrawLine(projectId, 0, 0, 0, 2, 0, 4);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void SpriteEditTool_MoveSelection_ReturnsSuccess()
    {
        var service = new InMemoryProjectAutomationService();
        var mapper = new McpErrorMapper();
        var projectId = service.CreateProject("mcp-move").Value!;
        var edit = new SpriteEditTools(service, mapper);

        var result = edit.MoveSelection(projectId, 0, 0, 0, 0, 0, 1, 1);

        Assert.True(result.IsSuccess);
    }
}
