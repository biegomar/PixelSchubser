using PixelSchubser.Application.Ports;
using PixelSchubser.Domain.Entities;
using PixelSchubser.Domain.Errors;
using PixelSchubser.Domain.Results;

namespace PixelSchubser.Application.UseCases.Handlers;

public sealed class CreateProjectHandler
{
    public DomainResult<SpriteProject> Handle(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return DomainResult<SpriteProject>.Failure(DomainError.Validation("project.name.empty", "Project name is required."));
        }

        return DomainResult<SpriteProject>.Success(new SpriteProject(name));
    }
}

public sealed class LoadProjectHandler(IProjectRepository repository)
{
    public Task<DomainResult<string>> HandleAsync(string projectPath, CancellationToken cancellationToken = default) =>
        repository.LoadAsync(projectPath, cancellationToken);
}

public sealed class SaveProjectHandler(IProjectRepository repository)
{
    public Task<DomainResult> HandleAsync(string projectPath, string projectPayload, CancellationToken cancellationToken = default) =>
        repository.SaveAsync(projectPath, projectPayload, cancellationToken);
}
