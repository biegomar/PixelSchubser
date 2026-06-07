using PixelSchubser.Application.UseCases.Commands;
using PixelSchubser.Application.UseCases.Handlers;
using PixelSchubser.Application.UseCases.Queries;
using PixelSchubser.Domain.Entities;

namespace PixelSchubser.Application.Tests;

public class AnimationPreviewUseCaseTests
{
    [Fact]
    public async Task PreviewQuery_ReturnsExpectedFrameRange()
    {
        var project = new SpriteProject("demo");
        project.AddAnimation(new Animation("idle", 0, 3, 8, AnimationMode.Loop));
        var handler = new GetAnimationPreviewQueryHandler();

        var frames = await handler.HandleAsync(new GetAnimationPreviewQuery(project, 0));

        Assert.Equal(new[] { 0, 1, 2, 3 }, frames);
    }

    [Fact]
    public async Task SetAnimationSettings_UpdatesAnimation()
    {
        var project = new SpriteProject("demo");
        project.AddAnimation(new Animation("idle", 0, 3, 8, AnimationMode.Loop));
        var handler = new SetAnimationSettingsCommandHandler();

        await handler.HandleAsync(new SetAnimationSettingsCommand(project, 0, 1, 4, 12, AnimationMode.PingPong));

        var animation = project.Animations[0];
        Assert.Equal(1, animation.StartSpriteIndex);
        Assert.Equal(4, animation.EndSpriteIndex);
        Assert.Equal(12, animation.Fps);
        Assert.Equal(AnimationMode.PingPong, animation.Mode);
    }
}
