using PixelSchubser.Application.Abstractions;
using PixelSchubser.Domain.Entities;

namespace PixelSchubser.Application.UseCases.Commands;

public sealed record SetAnimationSettingsCommand(
    SpriteProject Project,
    int AnimationIndex,
    int StartSpriteIndex,
    int EndSpriteIndex,
    int Fps,
    AnimationMode Mode) : ICommand<bool>;
