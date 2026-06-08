using PixelSchubser.Api.Contracts.Requests;
using PixelSchubser.Api.Endpoints;
using PixelSchubser.Api.Errors;
using PixelSchubser.Application.Services;
using PixelSchubser.Mcp.Errors;
using PixelSchubser.Mcp.Tools;

namespace PixelSchubser.Application.Tests;

public class AnimationParityIntegrationTests
{
    [Fact]
    public void CoreApiMcp_ReturnEquivalentAnimationPreviewFrames()
    {
        var coreService = new InMemoryProjectAutomationService();
        var apiService = new InMemoryProjectAutomationService();
        var mcpService = new InMemoryProjectAutomationService();

        var coreId = coreService.CreateProject("core").Value!;
        var apiId = apiService.CreateProject("api").Value!;
        var mcpId = mcpService.CreateProject("mcp").Value!;

        coreService.SetAnimation(coreId, 0, 1, 3, 10, "Loop");
        AnimationEndpoints.SetAnimationSettings(apiId, new SetAnimationRequest(0, 1, 3, 10, "Loop"), apiService, new ProblemDetailsMapper());
        var mcpTools = new AnimationTools(mcpService, new McpErrorMapper());
        mcpTools.SetAnimation(mcpId, 0, 1, 3, 10, "Loop");

        var coreFrames = coreService.GetAnimationPreview(coreId, 0).Value!;
        var apiFrames = apiService.GetAnimationPreview(apiId, 0).Value!;
        var mcpFrames = (IReadOnlyList<int>)mcpTools.GetAnimationPreview(mcpId, 0).Payload!;

        Assert.Equal(coreFrames, apiFrames);
        Assert.Equal(coreFrames, mcpFrames);
    }
}
