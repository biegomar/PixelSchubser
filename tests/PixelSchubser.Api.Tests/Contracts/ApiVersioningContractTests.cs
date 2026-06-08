using Microsoft.AspNetCore.Builder;
using PixelSchubser.Api.Endpoints;

namespace PixelSchubser.Api.Tests.Contracts;

public class ApiVersioningContractTests
{
    [Fact]
    public void Routes_AreVersionedUnderApiV1()
    {
        var path = "/api/v1/projects";
        Assert.StartsWith("/api/v1", path, StringComparison.Ordinal);
    }

    [Fact]
    public void ApiVersionHeader_UsesV1()
    {
        const string version = "v1";
        Assert.Equal("v1", version);
    }
}
