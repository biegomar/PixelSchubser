namespace PixelSchubser.Domain.Entities;

public enum AnimationMode
{
    Loop,
    PingPong,
    Once
}

public sealed class Animation
{
    public Animation(string name, int startSpriteIndex, int endSpriteIndex, int fps, AnimationMode mode)
    {
        if (startSpriteIndex > endSpriteIndex)
        {
            throw new ArgumentOutOfRangeException(nameof(startSpriteIndex), "Start index must be <= end index.");
        }

        if (fps <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(fps), "FPS must be positive.");
        }

        Name = name;
        StartSpriteIndex = startSpriteIndex;
        EndSpriteIndex = endSpriteIndex;
        Fps = fps;
        Mode = mode;
    }

    public string Name { get; }

    public int StartSpriteIndex { get; private set; }

    public int EndSpriteIndex { get; private set; }

    public int Fps { get; private set; }

    public AnimationMode Mode { get; private set; }

    public void Update(int startSpriteIndex, int endSpriteIndex, int fps, AnimationMode mode)
    {
        if (startSpriteIndex > endSpriteIndex)
        {
            throw new ArgumentOutOfRangeException(nameof(startSpriteIndex), "Start index must be <= end index.");
        }

        if (fps <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(fps), "FPS must be positive.");
        }

        StartSpriteIndex = startSpriteIndex;
        EndSpriteIndex = endSpriteIndex;
        Fps = fps;
        Mode = mode;
    }
}
