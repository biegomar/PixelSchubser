using System.Collections.Concurrent;
using PixelSchubser.Application.UseCases.Commands;
using PixelSchubser.Application.UseCases.Handlers;
using PixelSchubser.Application.UseCases.Queries;
using PixelSchubser.Domain.Entities;
using PixelSchubser.Domain.Errors;
using PixelSchubser.Domain.Results;
using PixelSchubser.Domain.ValueObjects;

namespace PixelSchubser.Application.Services;

public sealed class InMemoryProjectAutomationService : IProjectAutomationService
{
    private static readonly HashSet<string> SupportedFormats = new(StringComparer.OrdinalIgnoreCase)
    {
        "spm", "spd", "spr", "png", "spritesheet-png", "asm", "basic"
    };

    private readonly ConcurrentDictionary<string, SpriteProject> projects = new(StringComparer.OrdinalIgnoreCase);
    private readonly CreateProjectHandler createProjectHandler = new();
    private readonly SetPixelCommandHandler setPixelHandler = new(new UndoRedo.UndoRedoCoordinator());
    private readonly TransformSpriteCommandHandler transformHandler = new(new UndoRedo.UndoRedoCoordinator());
    private readonly SetAnimationSettingsCommandHandler setAnimationHandler = new();
    private readonly GetAnimationPreviewQueryHandler getAnimationPreviewHandler = new();

    public DomainResult<string> CreateProject(string name)
    {
        var createResult = createProjectHandler.Handle(name);
        if (createResult.IsFailure || createResult.Value is null)
        {
            return DomainResult<string>.Failure(createResult.Error!);
        }

        var id = Guid.NewGuid().ToString("N");
        projects[id] = createResult.Value;
        return DomainResult<string>.Success(id);
    }

    public DomainResult<ProjectSnapshot> GetProject(string projectId)
    {
        if (!projects.TryGetValue(projectId, out var project))
        {
            return DomainResult<ProjectSnapshot>.Failure(DomainError.NotFound("project.notfound", "Project not found."));
        }

        return DomainResult<ProjectSnapshot>.Success(ToSnapshot(projectId, project));
    }

    public DomainResult<WorkspaceSnapshot> GetWorkspace(string projectId)
    {
        if (!projects.TryGetValue(projectId, out var project))
        {
            return DomainResult<WorkspaceSnapshot>.Failure(DomainError.NotFound("project.notfound", "Project not found."));
        }

        return DomainResult<WorkspaceSnapshot>.Success(ToWorkspaceSnapshot(projectId, project));
    }

    public DomainResult<ProjectSnapshot> SetPixel(string projectId, int spriteIndex, int x, int y, int penValue)
    {
        return ExecuteSpriteMutation(
            projectId,
            spriteIndex,
            project => setPixelHandler.HandleAsync(new SetPixelCommand(project, spriteIndex, x, y, penValue)).GetAwaiter().GetResult());
    }

    public DomainResult<ProjectSnapshot> DrawLine(string projectId, int spriteIndex, int x1, int y1, int x2, int y2, int penValue)
    {
        return ExecuteSpriteMutation(
            projectId,
            spriteIndex,
            project => project.GetSprite(spriteIndex).DrawLine(x1, y1, x2, y2, penValue));
    }

    public DomainResult<ProjectSnapshot> FloodFill(string projectId, int spriteIndex, int x, int y, int penValue)
    {
        return ExecuteSpriteMutation(
            projectId,
            spriteIndex,
            project => project.GetSprite(spriteIndex).FloodFill(x, y, penValue));
    }

    public DomainResult<ProjectSnapshot> FlipHorizontal(string projectId, int spriteIndex)
    {
        return ExecuteSpriteMutation(projectId, spriteIndex, project => project.GetSprite(spriteIndex).FlipHorizontal());
    }

    public DomainResult<ProjectSnapshot> FlipVertical(string projectId, int spriteIndex)
    {
        return ExecuteSpriteMutation(projectId, spriteIndex, project => project.GetSprite(spriteIndex).FlipVertical());
    }

    public DomainResult<ProjectSnapshot> ShiftLeft(string projectId, int spriteIndex)
    {
        return ExecuteSpriteMutation(projectId, spriteIndex, project => project.GetSprite(spriteIndex).ShiftLeft());
    }

    public DomainResult<ProjectSnapshot> ShiftRight(string projectId, int spriteIndex)
    {
        return ExecuteSpriteMutation(projectId, spriteIndex, project => project.GetSprite(spriteIndex).ShiftRight());
    }

    public DomainResult<ProjectSnapshot> ShiftUp(string projectId, int spriteIndex)
    {
        return ExecuteSpriteMutation(projectId, spriteIndex, project => project.GetSprite(spriteIndex).ShiftUp());
    }

    public DomainResult<ProjectSnapshot> ShiftDown(string projectId, int spriteIndex)
    {
        return ExecuteSpriteMutation(projectId, spriteIndex, project => project.GetSprite(spriteIndex).ShiftDown());
    }

    public DomainResult<ProjectSnapshot> Invert(string projectId, int spriteIndex)
    {
        return ExecuteSpriteMutation(
            projectId,
            spriteIndex,
            project =>
            {
                var maxPen = project.PlatformProfile.Palette.Count - 1;
                project.GetSprite(spriteIndex).Invert(maxPen);
            });
    }

    public DomainResult<ProjectSnapshot> CopySelection(string projectId, int spriteIndex, int x1, int y1, int x2, int y2, int targetX, int targetY)
    {
        return ExecuteSpriteMutation(
            projectId,
            spriteIndex,
            project =>
            {
                var selection = new SelectionBounds(x1, y1, x2, y2);
                project.GetSprite(spriteIndex).CopySelection(selection, targetX, targetY);
            });
    }

    public DomainResult<ProjectSnapshot> MoveSelection(string projectId, int spriteIndex, int x1, int y1, int x2, int y2, int targetX, int targetY)
    {
        return ExecuteSpriteMutation(
            projectId,
            spriteIndex,
            project =>
            {
                var selection = new SelectionBounds(x1, y1, x2, y2);
                project.GetSprite(spriteIndex).MoveSelection(selection, targetX, targetY);
            });
    }

    public DomainResult<ProjectSnapshot> FillSelection(string projectId, int spriteIndex, int x1, int y1, int x2, int y2, int penValue)
    {
        return ExecuteSpriteMutation(
            projectId,
            spriteIndex,
            project =>
            {
                project.SetSelection(new SelectionBounds(x1, y1, x2, y2));
                transformHandler.HandleAsync(new TransformSpriteCommand(project, spriteIndex, TransformOperation.FillSelection, penValue)).GetAwaiter().GetResult();
            });
    }

    public DomainResult<string> ExportProject(string projectId, string format)
    {
        if (!SupportedFormats.Contains(format))
        {
            return DomainResult<string>.Failure(DomainError.Validation("format.unsupported", $"Unsupported format '{format}'."));
        }

        var projectResult = GetProjectEntity(projectId);
        if (projectResult.IsFailure)
        {
            return DomainResult<string>.Failure(projectResult.Error!);
        }

        var payload = $"{format}:{projectResult.Value!.Name}:{projectResult.Value.Sprites.Count}";
        return DomainResult<string>.Success(payload);
    }

    public DomainResult<string> ImportProject(string format, string payload)
    {
        if (!SupportedFormats.Contains(format))
        {
            return DomainResult<string>.Failure(DomainError.Validation("format.unsupported", $"Unsupported format '{format}'."));
        }

        var name = string.IsNullOrWhiteSpace(payload) ? "imported" : payload.Trim();
        return CreateProject($"import:{name}");
    }

    public DomainResult<byte[]> GetPreview(string projectId)
    {
        var projectResult = GetProjectEntity(projectId);
        if (projectResult.IsFailure)
        {
            return DomainResult<byte[]>.Failure(projectResult.Error!);
        }

        var payload = $"{projectResult.Value!.Name}:{projectResult.Value.Sprites.Count}";
        var bytes = System.Text.Encoding.UTF8.GetBytes(payload);
        return DomainResult<byte[]>.Success(bytes);
    }

    public DomainResult SetAnimation(string projectId, int animationIndex, int start, int end, int fps, string mode)
    {
        var projectResult = GetProjectEntity(projectId);
        if (projectResult.IsFailure || projectResult.Value is null)
        {
            return DomainResult.Failure(projectResult.Error!);
        }

        if (!Enum.TryParse<AnimationMode>(mode, true, out var parsedMode))
        {
            return DomainResult.Failure(DomainError.Validation("animation.mode", "Animation mode is invalid."));
        }

        if (animationIndex < 0)
        {
            return DomainResult.Failure(DomainError.Validation("animation.index", "Animation index must be >= 0."));
        }

        while (projectResult.Value.Animations.Count <= animationIndex)
        {
            projectResult.Value.AddAnimation(new Animation($"anim-{projectResult.Value.Animations.Count}", 0, 0, 8, AnimationMode.Loop));
        }

        setAnimationHandler
            .HandleAsync(new SetAnimationSettingsCommand(projectResult.Value, animationIndex, start, end, fps, parsedMode))
            .GetAwaiter()
            .GetResult();

        return DomainResult.Success();
    }

    public DomainResult<IReadOnlyList<int>> GetAnimationPreview(string projectId, int animationIndex)
    {
        var projectResult = GetProjectEntity(projectId);
        if (projectResult.IsFailure || projectResult.Value is null)
        {
            return DomainResult<IReadOnlyList<int>>.Failure(projectResult.Error!);
        }

        if (animationIndex < 0 || animationIndex >= projectResult.Value.Animations.Count)
        {
            return DomainResult<IReadOnlyList<int>>.Failure(DomainError.Validation("animation.index", "Animation index is out of range."));
        }

        var preview = getAnimationPreviewHandler
            .HandleAsync(new GetAnimationPreviewQuery(projectResult.Value, animationIndex))
            .GetAwaiter()
            .GetResult();

        return DomainResult<IReadOnlyList<int>>.Success(preview);
    }

    private DomainResult<SpriteProject> GetProjectEntity(string projectId)
    {
        if (!projects.TryGetValue(projectId, out var project))
        {
            return DomainResult<SpriteProject>.Failure(DomainError.NotFound("project.notfound", "Project not found."));
        }

        return DomainResult<SpriteProject>.Success(project);
    }

    private static bool TryInSpriteRange(SpriteProject project, int spriteIndex) =>
        spriteIndex >= 0 && spriteIndex < project.Sprites.Count;

    private DomainResult<ProjectSnapshot> ExecuteSpriteMutation(string projectId, int spriteIndex, Action<SpriteProject> mutation)
    {
        var projectResult = GetProjectEntity(projectId);
        if (projectResult.IsFailure || projectResult.Value is null)
        {
            return DomainResult<ProjectSnapshot>.Failure(projectResult.Error!);
        }

        if (!TryInSpriteRange(projectResult.Value, spriteIndex))
        {
            return DomainResult<ProjectSnapshot>.Failure(DomainError.Validation("sprite.index", "Sprite index is out of range."));
        }

        try
        {
            mutation(projectResult.Value);
            return DomainResult<ProjectSnapshot>.Success(ToSnapshot(projectId, projectResult.Value));
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return DomainResult<ProjectSnapshot>.Failure(DomainError.Validation("sprite.bounds", ex.Message));
        }
    }

    private static ProjectSnapshot ToSnapshot(string projectId, SpriteProject project)
    {
        var firstSprite = project.GetSprite(0);
        return new ProjectSnapshot(
            projectId,
            project.Name,
            project.Sprites.Count,
            project.Animations.Count,
            firstSprite.PenGrid.Width,
            firstSprite.PenGrid.Height);
    }

    private static WorkspaceSnapshot ToWorkspaceSnapshot(string projectId, SpriteProject project)
    {
        var sprites = project.Sprites
            .Select(sprite => new WorkspaceSpriteSnapshot(
                sprite.Name,
                sprite.PenGrid.Width,
                sprite.PenGrid.Height,
                sprite.PenGrid.Flatten()))
            .ToArray();

        var animations = project.Animations
            .Select(animation => new WorkspaceAnimationSnapshot(
                animation.Name,
                animation.StartSpriteIndex,
                animation.EndSpriteIndex,
                animation.Fps,
                animation.Mode.ToString()))
            .ToArray();

        return new WorkspaceSnapshot(
            projectId,
            project.Name,
            project.PlatformProfile.ProfileId,
            project.PlatformProfile.Name,
            project.PlatformProfile.SpriteWidth,
            project.PlatformProfile.SpriteHeight,
            project.PlatformProfile.Palette,
            sprites,
            animations);
    }
}
