using Microsoft.Extensions.DependencyInjection;
using PixelSchubser.Application.Ports;
using PixelSchubser.Application.Profiles;
using PixelSchubser.Formats.Registry;
using PixelSchubser.Infrastructure.Logging;

namespace PixelSchubser.Infrastructure.DependencyInjection;

public static class ExtensionRegistration
{
    public static IServiceCollection AddPixelSchubserExtensionRegistries(this IServiceCollection services)
    {
        services.AddSingleton<IPlatformProfileRegistry, PlatformProfileRegistry>();
        services.AddSingleton<IFormatAdapterRegistry, FormatAdapterRegistry>();
        services.AddSingleton<IStructuredLoggerAdapter, StructuredLoggerAdapter>();
        return services;
    }
}
