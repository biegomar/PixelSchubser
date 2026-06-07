namespace PixelSchubser.Application.UndoRedo;

public sealed class UndoRedoCoordinator : IUndoRedoCoordinator
{
    private readonly Stack<object> undoStack = new();
    private readonly Stack<object> redoStack = new();

    public bool CanUndo => undoStack.Count > 0;

    public bool CanRedo => redoStack.Count > 0;

    public void Clear()
    {
        undoStack.Clear();
        redoStack.Clear();
    }

    public void Push(object snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        undoStack.Push(snapshot);
        redoStack.Clear();
    }

    public bool TryUndo(object currentSnapshot, out object? restoredSnapshot)
    {
        ArgumentNullException.ThrowIfNull(currentSnapshot);

        if (!CanUndo)
        {
            restoredSnapshot = null;
            return false;
        }

        redoStack.Push(currentSnapshot);
        restoredSnapshot = undoStack.Pop();
        return true;
    }

    public bool TryRedo(object currentSnapshot, out object? restoredSnapshot)
    {
        ArgumentNullException.ThrowIfNull(currentSnapshot);

        if (!CanRedo)
        {
            restoredSnapshot = null;
            return false;
        }

        undoStack.Push(currentSnapshot);
        restoredSnapshot = redoStack.Pop();
        return true;
    }
}
