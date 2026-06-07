using PixelSchubser.Domain.ValueObjects;

namespace PixelSchubser.Domain.Entities;

public sealed class Sprite
{
    public Sprite(string name, int width, int height)
    {
        Name = name;
        PenGrid = new PenGrid(width, height);
    }

    public string Name { get; }

    public PenGrid PenGrid { get; }

    public void SetPixel(int x, int y, int penValue) => PenGrid.Set(x, y, penValue);

    public void FillSelection(SelectionBounds selection, int penValue)
    {
        for (var y = 0; y < PenGrid.Height; y++)
        {
            for (var x = 0; x < PenGrid.Width; x++)
            {
                if (selection.Contains(x, y))
                {
                    PenGrid.Set(x, y, penValue);
                }
            }
        }
    }
}
