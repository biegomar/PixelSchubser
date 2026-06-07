using PixelSchubser.Application.Abstractions;
using PixelSchubser.Domain.Entities;

namespace PixelSchubser.Application.UseCases.Queries;

public sealed record GetAnimationPreviewQuery(SpriteProject Project, int AnimationIndex) : IQuery<IReadOnlyList<int>>;
