using PixelSchubser.Application.Services;

namespace PixelSchubser.Application.Tests;

public sealed class WorkspaceSnapshotTests
{
    [Fact]
    public void GetWorkspace_ReturnsEditableSpriteStateAndProjectMetadata()
    {
        var service = new InMemoryProjectAutomationService();
        var projectId = service.CreateProject("workspace").Value!;

        _ = service.SetPixel(projectId, 0, 2, 3, 7);
        _ = service.SetAnimation(projectId, 0, 0, 0, 12, "Loop");

        var result = service.GetWorkspace(projectId);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("workspace", result.Value!.Name);
        Assert.Single(result.Value.Sprites);
        Assert.Single(result.Value.Animations);
        Assert.Equal(24, result.Value.SpriteWidth);
        Assert.Equal(21, result.Value.SpriteHeight);
        Assert.Equal(7, result.Value.Sprites[0].Pixels[3 * result.Value.SpriteWidth + 2]);
        Assert.NotEmpty(result.Value.Palette);
    }
}
