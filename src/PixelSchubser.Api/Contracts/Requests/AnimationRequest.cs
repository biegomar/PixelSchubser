namespace PixelSchubser.Api.Contracts.Requests;

public sealed record SetAnimationRequest(int AnimationIndex, int StartSpriteIndex, int EndSpriteIndex, int Fps, string Mode);
