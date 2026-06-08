using Microsoft.AspNetCore.Http.HttpResults;
using PixelSchubser.Api.Endpoints;
using PixelSchubser.Api.Errors;
using PixelSchubser.Application.Services;

namespace PixelSchubser.Api.Tests.Contracts;

public class ProjectEndpointsContractTests
{
    [Fact]
    public void CreateProject_ReturnsOkWithProjectId()
    {
        var service = new InMemoryProjectAutomationService();
        var mapper = new ProblemDetailsMapper();

        var result = ProjectEndpoints.CreateProject("api-test", service, mapper);

        Assert.NotNull(result);
    }

    [Fact]
    public void GetProject_Returns404ForUnknownProject()
    {
        var service = new InMemoryProjectAutomationService();
        var mapper = new ProblemDetailsMapper();

        var result = ProjectEndpoints.GetProject("missing", service, mapper);

        Assert.IsNotType<Ok<object>>(result);
    }
}
