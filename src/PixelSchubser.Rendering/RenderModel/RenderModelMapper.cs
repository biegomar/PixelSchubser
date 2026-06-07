using PixelSchubser.Domain.Entities;

namespace PixelSchubser.Rendering.RenderModel;

public sealed class RenderModelMapper
{
    public int[] MapSprite(Sprite sprite) => sprite.PenGrid.Flatten();
}
