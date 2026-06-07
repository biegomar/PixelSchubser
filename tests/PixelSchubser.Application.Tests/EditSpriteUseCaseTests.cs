using PixelSchubser.Application.UndoRedo;
using PixelSchubser.Application.UseCases.Commands;
using PixelSchubser.Application.UseCases.Handlers;
using PixelSchubser.Domain.Entities;
using PixelSchubser.Domain.ValueObjects;

namespace PixelSchubser.Application.Tests;

public class EditSpriteUseCaseTests
{
    [Fact]
    public async Task SetPixel_UpdatesPenGrid()
    {
        var project = new SpriteProject("demo");
        var handler = new SetPixelCommandHandler(new UndoRedoCoordinator());

        await handler.HandleAsync(new SetPixelCommand(project, 0, 2, 2, 7));

        Assert.Equal(7, project.GetSprite(0).PenGrid.Get(2, 2));
    }

    [Fact]
    public async Task Transform_FillSelection_UsesSelectionBounds()
    {
        var project = new SpriteProject("demo");
        project.SetSelection(new SelectionBounds(0, 0, 1, 1));
        var handler = new TransformSpriteCommandHandler(new UndoRedoCoordinator());

        await handler.HandleAsync(new TransformSpriteCommand(project, 0, TransformOperation.FillSelection, 4));

        Assert.Equal(4, project.GetSprite(0).PenGrid.Get(0, 0));
        Assert.Equal(0, project.GetSprite(0).PenGrid.Get(2, 2));
    }
}
