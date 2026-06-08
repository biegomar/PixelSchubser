using PixelSchubser.Avalonia.ViewModels;
using PixelSchubser.Application.Services;

namespace PixelSchubser.Avalonia.Tests;

public sealed class EditorViewModelTests
{
    [Fact]
    public void Constructor_CreatesInitialProjectAndCanvas()
    {
        var viewModel = new MainWindowViewModel(new InMemoryProjectAutomationService());

        Assert.True(viewModel.HasProject);
        Assert.Equal(24 * 21, viewModel.SpriteCells.Count);
        Assert.NotEmpty(viewModel.Palette);
    }

    [Fact]
    public void PaintCellAndExport_UpdateEditorState()
    {
        var viewModel = new MainWindowViewModel(new InMemoryProjectAutomationService());
        var cell = viewModel.SpriteCells[(3 * viewModel.SpriteWidth) + 2];

        viewModel.PaintCell(cell);
        var paintedCell = viewModel.SpriteCells[(3 * viewModel.SpriteWidth) + 2];
        viewModel.ExportProject();

        Assert.Equal(viewModel.SelectedPen, paintedCell.PenValue);
        Assert.Contains("Exported", viewModel.StatusMessage);
        Assert.NotEmpty(viewModel.ExportPayload);
    }

    [Fact]
    public void DrawLineAndShiftLeft_UpdateCanvasState()
    {
        var viewModel = new MainWindowViewModel(new InMemoryProjectAutomationService())
        {
            LineStartX = "0",
            LineStartY = "0",
            LineEndX = "2",
            LineEndY = "0"
        };

        viewModel.DrawLine();
        viewModel.ShiftLeft();

        Assert.Equal(viewModel.SelectedPen, viewModel.SpriteCells[0].PenValue);
        Assert.Equal(viewModel.SelectedPen, viewModel.SpriteCells[1].PenValue);
        Assert.Equal(0, viewModel.SpriteCells[2].PenValue);
        Assert.Contains("shifted left", viewModel.StatusMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CanvasLineMode_UsesTwoClicksForLineDrawing()
    {
        var viewModel = new MainWindowViewModel(new InMemoryProjectAutomationService())
        {
            SelectedToolMode = "Line"
        };

        viewModel.PaintCell(viewModel.SpriteCells[0]);
        viewModel.PaintCell(viewModel.SpriteCells[2]);

        Assert.Equal(viewModel.SelectedPen, viewModel.SpriteCells[0].PenValue);
        Assert.Equal(viewModel.SelectedPen, viewModel.SpriteCells[1].PenValue);
        Assert.Equal(viewModel.SelectedPen, viewModel.SpriteCells[2].PenValue);
    }

    [Fact]
    public void CanvasFloodFillMode_FillsFromClickedCell()
    {
        var viewModel = new MainWindowViewModel(new InMemoryProjectAutomationService())
        {
            SelectedToolMode = "FloodFill"
        };

        viewModel.PaintCell(viewModel.SpriteCells[0]);

        Assert.Equal(viewModel.SelectedPen, viewModel.SpriteCells[0].PenValue);
        Assert.Contains("Flood filled", viewModel.StatusMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void SelectRectModeAndMoveSelection_UpdatesSelectionAndMovesPixels()
    {
        var viewModel = new MainWindowViewModel(new InMemoryProjectAutomationService())
        {
            SelectedToolMode = "Pen"
        };

        viewModel.PaintCell(viewModel.SpriteCells[0]);
        viewModel.SelectedToolMode = "SelectRect";
        viewModel.PaintCell(viewModel.SpriteCells[0]);
        viewModel.PaintCell(viewModel.SpriteCells[0]);
        viewModel.SelectionTargetX = "2";
        viewModel.SelectionTargetY = "1";

        viewModel.MoveSelection();

        var targetIndex = (1 * viewModel.SpriteWidth) + 2;
        Assert.Equal(viewModel.SelectedPen, viewModel.SpriteCells[targetIndex].PenValue);
        Assert.Contains("Moved selection", viewModel.StatusMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void LineDrag_ShowsPreviewOverlayBeforeCommit()
    {
        var viewModel = new MainWindowViewModel(new InMemoryProjectAutomationService())
        {
            SelectedToolMode = "Line"
        };

        viewModel.CanvasPointerPressed(viewModel.SpriteCells[0]);
        viewModel.CanvasPointerMoved(viewModel.SpriteCells[2]);

        Assert.NotEqual(global::Avalonia.Media.Brushes.Transparent, viewModel.PreviewCells[0].Brush);
        Assert.NotEqual(global::Avalonia.Media.Brushes.Transparent, viewModel.PreviewCells[1].Brush);
        Assert.NotEqual(global::Avalonia.Media.Brushes.Transparent, viewModel.PreviewCells[2].Brush);
    }

    [Fact]
    public void SelectionDrag_ShowsRectangleOverlayBeforeCommit()
    {
        var viewModel = new MainWindowViewModel(new InMemoryProjectAutomationService())
        {
            SelectedToolMode = "SelectRect"
        };

        viewModel.CanvasPointerPressed(viewModel.SpriteCells[0]);
        viewModel.CanvasPointerMoved(viewModel.SpriteCells[(1 * viewModel.SpriteWidth) + 1]);

        Assert.NotEqual(global::Avalonia.Media.Brushes.Transparent, viewModel.PreviewCells[0].Brush);
        Assert.NotEqual(global::Avalonia.Media.Brushes.Transparent, viewModel.PreviewCells[1].Brush);
        Assert.NotEqual(global::Avalonia.Media.Brushes.Transparent, viewModel.PreviewCells[viewModel.SpriteWidth].Brush);
    }
}
