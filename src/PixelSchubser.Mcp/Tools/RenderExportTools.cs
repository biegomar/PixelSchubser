using System.ComponentModel;
using ModelContextProtocol.Server;
using PixelSchubser.Application.Services;
using PixelSchubser.Mcp.Errors;

namespace PixelSchubser.Mcp.Tools;

[McpServerToolType]
public sealed class RenderExportTools(IProjectAutomationService service, McpErrorMapper mapper)
{
    [McpServerTool, Description("Generate a preview payload for a project.")]
    public McpToolResult Preview(string projectId)
    {
        var result = service.GetPreview(projectId);
        return mapper.ToResult(result);
    }

    [McpServerTool, Description("Export the project with the requested format id.")]
    public McpToolResult Export(string projectId, string format)
    {
        var result = service.ExportProject(projectId, format);
        return mapper.ToResult(result);
    }

    [McpServerTool, Description("Import a project payload and return the new project id.")]
    public McpToolResult Import(string format, string payload)
    {
        var result = service.ImportProject(format, payload);
        return mapper.ToResult(result);
    }
}
