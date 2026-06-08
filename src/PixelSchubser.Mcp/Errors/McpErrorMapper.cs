using PixelSchubser.Domain.Results;

namespace PixelSchubser.Mcp.Errors;

public sealed class McpErrorMapper
{
    public McpToolResult ToResult(DomainResult result)
    {
        return result.IsSuccess
            ? McpToolResult.Success(null)
            : McpToolResult.Failure(result.Error!.Code, result.Error.Message);
    }

    public McpToolResult ToResult<T>(DomainResult<T> result)
    {
        return result.IsSuccess
            ? McpToolResult.Success(result.Value)
            : McpToolResult.Failure(result.Error!.Code, result.Error.Message);
    }
}

public sealed record McpToolResult(bool IsSuccess, string? ErrorCode, string? ErrorMessage, object? Payload)
{
    public static McpToolResult Success(object? payload) => new(true, null, null, payload);

    public static McpToolResult Failure(string code, string message) => new(false, code, message, null);
}
