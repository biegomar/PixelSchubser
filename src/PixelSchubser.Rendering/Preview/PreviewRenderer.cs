using PixelSchubser.Application.Ports;
using PixelSchubser.Domain.Results;

namespace PixelSchubser.Rendering.Preview;

public sealed class PreviewRenderer : IRenderPreviewService
{
    public Task<DomainResult<byte[]>> RenderPreviewAsync(string projectPayload, CancellationToken cancellationToken = default)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(projectPayload);
        return Task.FromResult(DomainResult<byte[]>.Success(bytes));
    }
}
