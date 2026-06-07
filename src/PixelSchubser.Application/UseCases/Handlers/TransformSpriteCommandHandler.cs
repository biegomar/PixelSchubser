using PixelSchubser.Application.Abstractions;
using PixelSchubser.Application.UndoRedo;
using PixelSchubser.Application.UseCases.Commands;

namespace PixelSchubser.Application.UseCases.Handlers;

public sealed class TransformSpriteCommandHandler(IUndoRedoCoordinator undoRedoCoordinator)
    : ICommandHandler<TransformSpriteCommand, bool>
{
    public Task<bool> HandleAsync(TransformSpriteCommand command, CancellationToken cancellationToken = default)
    {
        var sprite = command.Project.GetSprite(command.SpriteIndex);
        undoRedoCoordinator.Push(sprite.PenGrid.Flatten());

        if (command.Operation == TransformOperation.FillSelection)
        {
            sprite.FillSelection(command.Project.CurrentSelection, command.PenValue);
        }

        return Task.FromResult(true);
    }
}
