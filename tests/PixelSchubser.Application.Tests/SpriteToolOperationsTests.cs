using PixelSchubser.Application.Services;

namespace PixelSchubser.Application.Tests;

public sealed class SpriteToolOperationsTests
{
    [Fact]
    public void DrawLine_SetsPixelsAcrossTheLine()
    {
        var service = new InMemoryProjectAutomationService();
        var projectId = service.CreateProject("line").Value!;

        var result = service.DrawLine(projectId, 0, 0, 0, 3, 3, 5);
        var workspace = service.GetWorkspace(projectId).Value!;

        Assert.True(result.IsSuccess);
        Assert.Equal(5, workspace.Sprites[0].Pixels[0]);
        Assert.Equal(5, workspace.Sprites[0].Pixels[(1 * workspace.SpriteWidth) + 1]);
        Assert.Equal(5, workspace.Sprites[0].Pixels[(2 * workspace.SpriteWidth) + 2]);
        Assert.Equal(5, workspace.Sprites[0].Pixels[(3 * workspace.SpriteWidth) + 3]);
    }

    [Fact]
    public void FloodFill_ReplacesConnectedRegionOnly()
    {
        var service = new InMemoryProjectAutomationService();
        var projectId = service.CreateProject("fill").Value!;

        _ = service.DrawLine(projectId, 0, 1, 0, 1, 20, 2);
        var result = service.FloodFill(projectId, 0, 0, 0, 4);
        var workspace = service.GetWorkspace(projectId).Value!;

        Assert.True(result.IsSuccess);
        Assert.Equal(4, workspace.Sprites[0].Pixels[0]);
        Assert.Equal(2, workspace.Sprites[0].Pixels[1]);
        Assert.Equal(0, workspace.Sprites[0].Pixels[(4 * workspace.SpriteWidth) + 4]);
    }

    [Fact]
    public void FlipHorizontal_MirrorsPixelsAcrossSprite()
    {
        var service = new InMemoryProjectAutomationService();
        var projectId = service.CreateProject("flip").Value!;

        _ = service.SetPixel(projectId, 0, 0, 0, 6);
        var result = service.FlipHorizontal(projectId, 0);
        var workspace = service.GetWorkspace(projectId).Value!;

        Assert.True(result.IsSuccess);
        Assert.Equal(6, workspace.Sprites[0].Pixels[workspace.SpriteWidth - 1]);
    }

    [Fact]
    public void ShiftLeft_MovesPixelsAndClearsTrailingEdge()
    {
        var service = new InMemoryProjectAutomationService();
        var projectId = service.CreateProject("shift").Value!;

        _ = service.SetPixel(projectId, 0, 2, 0, 8);
        var result = service.ShiftLeft(projectId, 0);
        var workspace = service.GetWorkspace(projectId).Value!;

        Assert.True(result.IsSuccess);
        Assert.Equal(8, workspace.Sprites[0].Pixels[1]);
        Assert.Equal(0, workspace.Sprites[0].Pixels[workspace.SpriteWidth - 1]);
    }

    [Fact]
    public void Invert_MapsPenValuesAgainstPaletteMaximum()
    {
        var service = new InMemoryProjectAutomationService();
        var projectId = service.CreateProject("invert").Value!;

        _ = service.SetPixel(projectId, 0, 0, 0, 1);
        var result = service.Invert(projectId, 0);
        var workspace = service.GetWorkspace(projectId).Value!;

        Assert.True(result.IsSuccess);
        Assert.Equal(14, workspace.Sprites[0].Pixels[0]);
        Assert.Equal(15, workspace.Sprites[0].Pixels[1]);
    }

    [Fact]
    public void MoveSelection_MovesAreaToTargetCoordinates()
    {
        var service = new InMemoryProjectAutomationService();
        var projectId = service.CreateProject("move").Value!;

        _ = service.SetPixel(projectId, 0, 0, 0, 9);
        var result = service.MoveSelection(projectId, 0, 0, 0, 0, 0, 2, 1);
        var workspace = service.GetWorkspace(projectId).Value!;

        Assert.True(result.IsSuccess);
        Assert.Equal(0, workspace.Sprites[0].Pixels[0]);
        Assert.Equal(9, workspace.Sprites[0].Pixels[(1 * workspace.SpriteWidth) + 2]);
    }
}
