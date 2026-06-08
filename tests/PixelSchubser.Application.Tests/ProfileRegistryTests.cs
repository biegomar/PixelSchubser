using PixelSchubser.Application.Profiles;
using PixelSchubser.Domain.Entities;

namespace PixelSchubser.Application.Tests;

public class ProfileRegistryTests
{
    [Fact]
    public void Registry_ContainsDefaultC64Profile()
    {
        var registry = new PlatformProfileRegistry();

        var found = registry.TryGet("c64", out var profile);

        Assert.True(found);
        Assert.Equal("Commodore 64", profile.Name);
    }

    [Fact]
    public void Registry_CanRegisterNewProfile()
    {
        var registry = new PlatformProfileRegistry();
        registry.Register(new PlatformProfile("custom", "Custom", 16, 16, ["indexed"], ["#000000"]));

        var found = registry.TryGet("custom", out _);

        Assert.True(found);
    }
}
