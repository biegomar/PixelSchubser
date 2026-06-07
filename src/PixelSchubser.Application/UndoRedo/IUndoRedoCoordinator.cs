namespace PixelSchubser.Application.UndoRedo;

public interface IUndoRedoCoordinator
{
    bool CanUndo { get; }

    bool CanRedo { get; }

    void Clear();

    void Push(object snapshot);

    bool TryUndo(object currentSnapshot, out object? restoredSnapshot);

    bool TryRedo(object currentSnapshot, out object? restoredSnapshot);
}
