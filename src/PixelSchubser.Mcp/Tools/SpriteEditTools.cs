using System.ComponentModel;
using ModelContextProtocol.Server;
using PixelSchubser.Application.Services;
using PixelSchubser.Mcp.Errors;

namespace PixelSchubser.Mcp.Tools;

[McpServerToolType]
public sealed class SpriteEditTools(IProjectAutomationService service, McpErrorMapper mapper)
{
    [McpServerTool, Description("Set a pixel value in a sprite.")]
    public McpToolResult SetPixel(string projectId, int spriteIndex, int x, int y, int penValue)
    {
        var result = service.SetPixel(projectId, spriteIndex, x, y, penValue);
        return mapper.ToResult(result);
    }

    [McpServerTool, Description("Fill the selected rectangle with a pen value.")]
    public McpToolResult FillSelection(string projectId, int spriteIndex, int x1, int y1, int x2, int y2, int penValue)
    {
        var result = service.FillSelection(projectId, spriteIndex, x1, y1, x2, y2, penValue);
        return mapper.ToResult(result);
    }

    [McpServerTool, Description("Draw a line using the selected pen value.")]
    public McpToolResult DrawLine(string projectId, int spriteIndex, int x1, int y1, int x2, int y2, int penValue)
    {
        var result = service.DrawLine(projectId, spriteIndex, x1, y1, x2, y2, penValue);
        return mapper.ToResult(result);
    }

    [McpServerTool, Description("Flood fill from one coordinate with a pen value.")]
    public McpToolResult FloodFill(string projectId, int spriteIndex, int x, int y, int penValue)
    {
        var result = service.FloodFill(projectId, spriteIndex, x, y, penValue);
        return mapper.ToResult(result);
    }

    [McpServerTool, Description("Flip a sprite horizontally.")]
    public McpToolResult FlipHorizontal(string projectId, int spriteIndex)
    {
        var result = service.FlipHorizontal(projectId, spriteIndex);
        return mapper.ToResult(result);
    }

    [McpServerTool, Description("Flip a sprite vertically.")]
    public McpToolResult FlipVertical(string projectId, int spriteIndex)
    {
        var result = service.FlipVertical(projectId, spriteIndex);
        return mapper.ToResult(result);
    }

    [McpServerTool, Description("Shift a sprite one pixel to the left.")]
    public McpToolResult ShiftLeft(string projectId, int spriteIndex)
    {
        var result = service.ShiftLeft(projectId, spriteIndex);
        return mapper.ToResult(result);
    }

    [McpServerTool, Description("Shift a sprite one pixel to the right.")]
    public McpToolResult ShiftRight(string projectId, int spriteIndex)
    {
        var result = service.ShiftRight(projectId, spriteIndex);
        return mapper.ToResult(result);
    }

    [McpServerTool, Description("Shift a sprite one pixel up.")]
    public McpToolResult ShiftUp(string projectId, int spriteIndex)
    {
        var result = service.ShiftUp(projectId, spriteIndex);
        return mapper.ToResult(result);
    }

    [McpServerTool, Description("Shift a sprite one pixel down.")]
    public McpToolResult ShiftDown(string projectId, int spriteIndex)
    {
        var result = service.ShiftDown(projectId, spriteIndex);
        return mapper.ToResult(result);
    }

    [McpServerTool, Description("Invert all pen values within the active sprite.")]
    public McpToolResult Invert(string projectId, int spriteIndex)
    {
        var result = service.Invert(projectId, spriteIndex);
        return mapper.ToResult(result);
    }

    [McpServerTool, Description("Copy a rectangular selection to a target coordinate.")]
    public McpToolResult CopySelection(string projectId, int spriteIndex, int x1, int y1, int x2, int y2, int targetX, int targetY)
    {
        var result = service.CopySelection(projectId, spriteIndex, x1, y1, x2, y2, targetX, targetY);
        return mapper.ToResult(result);
    }

    [McpServerTool, Description("Move a rectangular selection to a target coordinate.")]
    public McpToolResult MoveSelection(string projectId, int spriteIndex, int x1, int y1, int x2, int y2, int targetX, int targetY)
    {
        var result = service.MoveSelection(projectId, spriteIndex, x1, y1, x2, y2, targetX, targetY);
        return mapper.ToResult(result);
    }
}
