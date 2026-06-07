using PixelSchubser.Application.Abstractions;
using PixelSchubser.Application.UseCases.Commands;

namespace PixelSchubser.Application.UseCases.Handlers;

public sealed class SetAnimationSettingsCommandHandler : ICommandHandler<SetAnimationSettingsCommand, bool>
{
    public Task<bool> HandleAsync(SetAnimationSettingsCommand command, CancellationToken cancellationToken = default)
    {
        var animation = command.Project.Animations[command.AnimationIndex];
        animation.Update(command.StartSpriteIndex, command.EndSpriteIndex, command.Fps, command.Mode);
        return Task.FromResult(true);
    }
}
