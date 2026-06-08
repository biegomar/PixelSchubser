using System.ComponentModel;
using ModelContextProtocol.Server;
using PixelSchubser.Application.Services;
using PixelSchubser.Mcp.Errors;

namespace PixelSchubser.Mcp.Tools;

[McpServerToolType]
public sealed class AnimationTools(IProjectAutomationService service, McpErrorMapper mapper)
{
    [McpServerTool, Description("Set animation timing and range parameters.")]
    public McpToolResult SetAnimation(string projectId, int animationIndex, int start, int end, int fps, string mode)
    {
        var result = service.SetAnimation(projectId, animationIndex, start, end, fps, mode);
        return mapper.ToResult(result);
    }

    [McpServerTool, Description("Return frame indices for an animation preview.")]
    public McpToolResult GetAnimationPreview(string projectId, int animationIndex)
    {
        var result = service.GetAnimationPreview(projectId, animationIndex);
        return mapper.ToResult(result);
    }
}
