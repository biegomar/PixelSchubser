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

    public void DrawLine(int x1, int y1, int x2, int y2, int penValue)
    {
        var dx = Math.Abs(x2 - x1);
        var sx = x1 < x2 ? 1 : -1;
        var dy = -Math.Abs(y2 - y1);
        var sy = y1 < y2 ? 1 : -1;
        var error = dx + dy;

        while (true)
        {
            PenGrid.Set(x1, y1, penValue);
            if (x1 == x2 && y1 == y2)
            {
                break;
            }

            var twiceError = 2 * error;
            if (twiceError >= dy)
            {
                error += dy;
                x1 += sx;
            }

            if (twiceError <= dx)
            {
                error += dx;
                y1 += sy;
            }
        }
    }

    public void FloodFill(int x, int y, int penValue)
    {
        var target = PenGrid.Get(x, y);
        if (target == penValue)
        {
            return;
        }

        var queue = new Queue<(int X, int Y)>();
        queue.Enqueue((x, y));

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current.X < 0 || current.Y < 0 || current.X >= PenGrid.Width || current.Y >= PenGrid.Height)
            {
                continue;
            }

            if (PenGrid.Get(current.X, current.Y) != target)
            {
                continue;
            }

            PenGrid.Set(current.X, current.Y, penValue);
            queue.Enqueue((current.X - 1, current.Y));
            queue.Enqueue((current.X + 1, current.Y));
            queue.Enqueue((current.X, current.Y - 1));
            queue.Enqueue((current.X, current.Y + 1));
        }
    }

    public void FlipHorizontal()
    {
        var buffer = new int[PenGrid.Width * PenGrid.Height];
        for (var y = 0; y < PenGrid.Height; y++)
        {
            for (var x = 0; x < PenGrid.Width; x++)
            {
                buffer[(y * PenGrid.Width) + (PenGrid.Width - 1 - x)] = PenGrid.Get(x, y);
            }
        }

        ApplyBuffer(buffer);
    }

    public void FlipVertical()
    {
        var buffer = new int[PenGrid.Width * PenGrid.Height];
        for (var y = 0; y < PenGrid.Height; y++)
        {
            for (var x = 0; x < PenGrid.Width; x++)
            {
                buffer[((PenGrid.Height - 1 - y) * PenGrid.Width) + x] = PenGrid.Get(x, y);
            }
        }

        ApplyBuffer(buffer);
    }

    public void ShiftLeft()
    {
        var buffer = new int[PenGrid.Width * PenGrid.Height];
        for (var y = 0; y < PenGrid.Height; y++)
        {
            for (var x = 0; x < PenGrid.Width - 1; x++)
            {
                buffer[(y * PenGrid.Width) + x] = PenGrid.Get(x + 1, y);
            }
        }

        ApplyBuffer(buffer);
    }

    public void ShiftRight()
    {
        var buffer = new int[PenGrid.Width * PenGrid.Height];
        for (var y = 0; y < PenGrid.Height; y++)
        {
            for (var x = 1; x < PenGrid.Width; x++)
            {
                buffer[(y * PenGrid.Width) + x] = PenGrid.Get(x - 1, y);
            }
        }

        ApplyBuffer(buffer);
    }

    public void ShiftUp()
    {
        var buffer = new int[PenGrid.Width * PenGrid.Height];
        for (var y = 0; y < PenGrid.Height - 1; y++)
        {
            for (var x = 0; x < PenGrid.Width; x++)
            {
                buffer[(y * PenGrid.Width) + x] = PenGrid.Get(x, y + 1);
            }
        }

        ApplyBuffer(buffer);
    }

    public void ShiftDown()
    {
        var buffer = new int[PenGrid.Width * PenGrid.Height];
        for (var y = 1; y < PenGrid.Height; y++)
        {
            for (var x = 0; x < PenGrid.Width; x++)
            {
                buffer[(y * PenGrid.Width) + x] = PenGrid.Get(x, y - 1);
            }
        }

        ApplyBuffer(buffer);
    }

    public void Invert(int maxPenValue)
    {
        for (var y = 0; y < PenGrid.Height; y++)
        {
            for (var x = 0; x < PenGrid.Width; x++)
            {
                var current = PenGrid.Get(x, y);
                PenGrid.Set(x, y, maxPenValue - current);
            }
        }
    }

    public void CopySelection(SelectionBounds selection, int targetX, int targetY)
    {
        CopyOrMoveSelection(selection, targetX, targetY, clearSource: false);
    }

    public void MoveSelection(SelectionBounds selection, int targetX, int targetY)
    {
        CopyOrMoveSelection(selection, targetX, targetY, clearSource: true);
    }

    private void ApplyBuffer(IReadOnlyList<int> buffer)
    {
        var index = 0;
        for (var y = 0; y < PenGrid.Height; y++)
        {
            for (var x = 0; x < PenGrid.Width; x++)
            {
                PenGrid.Set(x, y, buffer[index++]);
            }
        }
    }

    private void CopyOrMoveSelection(SelectionBounds selection, int targetX, int targetY, bool clearSource)
    {
        if (!selection.IsActive)
        {
            return;
        }

        var width = selection.X2 - selection.X1 + 1;
        var height = selection.Y2 - selection.Y1 + 1;
        var buffer = new int[width * height];

        for (var row = 0; row < height; row++)
        {
            for (var col = 0; col < width; col++)
            {
                var sourceX = selection.X1 + col;
                var sourceY = selection.Y1 + row;
                buffer[(row * width) + col] = PenGrid.Get(sourceX, sourceY);
            }
        }

        if (clearSource)
        {
            for (var row = 0; row < height; row++)
            {
                for (var col = 0; col < width; col++)
                {
                    var sourceX = selection.X1 + col;
                    var sourceY = selection.Y1 + row;
                    PenGrid.Set(sourceX, sourceY, 0);
                }
            }
        }

        for (var row = 0; row < height; row++)
        {
            for (var col = 0; col < width; col++)
            {
                var targetCellX = targetX + col;
                var targetCellY = targetY + row;

                if (targetCellX < 0 || targetCellY < 0 || targetCellX >= PenGrid.Width || targetCellY >= PenGrid.Height)
                {
                    continue;
                }

                PenGrid.Set(targetCellX, targetCellY, buffer[(row * width) + col]);
            }
        }
    }
}
