using Microsoft.AspNetCore.Http.HttpResults;
using PixelSchubser.Api.Contracts.Requests;
using PixelSchubser.Api.Contracts.Responses;
using PixelSchubser.Api.Endpoints;
using PixelSchubser.Api.Errors;
using PixelSchubser.Application.Services;

namespace PixelSchubser.Api.Tests.Contracts;

public class AnimationEndpointsContractTests
{
    [Fact]
    public void SetAnimationSettings_ReturnsNoContentForValidRequest()
    {
        var service = new InMemoryProjectAutomationService();
        var mapper = new ProblemDetailsMapper();
        var projectId = service.CreateProject("anim").Value!;

        var result = AnimationEndpoints.SetAnimationSettings(projectId, new SetAnimationRequest(0, 0, 2, 8, "Loop"), service, mapper);

        Assert.IsType<NoContent>(result);
    }

    [Fact]
    public void GetAnimationPreview_ReturnsFrames()
    {
        var service = new InMemoryProjectAutomationService();
        var mapper = new ProblemDetailsMapper();
        var projectId = service.CreateProject("anim").Value!;
        service.SetAnimation(projectId, 0, 0, 2, 8, "Loop");

        var result = AnimationEndpoints.GetAnimationPreview(projectId, 0, service, mapper);

        Assert.IsType<Ok<AnimationPreviewResponse>>(result);
    }
}
