using PixelSchubser.Domain.Entities;

namespace PixelSchubser.Rendering.Preview;

public sealed class AnimationPreviewRenderer
{
    public IReadOnlyList<int> GetFrameIndices(Animation animation)
    {
        return Enumerable.Range(animation.StartSpriteIndex, animation.EndSpriteIndex - animation.StartSpriteIndex + 1)
            .ToArray();
    }
}
