using PixelSchubser.Api.Contracts.Requests;
using PixelSchubser.Api.Endpoints;
using PixelSchubser.Api.Errors;
using PixelSchubser.Application.Services;

namespace PixelSchubser.Api.Tests.Integration;

public class ApiCoreParityTests
{
    [Fact]
    public void ApiSetPixel_ProducesSameStateAsCoreService()
    {
        var serviceA = new InMemoryProjectAutomationService();
        var serviceB = new InMemoryProjectAutomationService();
        var mapper = new ProblemDetailsMapper();

        var idA = serviceA.CreateProject("p").Value!;
        var idB = serviceB.CreateProject("p").Value!;

        SpriteEditEndpoints.SetPixel(idA, new EditSpriteRequest(0, 2, 2, 7), serviceA, mapper);
        serviceB.SetPixel(idB, 0, 2, 2, 7);

        var a = serviceA.GetPreview(idA).Value!;
        var b = serviceB.GetPreview(idB).Value!;

        Assert.Equal(a, b);
    }

    [Fact]
    public void ApiDrawLine_ProducesSameStateAsCoreService()
    {
        var serviceA = new InMemoryProjectAutomationService();
        var serviceB = new InMemoryProjectAutomationService();
        var mapper = new ProblemDetailsMapper();

        var idA = serviceA.CreateProject("p").Value!;
        var idB = serviceB.CreateProject("p").Value!;

        SpriteEditEndpoints.DrawLine(idA, new DrawLineRequest(0, 0, 0, 3, 0, 7), serviceA, mapper);
        serviceB.DrawLine(idB, 0, 0, 0, 3, 0, 7);

        var a = serviceA.GetWorkspace(idA).Value!;
        var b = serviceB.GetWorkspace(idB).Value!;

        Assert.Equal(a.Sprites[0].Pixels, b.Sprites[0].Pixels);
    }

    [Fact]
    public void ApiMoveSelection_ProducesSameStateAsCoreService()
    {
        var serviceA = new InMemoryProjectAutomationService();
        var serviceB = new InMemoryProjectAutomationService();
        var mapper = new ProblemDetailsMapper();

        var idA = serviceA.CreateProject("p").Value!;
        var idB = serviceB.CreateProject("p").Value!;

        serviceA.SetPixel(idA, 0, 0, 0, 8);
        serviceB.SetPixel(idB, 0, 0, 0, 8);

        SpriteEditEndpoints.MoveSelection(idA, new SelectionTransferRequest(0, 0, 0, 0, 0, 2, 1), serviceA, mapper);
        serviceB.MoveSelection(idB, 0, 0, 0, 0, 0, 2, 1);

        var a = serviceA.GetWorkspace(idA).Value!;
        var b = serviceB.GetWorkspace(idB).Value!;

        Assert.Equal(a.Sprites[0].Pixels, b.Sprites[0].Pixels);
    }
}
