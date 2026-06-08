using PixelSchubser.Application.Profiles;
using PixelSchubser.Api.Contracts.Responses;
using static Microsoft.AspNetCore.Http.TypedResults;

namespace PixelSchubser.Api.Endpoints;

public static class ProfileEndpoints
{
    public static RouteGroupBuilder MapProfileEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/profiles", GetProfiles)
            .Produces<ProfileSummaryResponse[]>(StatusCodes.Status200OK);
        group.MapPost("/profiles/select/{profileId}", SelectProfile)
            .Produces<ProfileSummaryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);
        return group;
    }

    public static IResult GetProfiles(IPlatformProfileRegistry registry)
    {
        var profiles = registry.GetAll()
            .Select(p => new ProfileSummaryResponse(p.ProfileId, p.Name, p.SpriteWidth, p.SpriteHeight))
            .ToArray();

        return Ok(profiles);
    }

    public static IResult SelectProfile(string profileId, IPlatformProfileRegistry registry)
    {
        if (!registry.TryGet(profileId, out var profile))
        {
            return NotFound(new ErrorResponse("profile.notfound", "Profile not found."));
        }

        return Ok(new ProfileSummaryResponse(profile.ProfileId, profile.Name, profile.SpriteWidth, profile.SpriteHeight));
    }
}
