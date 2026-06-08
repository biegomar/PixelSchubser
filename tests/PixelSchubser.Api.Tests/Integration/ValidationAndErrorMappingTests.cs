using PixelSchubser.Api.Errors;
using PixelSchubser.Application.Services;

namespace PixelSchubser.Api.Tests.Integration;

public class ValidationAndErrorMappingTests
{
    [Fact]
    public void InvalidFormat_MapsToProblemResult()
    {
        var service = new InMemoryProjectAutomationService();
        var mapper = new ProblemDetailsMapper();
        var id = service.CreateProject("err").Value!;

        var exportResult = service.ExportProject(id, "unsupported");
        var mapped = mapper.ToResult(exportResult);

        Assert.NotNull(mapped);
    }
}
