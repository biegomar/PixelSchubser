using PixelSchubser.Application.Services;
using PixelSchubser.Domain.Entities;

namespace PixelSchubser.Application.Tests.Architecture;

public sealed class DependencyRuleTests
{
    [Fact]
    public void ApplicationAssembly_MustNotReferenceAdaptersOrInfrastructure()
    {
        var names = typeof(IProjectAutomationService)
            .Assembly
            .GetReferencedAssemblies()
            .Select(x => x.Name)
            .Where(x => x is not null)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        Assert.DoesNotContain("PixelSchubser.Api", names);
        Assert.DoesNotContain("PixelSchubser.Avalonia", names);
        Assert.DoesNotContain("PixelSchubser.Mcp", names);
        Assert.DoesNotContain("PixelSchubser.Infrastructure", names);
    }

    [Fact]
    public void DomainAssembly_MustNotReferenceApplicationOrAdapters()
    {
        var names = typeof(SpriteProject)
            .Assembly
            .GetReferencedAssemblies()
            .Select(x => x.Name)
            .Where(x => x is not null)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        Assert.DoesNotContain("PixelSchubser.Application", names);
        Assert.DoesNotContain("PixelSchubser.Api", names);
        Assert.DoesNotContain("PixelSchubser.Avalonia", names);
        Assert.DoesNotContain("PixelSchubser.Mcp", names);
        Assert.DoesNotContain("PixelSchubser.Infrastructure", names);
    }
}
