using Microsoft.AspNetCore.Http.HttpResults;
using PixelSchubser.Api.Endpoints;
using PixelSchubser.Application.Profiles;
using PixelSchubser.Domain.Profiles;

namespace PixelSchubser.Api.Tests.Integration;

public class ProfileSelectionTests
{
    [Fact]
    public void GetProfiles_ReturnsOk()
    {
        var registry = new PlatformProfileRegistry();

        var result = ProfileEndpoints.GetProfiles(registry);

        Assert.NotNull(result);
    }

    [Fact]
    public void SelectProfile_ReturnsNotFoundForUnknownId()
    {
        var registry = new PlatformProfileRegistry();

        var result = ProfileEndpoints.SelectProfile("missing", registry);

        Assert.NotNull(result);
    }

    [Fact]
    public void SelectProfile_ReturnsOkForKnownId()
    {
        var registry = new PlatformProfileRegistry();
        registry.Register(NesPlatformProfile.CreateDefault());

        var result = ProfileEndpoints.SelectProfile("nes", registry);

        Assert.NotNull(result);
    }
}
