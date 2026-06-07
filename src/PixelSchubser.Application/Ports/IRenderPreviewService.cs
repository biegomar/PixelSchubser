using PixelSchubser.Domain.Results;

namespace PixelSchubser.Application.Ports;

public interface IRenderPreviewService
{
    Task<DomainResult<byte[]>> RenderPreviewAsync(string projectPayload, CancellationToken cancellationToken = default);
}
