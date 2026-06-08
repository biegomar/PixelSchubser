namespace PixelSchubser.Infrastructure.Logging;

public interface IStructuredLoggerAdapter
{
    void LogOperationStarted(string operation, string correlationId, object? metadata = null);

    void LogOperationSucceeded(string operation, string correlationId, long elapsedMilliseconds, object? metadata = null);

    void LogOperationFailed(string operation, string correlationId, string errorCode, string errorMessage, object? metadata = null);
}
