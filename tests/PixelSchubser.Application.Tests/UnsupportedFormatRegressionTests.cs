using PixelSchubser.Application.Services;

namespace PixelSchubser.Application.Tests;

public class UnsupportedFormatRegressionTests
{
    [Fact]
    public void Export_UnsupportedFormat_ReturnsValidationError()
    {
        var service = new InMemoryProjectAutomationService();
        var id = service.CreateProject("fmt").Value!;

        var result = service.ExportProject(id, "zip");

        Assert.True(result.IsFailure);
        Assert.Equal("format.unsupported", result.Error!.Code);
    }
}
