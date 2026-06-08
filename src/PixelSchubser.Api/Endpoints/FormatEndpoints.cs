using PixelSchubser.Api.Contracts.Requests;
using PixelSchubser.Api.Contracts.Responses;
using PixelSchubser.Api.Errors;
using PixelSchubser.Application.Services;
using static Microsoft.AspNetCore.Http.TypedResults;

namespace PixelSchubser.Api.Endpoints;

public static class FormatEndpoints
{
    public static RouteGroupBuilder MapFormatEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/projects/{projectId}/export/{format}", Export)
            .Produces<ExportResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        group.MapPost("/projects/import", Import)
            .Produces<ImportResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        return group;
    }

    public static IResult Export(string projectId, string format, IProjectAutomationService service, ProblemDetailsMapper mapper)
    {
        var result = service.ExportProject(projectId, format);
        return mapper.ToResult(result, payload => Ok(new ExportResponse(projectId, format, payload)));
    }

    public static IResult Import(ImportRequest request, IProjectAutomationService service, ProblemDetailsMapper mapper)
    {
        var result = service.ImportProject(request.Format, request.Payload);
        return mapper.ToResult(result, projectId => Ok(new ImportResponse(projectId)));
    }
}
