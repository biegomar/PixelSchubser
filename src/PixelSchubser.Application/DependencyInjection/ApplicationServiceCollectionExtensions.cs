using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PixelSchubser.Application.UndoRedo;

namespace PixelSchubser.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddPixelSchubserApplicationCore(this IServiceCollection services)
    {
        services.TryAddSingleton<IUndoRedoCoordinator, UndoRedoCoordinator>();
        return services;
    }
}
