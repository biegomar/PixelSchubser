using PixelSchubser.Domain.Results;

namespace PixelSchubser.Application.Ports;

public interface ISettingsStore
{
    Task<DomainResult<string?>> GetAsync(string key, CancellationToken cancellationToken = default);

    Task<DomainResult> SetAsync(string key, string value, CancellationToken cancellationToken = default);
}
