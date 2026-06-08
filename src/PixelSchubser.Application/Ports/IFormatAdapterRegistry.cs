namespace PixelSchubser.Application.Ports;

public interface IFormatAdapterRegistry
{
    IReadOnlyCollection<FormatAdapterDescriptor> GetAll();

    bool TryGet(string formatId, out FormatAdapterDescriptor descriptor);

    void Register(FormatAdapterDescriptor descriptor);
}

public sealed record FormatAdapterDescriptor(string FormatId, bool SupportsImport, bool SupportsExport, int Priority);
