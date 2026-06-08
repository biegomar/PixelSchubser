using PixelSchubser.Api.Contracts.Responses;
using PixelSchubser.Api.Errors;
using PixelSchubser.Application.Services;
using static Microsoft.AspNetCore.Http.TypedResults;

namespace PixelSchubser.Api.Endpoints;

public static class PreviewEndpoints
{
    public static RouteGroupBuilder MapPreviewEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/projects/{projectId}/preview", GetPreview)
            .Produces<PreviewResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);
        return group;
    }

    public static IResult GetPreview(string projectId, IProjectAutomationService service, ProblemDetailsMapper mapper)
    {
        var result = service.GetPreview(projectId);
        return mapper.ToResult(result, bytes => Ok(new PreviewResponse(projectId, bytes.Length)));
    }
}
