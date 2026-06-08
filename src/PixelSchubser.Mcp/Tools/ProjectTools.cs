using System.ComponentModel;
using ModelContextProtocol.Server;
using PixelSchubser.Application.Services;
using PixelSchubser.Mcp.Errors;

namespace PixelSchubser.Mcp.Tools;

[McpServerToolType]
public sealed class ProjectTools(IProjectAutomationService service, McpErrorMapper mapper)
{
    [McpServerTool, Description("Create a new sprite project and return its identifier.")]
    public McpToolResult CreateProject(string name)
    {
        var result = service.CreateProject(name);
        return mapper.ToResult(result);
    }

    [McpServerTool, Description("Get the current project snapshot by project id.")]
    public McpToolResult GetProject(string projectId)
    {
        var result = service.GetProject(projectId);
        return mapper.ToResult(result);
    }
}
