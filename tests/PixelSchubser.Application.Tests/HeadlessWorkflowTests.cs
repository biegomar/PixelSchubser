using PixelSchubser.Application.UndoRedo;
using PixelSchubser.Application.UseCases.Commands;
using PixelSchubser.Application.UseCases.Handlers;
using PixelSchubser.Domain.Entities;
using PixelSchubser.Rendering.Preview;

namespace PixelSchubser.Application.Tests;

public class HeadlessWorkflowTests
{
    [Fact]
    public async Task CreateEditRender_Workflow_WorksWithoutUi()
    {
        var createHandler = new CreateProjectHandler();
        var createResult = createHandler.Handle("headless");
        Assert.True(createResult.IsSuccess);

        var project = createResult.Value!;
        var setPixel = new SetPixelCommandHandler(new UndoRedoCoordinator());
        await setPixel.HandleAsync(new SetPixelCommand(project, 0, 1, 1, 5));

        var renderer = new PreviewRenderer();
        var previewResult = await renderer.RenderPreviewAsync("payload");

        Assert.True(previewResult.IsSuccess);
        Assert.Equal(5, project.GetSprite(0).PenGrid.Get(1, 1));
    }
}
