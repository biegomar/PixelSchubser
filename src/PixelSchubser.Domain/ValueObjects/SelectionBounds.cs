namespace PixelSchubser.Domain.ValueObjects;

public sealed record SelectionBounds(int X1, int Y1, int X2, int Y2)
{
    public static SelectionBounds Empty { get; } = new(0, 0, 0, 0);

    public bool IsActive => X2 >= X1 && Y2 >= Y1;

    public bool Contains(int x, int y) => IsActive && x >= X1 && x <= X2 && y >= Y1 && y <= Y2;
}
