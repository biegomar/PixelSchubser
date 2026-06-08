using Microsoft.Extensions.Logging;

namespace PixelSchubser.Infrastructure.Logging;

public sealed class StructuredLoggerAdapter(ILogger<StructuredLoggerAdapter> logger) : IStructuredLoggerAdapter
{
    public void LogOperationStarted(string operation, string correlationId, object? metadata = null)
    {
        logger.LogInformation(
            "operation.started operation={Operation} correlationId={CorrelationId} metadata={@Metadata}",
            operation,
            correlationId,
            metadata);
    }

    public void LogOperationSucceeded(string operation, string correlationId, long elapsedMilliseconds, object? metadata = null)
    {
        logger.LogInformation(
            "operation.succeeded operation={Operation} correlationId={CorrelationId} elapsedMs={ElapsedMs} metadata={@Metadata}",
            operation,
            correlationId,
            elapsedMilliseconds,
            metadata);
    }

    public void LogOperationFailed(string operation, string correlationId, string errorCode, string errorMessage, object? metadata = null)
    {
        logger.LogWarning(
            "operation.failed operation={Operation} correlationId={CorrelationId} errorCode={ErrorCode} errorMessage={ErrorMessage} metadata={@Metadata}",
            operation,
            correlationId,
            errorCode,
            errorMessage,
            metadata);
    }
}
