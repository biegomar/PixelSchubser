using PixelSchubser.Application.Abstractions;
using PixelSchubser.Domain.Entities;

namespace PixelSchubser.Application.UseCases.Commands;

public enum TransformOperation
{
    FillSelection
}

public sealed record TransformSpriteCommand(
    SpriteProject Project,
    int SpriteIndex,
    TransformOperation Operation,
    int PenValue) : ICommand<bool>;
