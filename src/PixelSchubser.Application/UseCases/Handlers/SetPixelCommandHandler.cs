using PixelSchubser.Application.Abstractions;
using PixelSchubser.Application.UndoRedo;
using PixelSchubser.Application.UseCases.Commands;

namespace PixelSchubser.Application.UseCases.Handlers;

public sealed class SetPixelCommandHandler(IUndoRedoCoordinator undoRedoCoordinator)
    : ICommandHandler<SetPixelCommand, bool>
{
    public Task<bool> HandleAsync(SetPixelCommand command, CancellationToken cancellationToken = default)
    {
        var sprite = command.Project.GetSprite(command.SpriteIndex);
        undoRedoCoordinator.Push(sprite.PenGrid.Flatten());
        sprite.SetPixel(command.X, command.Y, command.PenValue);
        return Task.FromResult(true);
    }
}
