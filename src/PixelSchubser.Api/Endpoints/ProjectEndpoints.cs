using PixelSchubser.Api.Contracts.Responses;
using PixelSchubser.Api.Errors;
using PixelSchubser.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using static Microsoft.AspNetCore.Http.TypedResults;

namespace PixelSchubser.Api.Endpoints;

public static class ProjectEndpoints
{
    public static RouteGroupBuilder MapProjectEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/projects", CreateProject)
            .Produces<CreateProjectResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        group.MapGet("/projects/{projectId}", GetProject)
            .Produces<ProjectResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);
        return group;
    }

    public static IResult CreateProject(string name, IProjectAutomationService service, ProblemDetailsMapper mapper)
    {
        var result = service.CreateProject(name);
        return mapper.ToResult(result, projectId => Ok(new CreateProjectResponse(projectId)));
    }

    public static IResult GetProject(string projectId, IProjectAutomationService service, ProblemDetailsMapper mapper)
    {
        var result = service.GetProject(projectId);
        return mapper.ToResult(result, snapshot => Ok(new ProjectResponse(
            snapshot.ProjectId,
            snapshot.Name,
            snapshot.SpriteCount,
            snapshot.AnimationCount,
            snapshot.Width,
            snapshot.Height)));
    }
}
