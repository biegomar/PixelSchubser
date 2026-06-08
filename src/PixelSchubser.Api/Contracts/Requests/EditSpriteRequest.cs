namespace PixelSchubser.Api.Contracts.Requests;

public sealed record EditSpriteRequest(int SpriteIndex, int X, int Y, int PenValue);

public sealed record FillSelectionRequest(int SpriteIndex, int X1, int Y1, int X2, int Y2, int PenValue);

public sealed record DrawLineRequest(int SpriteIndex, int X1, int Y1, int X2, int Y2, int PenValue);

public sealed record FloodFillRequest(int SpriteIndex, int X, int Y, int PenValue);

public sealed record SpriteTransformRequest(int SpriteIndex);

public sealed record SelectionTransferRequest(int SpriteIndex, int X1, int Y1, int X2, int Y2, int TargetX, int TargetY);
