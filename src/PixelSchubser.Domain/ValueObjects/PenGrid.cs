namespace PixelSchubser.Domain.ValueObjects;

public sealed class PenGrid
{
    private readonly int[,] cells;

    public PenGrid(int width, int height)
    {
        if (width <= 0 || height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "Grid size must be positive.");
        }

        Width = width;
        Height = height;
        cells = new int[width, height];
    }

    public int Width { get; }

    public int Height { get; }

    public int Get(int x, int y)
    {
        EnsureInBounds(x, y);
        return cells[x, y];
    }

    public void Set(int x, int y, int value)
    {
        EnsureInBounds(x, y);
        cells[x, y] = value;
    }

    public int[] Flatten()
    {
        var result = new int[Width * Height];
        var index = 0;

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                result[index++] = cells[x, y];
            }
        }

        return result;
    }

    private void EnsureInBounds(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
        {
            throw new ArgumentOutOfRangeException($"({x},{y})", "Coordinate outside sprite bounds.");
        }
    }
}
