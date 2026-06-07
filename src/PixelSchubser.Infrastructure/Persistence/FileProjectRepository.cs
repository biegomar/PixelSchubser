using PixelSchubser.Application.Ports;
using PixelSchubser.Domain.Errors;
using PixelSchubser.Domain.Results;

namespace PixelSchubser.Infrastructure.Persistence;

public sealed class FileProjectRepository : IProjectRepository
{
    public async Task<DomainResult<string>> LoadAsync(string projectPath, CancellationToken cancellationToken = default)
    {
        if (!File.Exists(projectPath))
        {
            return DomainResult<string>.Failure(DomainError.NotFound("project.file.missing", "Project file does not exist."));
        }

        var payload = await File.ReadAllTextAsync(projectPath, cancellationToken);
        return DomainResult<string>.Success(payload);
    }

    public async Task<DomainResult> SaveAsync(string projectPath, string projectPayload, CancellationToken cancellationToken = default)
    {
        var directory = Path.GetDirectoryName(projectPath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.WriteAllTextAsync(projectPath, projectPayload, cancellationToken);
        return DomainResult.Success();
    }
}
