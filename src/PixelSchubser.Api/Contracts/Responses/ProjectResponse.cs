namespace PixelSchubser.Api.Contracts.Responses;

public sealed record CreateProjectResponse(string ProjectId);

public sealed record ProjectResponse(string ProjectId, string Name, int SpriteCount, int AnimationCount, int Width, int Height);

public sealed record PreviewResponse(string ProjectId, int ByteLength);

public sealed record ExportResponse(string ProjectId, string Format, string Payload);

public sealed record ImportResponse(string ProjectId);

public sealed record ErrorResponse(string Code, string Message);

public sealed record AnimationPreviewResponse(string ProjectId, int AnimationIndex, IReadOnlyList<int> Frames);

public sealed record ProfileSummaryResponse(string ProfileId, string Name, int SpriteWidth, int SpriteHeight);
