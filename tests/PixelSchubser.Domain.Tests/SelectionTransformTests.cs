using PixelSchubser.Domain.Entities;
using PixelSchubser.Domain.ValueObjects;

namespace PixelSchubser.Domain.Tests;

public class SelectionTransformTests
{
    [Fact]
    public void FillSelection_OnlyTouchesSelectedCells()
    {
        var sprite = new Sprite("s", 4, 4);
        var selection = new SelectionBounds(1, 1, 2, 2);

        sprite.FillSelection(selection, 3);

        Assert.Equal(3, sprite.PenGrid.Get(1, 1));
        Assert.Equal(3, sprite.PenGrid.Get(2, 2));
        Assert.Equal(0, sprite.PenGrid.Get(0, 0));
        Assert.Equal(0, sprite.PenGrid.Get(3, 3));
    }
}
