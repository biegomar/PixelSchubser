using PixelSchubser.Domain.Entities;

namespace PixelSchubser.Domain.Tests;

public class AnimationInvariantTests
{
    [Fact]
    public void Ctor_Throws_WhenRangeInvalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Animation("a", 5, 4, 12, AnimationMode.Loop));
    }

    [Fact]
    public void Ctor_Throws_WhenFpsNotPositive()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Animation("a", 0, 1, 0, AnimationMode.Loop));
    }
}
