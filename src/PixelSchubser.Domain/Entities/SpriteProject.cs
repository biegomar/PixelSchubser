using PixelSchubser.Domain.Profiles;
using PixelSchubser.Domain.ValueObjects;

namespace PixelSchubser.Domain.Entities;

public sealed class SpriteProject
{
    public SpriteProject(string name, PlatformProfile? profile = null)
    {
        Name = name;
        PlatformProfile = profile ?? C64PlatformProfile.CreateDefault();
        Sprites = new List<Sprite> { new("Sprite 1", PlatformProfile.SpriteWidth, PlatformProfile.SpriteHeight) };
        Animations = new List<Animation>();
        CurrentSelection = SelectionBounds.Empty;
    }

    public string Name { get; }

    public PlatformProfile PlatformProfile { get; }

    public List<Sprite> Sprites { get; }

    public List<Animation> Animations { get; }

    public SelectionBounds CurrentSelection { get; private set; }

    public void SetSelection(SelectionBounds selection) => CurrentSelection = selection;

    public void AddAnimation(Animation animation) => Animations.Add(animation);

    public Sprite GetSprite(int index) => Sprites[index];
}
