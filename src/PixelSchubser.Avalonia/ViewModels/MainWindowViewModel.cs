using System.Collections.ObjectModel;
using Avalonia.Media;
using PixelSchubser.Application.Services;
using PixelSchubser.Domain.Results;

namespace PixelSchubser.Avalonia.ViewModels;

public sealed class MainWindowViewModel : NotifyObject
{
    private enum CanvasInteractionKind
    {
        None,
        Paint,
        Line,
        SelectRect
    }

    private readonly IProjectAutomationService automationService;

    private string? currentProjectId;
    private string projectName = "Spritemate-style Demo";
    private string profileName = "-";
    private string statusMessage = "Ready.";
    private string exportFormat = "spm";
    private string importFormat = "spm";
    private string importPayload = "imported-from-ui";
    private string exportPayload = string.Empty;
    private string previewSummary = string.Empty;
    private string animationPreviewSummary = string.Empty;
    private string selectionX1 = "0";
    private string selectionY1 = "0";
    private string selectionX2 = "5";
    private string selectionY2 = "5";
    private string selectionTargetX = "0";
    private string selectionTargetY = "0";
    private string lineStartX = "0";
    private string lineStartY = "0";
    private string lineEndX = "7";
    private string lineEndY = "7";
    private string floodFillX = "0";
    private string floodFillY = "0";
    private string selectedToolMode = "Pen";
    private string animationStart = "0";
    private string animationEnd = "0";
    private string animationFps = "8";
    private string selectedAnimationMode = "Loop";
    private int spriteWidth = 24;
    private int spriteHeight = 21;
    private int selectedPen = 1;
    private int selectedSpriteIndex;
    private PixelCellViewModel? pendingLineStartCell;
    private PixelCellViewModel? pendingSelectionStartCell;
    private PixelCellViewModel? canvasInteractionStartCell;
    private CanvasInteractionKind canvasInteractionKind;

    public MainWindowViewModel(IProjectAutomationService automationService)
    {
        this.automationService = automationService;

        AvailableFormats = new[] { "spm", "spd", "spr", "png", "spritesheet-png", "asm", "basic" };
        ToolModes = new[] { "Pen", "Line", "FloodFill", "SelectRect" };
        AnimationModes = new[] { "Loop", "PingPong", "Once" };

        Palette = new ObservableCollection<PaletteSwatchViewModel>();
        SpriteCells = new ObservableCollection<PixelCellViewModel>();
        PreviewCells = new ObservableCollection<CanvasPreviewCellViewModel>();
        SpriteStrip = new ObservableCollection<SpriteStripItemViewModel>();
        AnimationSummaries = new ObservableCollection<string>();

        CreateProjectCommand = new DelegateCommand(CreateProject);
        ExportCommand = new DelegateCommand(ExportProject, HasProjectLoaded);
        ImportCommand = new DelegateCommand(ImportProject);
        FillSelectionCommand = new DelegateCommand(FillSelection, HasProjectLoaded);
        CopySelectionCommand = new DelegateCommand(CopySelection, HasProjectLoaded);
        MoveSelectionCommand = new DelegateCommand(MoveSelection, HasProjectLoaded);
        DrawLineCommand = new DelegateCommand(DrawLine, HasProjectLoaded);
        FloodFillCommand = new DelegateCommand(FloodFill, HasProjectLoaded);
        FlipHorizontalCommand = new DelegateCommand(FlipHorizontal, HasProjectLoaded);
        FlipVerticalCommand = new DelegateCommand(FlipVertical, HasProjectLoaded);
        ShiftLeftCommand = new DelegateCommand(ShiftLeft, HasProjectLoaded);
        ShiftRightCommand = new DelegateCommand(ShiftRight, HasProjectLoaded);
        ShiftUpCommand = new DelegateCommand(ShiftUp, HasProjectLoaded);
        ShiftDownCommand = new DelegateCommand(ShiftDown, HasProjectLoaded);
        InvertCommand = new DelegateCommand(Invert, HasProjectLoaded);
        ApplyAnimationCommand = new DelegateCommand(ApplyAnimation, HasProjectLoaded);
        RefreshCommand = new DelegateCommand(RefreshWorkspace, HasProjectLoaded);
        ClearSpriteCommand = new DelegateCommand(ClearSprite, HasProjectLoaded);
        SelectPenCommand = new DelegateCommand<PaletteSwatchViewModel>(SelectPen);
        PaintCellCommand = new DelegateCommand<PixelCellViewModel>(PaintCell, cell => HasProjectLoaded() && cell is not null);
        SelectSpriteCommand = new DelegateCommand<SpriteStripItemViewModel>(SelectSprite, item => item is not null);

        for (var y = 0; y < SpriteHeight; y++)
        {
            for (var x = 0; x < SpriteWidth; x++)
            {
                SpriteCells.Add(new PixelCellViewModel(x, y, 0, Brushes.Transparent));
                PreviewCells.Add(new CanvasPreviewCellViewModel(x, y, Brushes.Transparent));
            }
        }

        CreateProject();
    }

    public ObservableCollection<PaletteSwatchViewModel> Palette { get; }

    public ObservableCollection<PixelCellViewModel> SpriteCells { get; }

    public ObservableCollection<CanvasPreviewCellViewModel> PreviewCells { get; }

    public ObservableCollection<SpriteStripItemViewModel> SpriteStrip { get; }

    public ObservableCollection<string> AnimationSummaries { get; }

    public IReadOnlyList<string> AvailableFormats { get; }

    public IReadOnlyList<string> ToolModes { get; }

    public IReadOnlyList<string> AnimationModes { get; }

    public DelegateCommand CreateProjectCommand { get; }

    public DelegateCommand ExportCommand { get; }

    public DelegateCommand ImportCommand { get; }

    public DelegateCommand FillSelectionCommand { get; }

    public DelegateCommand CopySelectionCommand { get; }

    public DelegateCommand MoveSelectionCommand { get; }

    public DelegateCommand DrawLineCommand { get; }

    public DelegateCommand FloodFillCommand { get; }

    public DelegateCommand FlipHorizontalCommand { get; }

    public DelegateCommand FlipVerticalCommand { get; }

    public DelegateCommand ShiftLeftCommand { get; }

    public DelegateCommand ShiftRightCommand { get; }

    public DelegateCommand ShiftUpCommand { get; }

    public DelegateCommand ShiftDownCommand { get; }

    public DelegateCommand InvertCommand { get; }

    public DelegateCommand ApplyAnimationCommand { get; }

    public DelegateCommand RefreshCommand { get; }

    public DelegateCommand ClearSpriteCommand { get; }

    public DelegateCommand<PaletteSwatchViewModel> SelectPenCommand { get; }

    public DelegateCommand<PixelCellViewModel> PaintCellCommand { get; }

    public DelegateCommand<SpriteStripItemViewModel> SelectSpriteCommand { get; }

    public string ProjectName
    {
        get => projectName;
        set => SetProperty(ref projectName, value);
    }

    public string ProfileName
    {
        get => profileName;
        private set => SetProperty(ref profileName, value);
    }

    public string StatusMessage
    {
        get => statusMessage;
        private set => SetProperty(ref statusMessage, value);
    }

    public string ExportFormat
    {
        get => exportFormat;
        set => SetProperty(ref exportFormat, value);
    }

    public string ImportFormat
    {
        get => importFormat;
        set => SetProperty(ref importFormat, value);
    }

    public string ImportPayload
    {
        get => importPayload;
        set => SetProperty(ref importPayload, value);
    }

    public string ExportPayload
    {
        get => exportPayload;
        private set => SetProperty(ref exportPayload, value);
    }

    public string PreviewSummary
    {
        get => previewSummary;
        private set => SetProperty(ref previewSummary, value);
    }

    public string AnimationPreviewSummary
    {
        get => animationPreviewSummary;
        private set => SetProperty(ref animationPreviewSummary, value);
    }

    public string SelectionX1
    {
        get => selectionX1;
        set => SetProperty(ref selectionX1, value);
    }

    public string SelectionY1
    {
        get => selectionY1;
        set => SetProperty(ref selectionY1, value);
    }

    public string SelectionX2
    {
        get => selectionX2;
        set => SetProperty(ref selectionX2, value);
    }

    public string SelectionY2
    {
        get => selectionY2;
        set => SetProperty(ref selectionY2, value);
    }

    public string LineStartX
    {
        get => lineStartX;
        set => SetProperty(ref lineStartX, value);
    }

    public string LineStartY
    {
        get => lineStartY;
        set => SetProperty(ref lineStartY, value);
    }

    public string LineEndX
    {
        get => lineEndX;
        set => SetProperty(ref lineEndX, value);
    }

    public string LineEndY
    {
        get => lineEndY;
        set => SetProperty(ref lineEndY, value);
    }

    public string FloodFillX
    {
        get => floodFillX;
        set => SetProperty(ref floodFillX, value);
    }

    public string FloodFillY
    {
        get => floodFillY;
        set => SetProperty(ref floodFillY, value);
    }

    public string SelectionTargetX
    {
        get => selectionTargetX;
        set => SetProperty(ref selectionTargetX, value);
    }

    public string SelectionTargetY
    {
        get => selectionTargetY;
        set => SetProperty(ref selectionTargetY, value);
    }

    public string AnimationStart
    {
        get => animationStart;
        set => SetProperty(ref animationStart, value);
    }

    public string SelectedToolMode
    {
        get => selectedToolMode;
        set
        {
            if (!SetProperty(ref selectedToolMode, value))
            {
                return;
            }

            pendingLineStartCell = null;
            pendingSelectionStartCell = null;
        }
    }

    public string AnimationEnd
    {
        get => animationEnd;
        set => SetProperty(ref animationEnd, value);
    }

    public string AnimationFps
    {
        get => animationFps;
        set => SetProperty(ref animationFps, value);
    }

    public string SelectedAnimationMode
    {
        get => selectedAnimationMode;
        set => SetProperty(ref selectedAnimationMode, value);
    }

    public int SpriteWidth
    {
        get => spriteWidth;
        private set => SetProperty(ref spriteWidth, value);
    }

    public int SpriteHeight
    {
        get => spriteHeight;
        private set => SetProperty(ref spriteHeight, value);
    }

    public int SelectedPen
    {
        get => selectedPen;
        private set => SetProperty(ref selectedPen, value);
    }

    public int SelectedSpriteIndex
    {
        get => selectedSpriteIndex;
        private set => SetProperty(ref selectedSpriteIndex, value);
    }

    public string CurrentProjectId => currentProjectId ?? string.Empty;

    public bool HasProject => !string.IsNullOrWhiteSpace(currentProjectId);

    public void CreateProject()
    {
        var name = string.IsNullOrWhiteSpace(ProjectName) ? "Untitled" : ProjectName.Trim();
        var result = automationService.CreateProject(name);
        if (result.IsFailure || result.Value is null)
        {
            SetFailure(result.Error?.Message ?? "Could not create project.");
            return;
        }

        currentProjectId = result.Value;
        SelectedSpriteIndex = 0;
        RaisePropertyChanged(nameof(CurrentProjectId));
        RaisePropertyChanged(nameof(HasProject));
        RefreshWorkspace();
        StatusMessage = $"Project '{name}' created.";
        RaiseCommandStates();
    }

    public void RefreshWorkspace()
    {
        if (!HasProjectLoaded())
        {
            return;
        }

        var workspaceResult = automationService.GetWorkspace(currentProjectId!);
        if (workspaceResult.IsFailure || workspaceResult.Value is null)
        {
            SetFailure(workspaceResult.Error?.Message ?? "Could not load workspace.");
            return;
        }

        ApplyWorkspace(workspaceResult.Value);

        var previewResult = automationService.GetPreview(currentProjectId!);
        PreviewSummary = previewResult.IsSuccess && previewResult.Value is not null
            ? $"Preview payload bytes: {previewResult.Value.Length}"
            : "Preview unavailable.";

        RaiseCommandStates();
    }

    public void PaintCell(PixelCellViewModel? cell)
    {
        if (cell is null || !HasProjectLoaded())
        {
            return;
        }

        switch (SelectedToolMode)
        {
            case "Line":
                HandleLineClick(cell);
                return;
            case "FloodFill":
                FloodFill(cell.X, cell.Y);
                return;
            case "SelectRect":
                HandleSelectionClick(cell);
                return;
            default:
                PaintPixel(cell.X, cell.Y);
                return;
        }
    }

    public void CanvasPointerPressed(PixelCellViewModel? cell)
    {
        if (cell is null || !HasProjectLoaded())
        {
            return;
        }

        switch (SelectedToolMode)
        {
            case "Line":
                canvasInteractionKind = CanvasInteractionKind.Line;
                canvasInteractionStartCell = cell;
                LineStartX = cell.X.ToString();
                LineStartY = cell.Y.ToString();
                UpdateLinePreview(cell, cell);
                StatusMessage = $"Line start set to ({cell.X},{cell.Y}). Drag to preview.";
                break;
            case "SelectRect":
                canvasInteractionKind = CanvasInteractionKind.SelectRect;
                canvasInteractionStartCell = cell;
                SelectionX1 = cell.X.ToString();
                SelectionY1 = cell.Y.ToString();
                SelectionX2 = cell.X.ToString();
                SelectionY2 = cell.Y.ToString();
                UpdateSelectionPreview(cell, cell);
                StatusMessage = $"Selection start set to ({cell.X},{cell.Y}). Drag to preview.";
                break;
            case "FloodFill":
                FloodFill(cell.X, cell.Y);
                break;
            default:
                canvasInteractionKind = CanvasInteractionKind.Paint;
                canvasInteractionStartCell = cell;
                PaintPixel(cell.X, cell.Y);
                break;
        }
    }

    public void CanvasPointerMoved(PixelCellViewModel? cell)
    {
        if (cell is null || !HasProjectLoaded())
        {
            return;
        }

        if (canvasInteractionStartCell is null)
        {
            return;
        }

        switch (canvasInteractionKind)
        {
            case CanvasInteractionKind.Paint:
                PaintPixel(cell.X, cell.Y);
                break;
            case CanvasInteractionKind.Line:
                LineEndX = cell.X.ToString();
                LineEndY = cell.Y.ToString();
                UpdateLinePreview(canvasInteractionStartCell, cell);
                break;
            case CanvasInteractionKind.SelectRect:
                SelectionX1 = Math.Min(canvasInteractionStartCell.X, cell.X).ToString();
                SelectionY1 = Math.Min(canvasInteractionStartCell.Y, cell.Y).ToString();
                SelectionX2 = Math.Max(canvasInteractionStartCell.X, cell.X).ToString();
                SelectionY2 = Math.Max(canvasInteractionStartCell.Y, cell.Y).ToString();
                UpdateSelectionPreview(canvasInteractionStartCell, cell);
                break;
        }
    }

    public void CanvasPointerReleased(PixelCellViewModel? cell)
    {
        if (cell is null || !HasProjectLoaded())
        {
            canvasInteractionKind = CanvasInteractionKind.None;
            canvasInteractionStartCell = null;
            ClearCanvasPreview();
            return;
        }

        switch (canvasInteractionKind)
        {
            case CanvasInteractionKind.Line when canvasInteractionStartCell is not null:
                LineEndX = cell.X.ToString();
                LineEndY = cell.Y.ToString();
                DrawLine();
                break;
            case CanvasInteractionKind.SelectRect when canvasInteractionStartCell is not null:
                SelectionX1 = Math.Min(canvasInteractionStartCell.X, cell.X).ToString();
                SelectionY1 = Math.Min(canvasInteractionStartCell.Y, cell.Y).ToString();
                SelectionX2 = Math.Max(canvasInteractionStartCell.X, cell.X).ToString();
                SelectionY2 = Math.Max(canvasInteractionStartCell.Y, cell.Y).ToString();
                ClearCanvasPreview();
                StatusMessage = $"Selection set to ({SelectionX1},{SelectionY1})..({SelectionX2},{SelectionY2}).";
                break;
        }

        canvasInteractionKind = CanvasInteractionKind.None;
        canvasInteractionStartCell = null;

        if (canvasInteractionKind != CanvasInteractionKind.Line && canvasInteractionKind != CanvasInteractionKind.SelectRect)
        {
            ClearCanvasPreview();
        }
    }

    public void FillSelection()
    {
        if (!TryParseSelection(out var x1, out var y1, out var x2, out var y2))
        {
            return;
        }

        var result = automationService.FillSelection(currentProjectId!, SelectedSpriteIndex, x1, y1, x2, y2, SelectedPen);
        if (result.IsFailure)
        {
            SetFailure(result.Error?.Message ?? "Fill selection failed.");
            return;
        }

        RefreshWorkspace();
        StatusMessage = $"Filled selection ({x1},{y1})..({x2},{y2}) with pen {SelectedPen}.";
    }

    public void DrawLine()
    {
        if (!TryParsePoint(LineStartX, LineStartY, "Line start", out var x1, out var y1) ||
            !TryParsePoint(LineEndX, LineEndY, "Line end", out var x2, out var y2))
        {
            return;
        }

        var result = automationService.DrawLine(currentProjectId!, SelectedSpriteIndex, x1, y1, x2, y2, SelectedPen);
        if (result.IsFailure)
        {
            SetFailure(result.Error?.Message ?? "Line drawing failed.");
            return;
        }

        RefreshWorkspace();
        StatusMessage = $"Drew line ({x1},{y1})..({x2},{y2}) with pen {SelectedPen}.";
        pendingLineStartCell = null;
        ClearCanvasPreview();
    }

    public void FloodFill()
    {
        if (!TryParsePoint(FloodFillX, FloodFillY, "Flood fill", out var x, out var y))
        {
            return;
        }

        FloodFill(x, y);
    }

    public void ClearSprite()
    {
        if (!HasProjectLoaded())
        {
            return;
        }

        var result = automationService.FillSelection(currentProjectId!, SelectedSpriteIndex, 0, 0, SpriteWidth - 1, SpriteHeight - 1, 0);
        if (result.IsFailure)
        {
            SetFailure(result.Error?.Message ?? "Clear failed.");
            return;
        }

        RefreshWorkspace();
        StatusMessage = "Sprite cleared.";
    }

    public void FlipHorizontal()
    {
        RunSpriteTool(() => automationService.FlipHorizontal(currentProjectId!, SelectedSpriteIndex), "Sprite flipped horizontally.");
    }

    public void FlipVertical()
    {
        RunSpriteTool(() => automationService.FlipVertical(currentProjectId!, SelectedSpriteIndex), "Sprite flipped vertically.");
    }

    public void ShiftLeft()
    {
        RunSpriteTool(() => automationService.ShiftLeft(currentProjectId!, SelectedSpriteIndex), "Sprite shifted left.");
    }

    public void ShiftRight()
    {
        RunSpriteTool(() => automationService.ShiftRight(currentProjectId!, SelectedSpriteIndex), "Sprite shifted right.");
    }

    public void ShiftUp()
    {
        RunSpriteTool(() => automationService.ShiftUp(currentProjectId!, SelectedSpriteIndex), "Sprite shifted up.");
    }

    public void ShiftDown()
    {
        RunSpriteTool(() => automationService.ShiftDown(currentProjectId!, SelectedSpriteIndex), "Sprite shifted down.");
    }

    public void Invert()
    {
        RunSpriteTool(() => automationService.Invert(currentProjectId!, SelectedSpriteIndex), "Sprite inverted.");
    }

    public void CopySelection()
    {
        if (!TryParseSelection(out var x1, out var y1, out var x2, out var y2) ||
            !TryParsePoint(SelectionTargetX, SelectionTargetY, "Selection target", out var targetX, out var targetY))
        {
            return;
        }

        var result = automationService.CopySelection(currentProjectId!, SelectedSpriteIndex, x1, y1, x2, y2, targetX, targetY);
        if (result.IsFailure)
        {
            SetFailure(result.Error?.Message ?? "Copy selection failed.");
            return;
        }

        RefreshWorkspace();
        StatusMessage = $"Copied selection to ({targetX},{targetY}).";
    }

    public void MoveSelection()
    {
        if (!TryParseSelection(out var x1, out var y1, out var x2, out var y2) ||
            !TryParsePoint(SelectionTargetX, SelectionTargetY, "Selection target", out var targetX, out var targetY))
        {
            return;
        }

        var result = automationService.MoveSelection(currentProjectId!, SelectedSpriteIndex, x1, y1, x2, y2, targetX, targetY);
        if (result.IsFailure)
        {
            SetFailure(result.Error?.Message ?? "Move selection failed.");
            return;
        }

        RefreshWorkspace();
        StatusMessage = $"Moved selection to ({targetX},{targetY}).";
    }

    public void ApplyAnimation()
    {
        if (!HasProjectLoaded())
        {
            return;
        }

        if (!int.TryParse(AnimationStart, out var start) || !int.TryParse(AnimationEnd, out var end) || !int.TryParse(AnimationFps, out var fps))
        {
            SetFailure("Animation values must be integers.");
            return;
        }

        var result = automationService.SetAnimation(currentProjectId!, 0, start, end, fps, SelectedAnimationMode);
        if (result.IsFailure)
        {
            SetFailure(result.Error?.Message ?? "Animation update failed.");
            return;
        }

        var preview = automationService.GetAnimationPreview(currentProjectId!, 0);
        AnimationPreviewSummary = preview.IsSuccess && preview.Value is not null
            ? string.Join(", ", preview.Value)
            : "No preview frames.";

        RefreshWorkspace();
        StatusMessage = "Animation settings applied.";
    }

    public void ExportProject()
    {
        if (!HasProjectLoaded())
        {
            return;
        }

        var result = automationService.ExportProject(currentProjectId!, ExportFormat);
        if (result.IsFailure || result.Value is null)
        {
            SetFailure(result.Error?.Message ?? "Export failed.");
            return;
        }

        ExportPayload = result.Value;
        StatusMessage = $"Exported project as {ExportFormat}.";
    }

    public void ImportProject()
    {
        var result = automationService.ImportProject(ImportFormat, ImportPayload);
        if (result.IsFailure || result.Value is null)
        {
            SetFailure(result.Error?.Message ?? "Import failed.");
            return;
        }

        currentProjectId = result.Value;
        SelectedSpriteIndex = 0;
        RaisePropertyChanged(nameof(CurrentProjectId));
        RaisePropertyChanged(nameof(HasProject));
        RefreshWorkspace();
        StatusMessage = $"Imported project via {ImportFormat}.";
        RaiseCommandStates();
    }

    public void SelectPen(PaletteSwatchViewModel? swatch)
    {
        if (swatch is null)
        {
            return;
        }

        SelectedPen = swatch.PenValue;
        foreach (var item in Palette)
        {
            item.IsSelected = item.PenValue == SelectedPen;
        }
    }

    public void SelectSprite(SpriteStripItemViewModel? sprite)
    {
        if (sprite is null)
        {
            return;
        }

        SelectedSpriteIndex = sprite.Index;
        RefreshWorkspace();
    }

    private void ApplyWorkspace(WorkspaceSnapshot snapshot)
    {
        ProjectName = snapshot.Name;
        ProfileName = snapshot.ProfileName;
        SpriteWidth = snapshot.SpriteWidth;
        SpriteHeight = snapshot.SpriteHeight;

        Palette.Clear();
        for (var index = 0; index < snapshot.Palette.Count; index++)
        {
            Palette.Add(new PaletteSwatchViewModel(index, snapshot.Palette[index], index == SelectedPen));
        }

        SpriteStrip.Clear();
        for (var index = 0; index < snapshot.Sprites.Count; index++)
        {
            SpriteStrip.Add(new SpriteStripItemViewModel(index, snapshot.Sprites[index].Name, index == SelectedSpriteIndex));
        }

        if (SelectedSpriteIndex >= snapshot.Sprites.Count)
        {
            SelectedSpriteIndex = 0;
        }

        var activeSprite = snapshot.Sprites[SelectedSpriteIndex];
        if (SpriteCells.Count != activeSprite.Width * activeSprite.Height)
        {
            SpriteCells.Clear();
            for (var y = 0; y < activeSprite.Height; y++)
            {
                for (var x = 0; x < activeSprite.Width; x++)
                {
                    SpriteCells.Add(new PixelCellViewModel(x, y, 0, Brushes.Transparent));
                }
            }
        }

        for (var y = 0; y < activeSprite.Height; y++)
        {
            for (var x = 0; x < activeSprite.Width; x++)
            {
                var pixelIndex = (y * activeSprite.Width) + x;
                var penValue = activeSprite.Pixels[pixelIndex];
                var cell = SpriteCells[pixelIndex];
                cell.PenValue = penValue;
                cell.Brush = ToBrush(snapshot.Palette[penValue]);
            }
        }

        AnimationSummaries.Clear();
        foreach (var animation in snapshot.Animations)
        {
            AnimationSummaries.Add($"{animation.Name}: {animation.StartSpriteIndex}-{animation.EndSpriteIndex} @ {animation.Fps}fps ({animation.Mode})");
        }
    }

    private bool TryParseSelection(out int x1, out int y1, out int x2, out int y2)
    {
        if (int.TryParse(SelectionX1, out x1) && int.TryParse(SelectionY1, out y1) && int.TryParse(SelectionX2, out x2) && int.TryParse(SelectionY2, out y2))
        {
            return true;
        }

        x1 = 0;
        y1 = 0;
        x2 = 0;
        y2 = 0;
        SetFailure("Selection coordinates must be integers.");
        return false;
    }

    private bool TryParsePoint(string xText, string yText, string label, out int x, out int y)
    {
        if (int.TryParse(xText, out x) && int.TryParse(yText, out y))
        {
            return true;
        }

        x = 0;
        y = 0;
        SetFailure($"{label} coordinates must be integers.");
        return false;
    }

    private bool HasProjectLoaded() => !string.IsNullOrWhiteSpace(currentProjectId);

    private void HandleLineClick(PixelCellViewModel cell)
    {
        if (pendingLineStartCell is null)
        {
            pendingLineStartCell = cell;
            LineStartX = cell.X.ToString();
            LineStartY = cell.Y.ToString();
            StatusMessage = $"Line start set to ({cell.X},{cell.Y}). Click end point.";
            return;
        }

        LineEndX = cell.X.ToString();
        LineEndY = cell.Y.ToString();
        DrawLine();
    }

    private void HandleSelectionClick(PixelCellViewModel cell)
    {
        if (pendingSelectionStartCell is null)
        {
            pendingSelectionStartCell = cell;
            SelectionX1 = cell.X.ToString();
            SelectionY1 = cell.Y.ToString();
            SelectionX2 = cell.X.ToString();
            SelectionY2 = cell.Y.ToString();
            StatusMessage = $"Selection start set to ({cell.X},{cell.Y}). Click opposite corner.";
            return;
        }

        var startX = pendingSelectionStartCell.X;
        var startY = pendingSelectionStartCell.Y;
        SelectionX1 = Math.Min(startX, cell.X).ToString();
        SelectionY1 = Math.Min(startY, cell.Y).ToString();
        SelectionX2 = Math.Max(startX, cell.X).ToString();
        SelectionY2 = Math.Max(startY, cell.Y).ToString();
        pendingSelectionStartCell = null;
        StatusMessage = $"Selection set to ({SelectionX1},{SelectionY1})..({SelectionX2},{SelectionY2}).";
    }

    private void PaintPixel(int x, int y)
    {
        var result = automationService.SetPixel(currentProjectId!, SelectedSpriteIndex, x, y, SelectedPen);
        if (result.IsFailure)
        {
            SetFailure(result.Error?.Message ?? "Painting failed.");
            return;
        }

        RefreshWorkspace();
        StatusMessage = $"Painted ({x},{y}) with pen {SelectedPen}.";
    }

    private void FloodFill(int x, int y)
    {
        var result = automationService.FloodFill(currentProjectId!, SelectedSpriteIndex, x, y, SelectedPen);
        if (result.IsFailure)
        {
            SetFailure(result.Error?.Message ?? "Flood fill failed.");
            return;
        }

        RefreshWorkspace();
        StatusMessage = $"Flood filled from ({x},{y}) with pen {SelectedPen}.";
    }

    private void RaiseCommandStates()
    {
        ExportCommand.RaiseCanExecuteChanged();
        FillSelectionCommand.RaiseCanExecuteChanged();
        CopySelectionCommand.RaiseCanExecuteChanged();
        MoveSelectionCommand.RaiseCanExecuteChanged();
        DrawLineCommand.RaiseCanExecuteChanged();
        FloodFillCommand.RaiseCanExecuteChanged();
        FlipHorizontalCommand.RaiseCanExecuteChanged();
        FlipVerticalCommand.RaiseCanExecuteChanged();
        ShiftLeftCommand.RaiseCanExecuteChanged();
        ShiftRightCommand.RaiseCanExecuteChanged();
        ShiftUpCommand.RaiseCanExecuteChanged();
        ShiftDownCommand.RaiseCanExecuteChanged();
        InvertCommand.RaiseCanExecuteChanged();
        ApplyAnimationCommand.RaiseCanExecuteChanged();
        RefreshCommand.RaiseCanExecuteChanged();
        ClearSpriteCommand.RaiseCanExecuteChanged();
        PaintCellCommand.RaiseCanExecuteChanged();
    }

    private void SetFailure(string message)
    {
        StatusMessage = message;
    }

    private void RunSpriteTool(Func<DomainResult<ProjectSnapshot>> operation, string successMessage)
    {
        if (!HasProjectLoaded())
        {
            return;
        }

        var result = operation();
        if (result.IsFailure)
        {
            SetFailure(result.Error?.Message ?? "Tool execution failed.");
            return;
        }

        RefreshWorkspace();
        StatusMessage = successMessage;
    }

    public void ClearCanvasPreview()
    {
        foreach (var cell in PreviewCells)
        {
            cell.Clear();
        }
    }

    private void UpdateLinePreview(PixelCellViewModel startCell, PixelCellViewModel endCell)
    {
        ClearCanvasPreview();
        foreach (var (x, y) in GetLinePoints(startCell.X, startCell.Y, endCell.X, endCell.Y))
        {
            SetPreviewCell(x, y, "#71A6D2CC");
        }
    }

    private void UpdateSelectionPreview(PixelCellViewModel startCell, PixelCellViewModel endCell)
    {
        ClearCanvasPreview();

        var x1 = Math.Min(startCell.X, endCell.X);
        var y1 = Math.Min(startCell.Y, endCell.Y);
        var x2 = Math.Max(startCell.X, endCell.X);
        var y2 = Math.Max(startCell.Y, endCell.Y);

        for (var y = y1; y <= y2; y++)
        {
            for (var x = x1; x <= x2; x++)
            {
                SetPreviewCell(x, y, "#C9A22766");
            }
        }
    }

    private void SetPreviewCell(int x, int y, string hexColor)
    {
        if (x < 0 || y < 0 || x >= SpriteWidth || y >= SpriteHeight)
        {
            return;
        }

        var index = (y * SpriteWidth) + x;
        var cell = PreviewCells[index];
        cell.SetOverlay(hexColor);
    }

    private static IEnumerable<(int X, int Y)> GetLinePoints(int x1, int y1, int x2, int y2)
    {
        var dx = Math.Abs(x2 - x1);
        var sx = x1 < x2 ? 1 : -1;
        var dy = -Math.Abs(y2 - y1);
        var sy = y1 < y2 ? 1 : -1;
        var error = dx + dy;

        while (true)
        {
            yield return (x1, y1);
            if (x1 == x2 && y1 == y2)
            {
                yield break;
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

    private static IBrush ToBrush(string hex) => new SolidColorBrush(Color.Parse(hex));
}

public sealed class PixelCellViewModel(int x, int y, int penValue, IBrush brush) : NotifyObject
{
    private int penValue = penValue;
    private IBrush brush = brush;

    public int X { get; } = x;

    public int Y { get; } = y;

    public int PenValue
    {
        get => penValue;
        set => SetProperty(ref penValue, value);
    }

    public IBrush Brush
    {
        get => brush;
        set => SetProperty(ref brush, value);
    }
}

public sealed class CanvasPreviewCellViewModel(int x, int y, IBrush brush) : NotifyObject
{
    private IBrush brush = brush;

    public int X { get; } = x;

    public int Y { get; } = y;

    public IBrush Brush
    {
        get => brush;
        set => SetProperty(ref brush, value);
    }

    public void Clear() => Brush = Brushes.Transparent;

    public void SetOverlay(string hexColor) => Brush = new SolidColorBrush(Color.Parse(hexColor));
}

public sealed class PaletteSwatchViewModel(int penValue, string hexColor, bool isSelected) : NotifyObject
{
    private bool isSelected = isSelected;

    public int PenValue { get; } = penValue;

    public string HexColor { get; } = hexColor;

    public IBrush Brush { get; } = new SolidColorBrush(Color.Parse(hexColor));

    public bool IsSelected
    {
        get => isSelected;
        set => SetProperty(ref isSelected, value);
    }
}

public sealed class SpriteStripItemViewModel(int index, string name, bool isSelected) : NotifyObject
{
    private bool isSelected = isSelected;

    public int Index { get; } = index;

    public string Name { get; } = name;

    public bool IsSelected
    {
        get => isSelected;
        set => SetProperty(ref isSelected, value);
    }
}
