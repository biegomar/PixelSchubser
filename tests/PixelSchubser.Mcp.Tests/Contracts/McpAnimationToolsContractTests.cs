using PixelSchubser.Application.Services;
using PixelSchubser.Mcp.Errors;
using PixelSchubser.Mcp.Tools;

namespace PixelSchubser.Mcp.Tests.Contracts;

public class McpAnimationToolsContractTests
{
    [Fact]
    public void AnimationTools_ReturnPreviewFrames()
    {
        var service = new InMemoryProjectAutomationService();
        var projectId = service.CreateProject("anim").Value!;
        var tools = new AnimationTools(service, new McpErrorMapper());

        tools.SetAnimation(projectId, 0, 0, 2, 8, "Loop");
        var preview = tools.GetAnimationPreview(projectId, 0);

        Assert.True(preview.IsSuccess);
        Assert.NotNull(preview.Payload);
    }
}
