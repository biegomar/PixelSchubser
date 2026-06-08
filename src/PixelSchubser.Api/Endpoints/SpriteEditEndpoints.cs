using PixelSchubser.Api.Contracts.Requests;
using PixelSchubser.Api.Contracts.Responses;
using PixelSchubser.Api.Errors;
using PixelSchubser.Application.Services;
using static Microsoft.AspNetCore.Http.TypedResults;

namespace PixelSchubser.Api.Endpoints;

public static class SpriteEditEndpoints
{
    public static RouteGroupBuilder MapSpriteEditEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/projects/{projectId}/sprites/pixel", SetPixel)
            .Produces<ProjectResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        group.MapPost("/projects/{projectId}/sprites/fill-selection", FillSelection)
            .Produces<ProjectResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        group.MapPost("/projects/{projectId}/sprites/line", DrawLine)
            .Produces<ProjectResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        group.MapPost("/projects/{projectId}/sprites/flood-fill", FloodFill)
            .Produces<ProjectResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        group.MapPost("/projects/{projectId}/sprites/flip-horizontal", FlipHorizontal)
            .Produces<ProjectResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        group.MapPost("/projects/{projectId}/sprites/flip-vertical", FlipVertical)
            .Produces<ProjectResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        group.MapPost("/projects/{projectId}/sprites/shift-left", ShiftLeft)
            .Produces<ProjectResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        group.MapPost("/projects/{projectId}/sprites/shift-right", ShiftRight)
            .Produces<ProjectResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        group.MapPost("/projects/{projectId}/sprites/shift-up", ShiftUp)
            .Produces<ProjectResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        group.MapPost("/projects/{projectId}/sprites/shift-down", ShiftDown)
            .Produces<ProjectResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        group.MapPost("/projects/{projectId}/sprites/invert", Invert)
            .Produces<ProjectResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        group.MapPost("/projects/{projectId}/sprites/copy-selection", CopySelection)
            .Produces<ProjectResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        group.MapPost("/projects/{projectId}/sprites/move-selection", MoveSelection)
            .Produces<ProjectResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        return group;
    }

    public static IResult SetPixel(string projectId, EditSpriteRequest request, IProjectAutomationService service, ProblemDetailsMapper mapper)
    {
        var result = service.SetPixel(projectId, request.SpriteIndex, request.X, request.Y, request.PenValue);
        return mapper.ToResult(result, snapshot => Ok(new ProjectResponse(
            snapshot.ProjectId,
            snapshot.Name,
            snapshot.SpriteCount,
            snapshot.AnimationCount,
            snapshot.Width,
            snapshot.Height)));
    }

    public static IResult FillSelection(string projectId, FillSelectionRequest request, IProjectAutomationService service, ProblemDetailsMapper mapper)
    {
        var result = service.FillSelection(projectId, request.SpriteIndex, request.X1, request.Y1, request.X2, request.Y2, request.PenValue);
        return ToProjectResult(mapper, result);
    }

    public static IResult DrawLine(string projectId, DrawLineRequest request, IProjectAutomationService service, ProblemDetailsMapper mapper)
    {
        var result = service.DrawLine(projectId, request.SpriteIndex, request.X1, request.Y1, request.X2, request.Y2, request.PenValue);
        return ToProjectResult(mapper, result);
    }

    public static IResult FloodFill(string projectId, FloodFillRequest request, IProjectAutomationService service, ProblemDetailsMapper mapper)
    {
        var result = service.FloodFill(projectId, request.SpriteIndex, request.X, request.Y, request.PenValue);
        return ToProjectResult(mapper, result);
    }

    public static IResult FlipHorizontal(string projectId, SpriteTransformRequest request, IProjectAutomationService service, ProblemDetailsMapper mapper)
    {
        var result = service.FlipHorizontal(projectId, request.SpriteIndex);
        return ToProjectResult(mapper, result);
    }

    public static IResult FlipVertical(string projectId, SpriteTransformRequest request, IProjectAutomationService service, ProblemDetailsMapper mapper)
    {
        var result = service.FlipVertical(projectId, request.SpriteIndex);
        return ToProjectResult(mapper, result);
    }

    public static IResult ShiftLeft(string projectId, SpriteTransformRequest request, IProjectAutomationService service, ProblemDetailsMapper mapper)
    {
        var result = service.ShiftLeft(projectId, request.SpriteIndex);
        return ToProjectResult(mapper, result);
    }

    public static IResult ShiftRight(string projectId, SpriteTransformRequest request, IProjectAutomationService service, ProblemDetailsMapper mapper)
    {
        var result = service.ShiftRight(projectId, request.SpriteIndex);
        return ToProjectResult(mapper, result);
    }

    public static IResult ShiftUp(string projectId, SpriteTransformRequest request, IProjectAutomationService service, ProblemDetailsMapper mapper)
    {
        var result = service.ShiftUp(projectId, request.SpriteIndex);
        return ToProjectResult(mapper, result);
    }

    public static IResult ShiftDown(string projectId, SpriteTransformRequest request, IProjectAutomationService service, ProblemDetailsMapper mapper)
    {
        var result = service.ShiftDown(projectId, request.SpriteIndex);
        return ToProjectResult(mapper, result);
    }

    public static IResult Invert(string projectId, SpriteTransformRequest request, IProjectAutomationService service, ProblemDetailsMapper mapper)
    {
        var result = service.Invert(projectId, request.SpriteIndex);
        return ToProjectResult(mapper, result);
    }

    public static IResult CopySelection(string projectId, SelectionTransferRequest request, IProjectAutomationService service, ProblemDetailsMapper mapper)
    {
        var result = service.CopySelection(projectId, request.SpriteIndex, request.X1, request.Y1, request.X2, request.Y2, request.TargetX, request.TargetY);
        return ToProjectResult(mapper, result);
    }

    public static IResult MoveSelection(string projectId, SelectionTransferRequest request, IProjectAutomationService service, ProblemDetailsMapper mapper)
    {
        var result = service.MoveSelection(projectId, request.SpriteIndex, request.X1, request.Y1, request.X2, request.Y2, request.TargetX, request.TargetY);
        return ToProjectResult(mapper, result);
    }

    private static IResult ToProjectResult(ProblemDetailsMapper mapper, Domain.Results.DomainResult<ProjectSnapshot> result)
    {
        return mapper.ToResult(result, snapshot => Ok(new ProjectResponse(
            snapshot.ProjectId,
            snapshot.Name,
            snapshot.SpriteCount,
            snapshot.AnimationCount,
            snapshot.Width,
            snapshot.Height)));
    }
}
