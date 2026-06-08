using Microsoft.AspNetCore.Http.HttpResults;
using PixelSchubser.Api.Contracts.Requests;
using PixelSchubser.Api.Endpoints;
using PixelSchubser.Api.Errors;
using PixelSchubser.Application.Services;

namespace PixelSchubser.Api.Tests.Contracts;

public class SpriteEditEndpointsContractTests
{
    [Fact]
    public void SetPixel_ReturnsOkForValidRequest()
    {
        var service = new InMemoryProjectAutomationService();
        var mapper = new ProblemDetailsMapper();
        var projectId = service.CreateProject("edit").Value!;

        var result = SpriteEditEndpoints.SetPixel(projectId, new EditSpriteRequest(0, 0, 0, 3), service, mapper);

        Assert.IsType<Ok<PixelSchubser.Api.Contracts.Responses.ProjectResponse>>(result);
    }

    [Fact]
    public void FillSelection_RejectsUnknownProject()
    {
        var service = new InMemoryProjectAutomationService();
        var mapper = new ProblemDetailsMapper();

        var result = SpriteEditEndpoints.FillSelection("missing", new FillSelectionRequest(0, 0, 0, 1, 1, 3), service, mapper);

        Assert.IsNotType<Ok<PixelSchubser.Api.Contracts.Responses.ProjectResponse>>(result);
    }

    [Fact]
    public void DrawLine_ReturnsOkForValidRequest()
    {
        var service = new InMemoryProjectAutomationService();
        var mapper = new ProblemDetailsMapper();
        var projectId = service.CreateProject("line").Value!;

        var result = SpriteEditEndpoints.DrawLine(projectId, new DrawLineRequest(0, 0, 0, 3, 0, 7), service, mapper);

        Assert.IsType<Ok<PixelSchubser.Api.Contracts.Responses.ProjectResponse>>(result);
    }

    [Fact]
    public void ShiftLeft_ReturnsOkForValidRequest()
    {
        var service = new InMemoryProjectAutomationService();
        var mapper = new ProblemDetailsMapper();
        var projectId = service.CreateProject("shift").Value!;

        var result = SpriteEditEndpoints.ShiftLeft(projectId, new SpriteTransformRequest(0), service, mapper);

        Assert.IsType<Ok<PixelSchubser.Api.Contracts.Responses.ProjectResponse>>(result);
    }

    [Fact]
    public void MoveSelection_ReturnsOkForValidRequest()
    {
        var service = new InMemoryProjectAutomationService();
        var mapper = new ProblemDetailsMapper();
        var projectId = service.CreateProject("move").Value!;

        var result = SpriteEditEndpoints.MoveSelection(projectId, new SelectionTransferRequest(0, 0, 0, 0, 0, 1, 1), service, mapper);

        Assert.IsType<Ok<PixelSchubser.Api.Contracts.Responses.ProjectResponse>>(result);
    }
}
