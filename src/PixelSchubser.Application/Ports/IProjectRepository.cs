using PixelSchubser.Domain.Results;

namespace PixelSchubser.Application.Ports;

public interface IProjectRepository
{
    Task<DomainResult<string>> LoadAsync(string projectPath, CancellationToken cancellationToken = default);

    Task<DomainResult> SaveAsync(string projectPath, string projectPayload, CancellationToken cancellationToken = default);
}
