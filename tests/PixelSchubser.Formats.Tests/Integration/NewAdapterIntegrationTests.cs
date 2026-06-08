using PixelSchubser.Application.Ports;
using PixelSchubser.Application.UseCases.Handlers;
using PixelSchubser.Formats.Registry;

namespace PixelSchubser.Formats.Tests.Integration;

public class NewAdapterIntegrationTests
{
    [Fact]
    public void RegisteringNewExportAdapter_DoesNotRequireCoreChange()
    {
        var registry = new FormatAdapterRegistry();
        registry.Register(new FormatAdapterDescriptor("test-export", false, true, 60));
        var handler = new ExportProjectCommandHandler(registry);

        var result = handler.Handle("test-export", "payload");

        Assert.True(result.IsSuccess);
        Assert.Equal("test-export:payload", result.Value);
    }
}
