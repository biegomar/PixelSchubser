using PixelSchubser.Application.Abstractions;
using PixelSchubser.Application.UseCases.Queries;

namespace PixelSchubser.Application.UseCases.Handlers;

public sealed class GetAnimationPreviewQueryHandler : IQueryHandler<GetAnimationPreviewQuery, IReadOnlyList<int>>
{
    public Task<IReadOnlyList<int>> HandleAsync(GetAnimationPreviewQuery query, CancellationToken cancellationToken = default)
    {
        var animation = query.Project.Animations[query.AnimationIndex];
        var frames = Enumerable.Range(animation.StartSpriteIndex, animation.EndSpriteIndex - animation.StartSpriteIndex + 1)
            .ToArray();

        return Task.FromResult<IReadOnlyList<int>>(frames);
    }
}
