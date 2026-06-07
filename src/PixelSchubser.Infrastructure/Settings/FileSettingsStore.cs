using PixelSchubser.Application.Ports;
using PixelSchubser.Domain.Results;

namespace PixelSchubser.Infrastructure.Settings;

public sealed class FileSettingsStore(string settingsFilePath) : ISettingsStore
{
    private readonly string filePath = settingsFilePath;

    public async Task<DomainResult<string?>> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        var settings = await ReadAllAsync(cancellationToken);
        settings.TryGetValue(key, out var value);
        return DomainResult<string?>.Success(value);
    }

    public async Task<DomainResult> SetAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        var settings = await ReadAllAsync(cancellationToken);
        settings[key] = value;

        var lines = settings.Select(kvp => $"{kvp.Key}={kvp.Value}");
        await File.WriteAllLinesAsync(filePath, lines, cancellationToken);
        return DomainResult.Success();
    }

    private async Task<Dictionary<string, string>> ReadAllAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(filePath))
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        var lines = await File.ReadAllLinesAsync(filePath, cancellationToken);
        var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var line in lines)
        {
            var idx = line.IndexOf('=');
            if (idx <= 0)
            {
                continue;
            }

            var key = line[..idx];
            var value = line[(idx + 1)..];
            dict[key] = value;
        }

        return dict;
    }
}
