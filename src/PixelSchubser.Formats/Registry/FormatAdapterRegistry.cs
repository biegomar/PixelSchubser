using System.Collections.Concurrent;
using PixelSchubser.Application.Ports;

namespace PixelSchubser.Formats.Registry;

public sealed class FormatAdapterRegistry : IFormatAdapterRegistry
{
    private readonly ConcurrentDictionary<string, FormatAdapterDescriptor> adapters = new(StringComparer.OrdinalIgnoreCase);

    public FormatAdapterRegistry()
    {
        Register(new FormatAdapterDescriptor("spm", true, true, 100));
        Register(new FormatAdapterDescriptor("spd", true, true, 100));
        Register(new FormatAdapterDescriptor("spr", true, true, 100));
        Register(new FormatAdapterDescriptor("png", false, true, 90));
        Register(new FormatAdapterDescriptor("spritesheet-png", false, true, 90));
        Register(new FormatAdapterDescriptor("asm", false, true, 80));
        Register(new FormatAdapterDescriptor("basic", false, true, 80));
    }

    public IReadOnlyCollection<FormatAdapterDescriptor> GetAll() =>
        adapters.Values.OrderByDescending(x => x.Priority).ToArray();

    public bool TryGet(string formatId, out FormatAdapterDescriptor descriptor)
    {
        var found = adapters.TryGetValue(formatId, out var existing);
        descriptor = existing!;
        return found;
    }

    public void Register(FormatAdapterDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        adapters[descriptor.FormatId] = descriptor;
    }
}
