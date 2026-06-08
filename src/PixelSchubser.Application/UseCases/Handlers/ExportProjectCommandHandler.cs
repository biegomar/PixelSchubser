using PixelSchubser.Application.Ports;
using PixelSchubser.Domain.Errors;
using PixelSchubser.Domain.Results;

namespace PixelSchubser.Application.UseCases.Handlers;

public sealed class ExportProjectCommandHandler(IFormatAdapterRegistry registry)
{
    public DomainResult<string> Handle(string formatId, string payload)
    {
        if (!registry.TryGet(formatId, out var descriptor) || !descriptor.SupportsExport)
        {
            return DomainResult<string>.Failure(DomainError.Validation("format.unsupported", $"Unsupported export format '{formatId}'."));
        }

        return DomainResult<string>.Success($"{descriptor.FormatId}:{payload}");
    }
}
