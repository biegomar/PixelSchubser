using System.Collections.Concurrent;
using PixelSchubser.Domain.Entities;
using PixelSchubser.Domain.Profiles;

namespace PixelSchubser.Application.Profiles;

public sealed class PlatformProfileRegistry : IPlatformProfileRegistry
{
    private readonly ConcurrentDictionary<string, PlatformProfile> profiles = new(StringComparer.OrdinalIgnoreCase);

    public PlatformProfileRegistry()
    {
        Register(C64PlatformProfile.CreateDefault());
    }

    public IReadOnlyCollection<PlatformProfile> GetAll() => profiles.Values.ToArray();

    public bool TryGet(string profileId, out PlatformProfile profile)
    {
        var found = profiles.TryGetValue(profileId, out var existing);
        profile = existing!;
        return found;
    }

    public void Register(PlatformProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        profiles[profile.ProfileId] = profile;
    }
}
