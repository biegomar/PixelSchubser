using PixelSchubser.Domain.Entities;

namespace PixelSchubser.Domain.Tests;

public class SpriteProjectTests
{
    [Fact]
    public void NewProject_HasDefaultSprite()
    {
        var project = new SpriteProject("demo");

        Assert.Single(project.Sprites);
    }
}
