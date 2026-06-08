using PixelSchubser.Api.Contracts.Requests;
using PixelSchubser.Api.Contracts.Responses;
using PixelSchubser.Api.Errors;
using PixelSchubser.Application.Services;
using static Microsoft.AspNetCore.Http.TypedResults;

namespace PixelSchubser.Api.Endpoints;

public static class AnimationEndpoints
{
    public static RouteGroupBuilder MapAnimationEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/projects/{projectId}/animations/settings", SetAnimationSettings)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        group.MapGet("/projects/{projectId}/animations/{animationIndex}/preview", GetAnimationPreview)
            .Produces<AnimationPreviewResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        return group;
    }

    public static IResult SetAnimationSettings(string projectId, SetAnimationRequest request, IProjectAutomationService service, ProblemDetailsMapper mapper)
    {
        var result = service.SetAnimation(projectId, request.AnimationIndex, request.StartSpriteIndex, request.EndSpriteIndex, request.Fps, request.Mode);
        return mapper.ToResult(result);
    }

    public static IResult GetAnimationPreview(string projectId, int animationIndex, IProjectAutomationService service, ProblemDetailsMapper mapper)
    {
        var result = service.GetAnimationPreview(projectId, animationIndex);
        return mapper.ToResult(result, frames => Ok(new AnimationPreviewResponse(projectId, animationIndex, frames)));
    }
}
