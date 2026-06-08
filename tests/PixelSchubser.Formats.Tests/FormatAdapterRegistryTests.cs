using PixelSchubser.Application.Ports;
using PixelSchubser.Formats.Registry;

namespace PixelSchubser.Formats.Tests;

public class FormatAdapterRegistryTests
{
    [Fact]
    public void Registry_ReturnsDefaultsOrderedByPriority()
    {
        var registry = new FormatAdapterRegistry();

        var all = registry.GetAll();

        Assert.NotEmpty(all);
        Assert.True(all.First().Priority >= all.Last().Priority);
    }

    [Fact]
    public void Registry_CanResolveKnownFormat()
    {
        var registry = new FormatAdapterRegistry();

        var found = registry.TryGet("spm", out var descriptor);

        Assert.True(found);
        Assert.True(descriptor.SupportsExport);
    }
}
