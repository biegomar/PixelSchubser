using Avalonia.Controls;
using Avalonia.Input;
using PixelSchubser.Avalonia.ViewModels;

namespace PixelSchubser.Avalonia;

public partial class MainWindow : Window
{
    private bool isCanvasDragging;

    public MainWindow()
        : this(new MainWindowViewModel(new PixelSchubser.Application.Services.InMemoryProjectAutomationService()))
    {
    }

    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void CanvasCell_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not Control control || control.DataContext is not PixelCellViewModel cell || DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        if (e.GetCurrentPoint(control).Properties.IsLeftButtonPressed)
        {
            isCanvasDragging = true;
            viewModel.CanvasPointerPressed(cell);
        }
    }

    private void CanvasCell_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (!isCanvasDragging || sender is not Control control || control.DataContext is not PixelCellViewModel cell || DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        if (e.GetCurrentPoint(control).Properties.IsLeftButtonPressed)
        {
            viewModel.CanvasPointerMoved(cell);
        }
    }

    private void CanvasCell_PointerEntered(object? sender, PointerEventArgs e)
    {
        CanvasCell_PointerMoved(sender, e);
    }

    private void CanvasCell_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (sender is not Control control || control.DataContext is not PixelCellViewModel cell || DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        if (isCanvasDragging)
        {
            isCanvasDragging = false;
            viewModel.CanvasPointerReleased(cell);
        }
    }
}
