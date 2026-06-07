using PixelSchubser.Rendering.Preview;

namespace PixelSchubser.Rendering.Tests;

public class PreviewRendererSnapshotTests
{
    [Fact]
    public async Task PreviewRenderer_ReturnsDeterministicBytes()
    {
        var renderer = new PreviewRenderer();

        var first = await renderer.RenderPreviewAsync("snapshot");
        var second = await renderer.RenderPreviewAsync("snapshot");

        Assert.True(first.IsSuccess);
        Assert.True(second.IsSuccess);
        Assert.Equal(first.Value, second.Value);
    }
}
