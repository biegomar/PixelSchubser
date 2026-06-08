using PixelSchubser.Application.Services;

namespace PixelSchubser.Application.Tests;

public sealed class HeadlessEndToEndRegressionTests
{
    [Fact]
    public void FullHeadlessScenario_RemainsStableAcrossCoreFlows()
    {
        var service = new InMemoryProjectAutomationService();

        var create = service.CreateProject("e2e-regression");
        Assert.True(create.IsSuccess);
        var projectId = create.Value!;

        var editResult = service.SetPixel(projectId, 0, 3, 4, 9);
        Assert.True(editResult.IsSuccess);

        var fillResult = service.FillSelection(projectId, 0, 0, 0, 2, 2, 5);
        Assert.True(fillResult.IsSuccess);

        var animationResult = service.SetAnimation(projectId, 0, 0, 0, 8, "Loop");
        Assert.True(animationResult.IsSuccess);

        var animationPreview = service.GetAnimationPreview(projectId, 0);
        Assert.True(animationPreview.IsSuccess);
        Assert.NotEmpty(animationPreview.Value!);

        var preview = service.GetPreview(projectId);
        Assert.True(preview.IsSuccess);
        Assert.NotEmpty(preview.Value!);

        var export = service.ExportProject(projectId, "spm");
        Assert.True(export.IsSuccess);
        Assert.StartsWith("spm:", export.Value!, StringComparison.OrdinalIgnoreCase);

        var import = service.ImportProject("spm", "reimported");
        Assert.True(import.IsSuccess);
        var importedProject = service.GetProject(import.Value!);
        Assert.True(importedProject.IsSuccess);
        Assert.StartsWith("import:", importedProject.Value!.Name, StringComparison.OrdinalIgnoreCase);
    }
}
