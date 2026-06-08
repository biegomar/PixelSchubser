using PixelSchubser.Domain.Entities;

namespace PixelSchubser.Application.Profiles;

public interface IPlatformProfileRegistry
{
    IReadOnlyCollection<PlatformProfile> GetAll();

    bool TryGet(string profileId, out PlatformProfile profile);

    void Register(PlatformProfile profile);
}
