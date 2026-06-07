using PixelSchubser.Application.Abstractions;
using PixelSchubser.Domain.Entities;

namespace PixelSchubser.Application.UseCases.Commands;

public sealed record SetPixelCommand(SpriteProject Project, int SpriteIndex, int X, int Y, int PenValue) : ICommand<bool>;
