using PixelSchubser.Domain.Results;

namespace PixelSchubser.Application.Services;

public interface IProjectAutomationService
{
    DomainResult<string> CreateProject(string name);

    DomainResult<ProjectSnapshot> GetProject(string projectId);

    DomainResult<WorkspaceSnapshot> GetWorkspace(string projectId);

    DomainResult<ProjectSnapshot> SetPixel(string projectId, int spriteIndex, int x, int y, int penValue);

    DomainResult<ProjectSnapshot> DrawLine(string projectId, int spriteIndex, int x1, int y1, int x2, int y2, int penValue);

    DomainResult<ProjectSnapshot> FloodFill(string projectId, int spriteIndex, int x, int y, int penValue);

    DomainResult<ProjectSnapshot> FlipHorizontal(string projectId, int spriteIndex);

    DomainResult<ProjectSnapshot> FlipVertical(string projectId, int spriteIndex);

    DomainResult<ProjectSnapshot> ShiftLeft(string projectId, int spriteIndex);

    DomainResult<ProjectSnapshot> ShiftRight(string projectId, int spriteIndex);

    DomainResult<ProjectSnapshot> ShiftUp(string projectId, int spriteIndex);

    DomainResult<ProjectSnapshot> ShiftDown(string projectId, int spriteIndex);

    DomainResult<ProjectSnapshot> Invert(string projectId, int spriteIndex);

    DomainResult<ProjectSnapshot> CopySelection(string projectId, int spriteIndex, int x1, int y1, int x2, int y2, int targetX, int targetY);

    DomainResult<ProjectSnapshot> MoveSelection(string projectId, int spriteIndex, int x1, int y1, int x2, int y2, int targetX, int targetY);

    DomainResult<ProjectSnapshot> FillSelection(string projectId, int spriteIndex, int x1, int y1, int x2, int y2, int penValue);

    DomainResult<string> ExportProject(string projectId, string format);

    DomainResult<string> ImportProject(string format, string payload);

    DomainResult<byte[]> GetPreview(string projectId);

    DomainResult SetAnimation(string projectId, int animationIndex, int start, int end, int fps, string mode);

    DomainResult<IReadOnlyList<int>> GetAnimationPreview(string projectId, int animationIndex);
}
