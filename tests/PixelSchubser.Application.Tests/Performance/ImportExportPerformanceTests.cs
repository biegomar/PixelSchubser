using System.Diagnostics;
using PixelSchubser.Application.Services;

namespace PixelSchubser.Application.Tests.Performance;

public sealed class ImportExportPerformanceTests
{
    [Fact]
    public void ExportPath_P95_MustStayUnder250Ms()
    {
        var service = new InMemoryProjectAutomationService();
        var projectId = service.CreateProject("perf-export").Value!;
        var samples = new List<double>(capacity: 120);

        for (var i = 0; i < 120; i++)
        {
            _ = service.SetPixel(projectId, 0, i % 24, (i / 24) % 21, (i % 15) + 1);

            var sw = Stopwatch.StartNew();
            _ = service.ExportProject(projectId, "spm");
            sw.Stop();
            samples.Add(sw.Elapsed.TotalMilliseconds);
        }

        var p95 = Percentile(samples, 95);
        Assert.True(p95 < 250, $"Expected export p95 < 250 ms but was {p95:F3} ms.");
    }

    [Fact]
    public void ImportPath_P95_MustStayUnder250Ms()
    {
        var service = new InMemoryProjectAutomationService();
        var samples = new List<double>(capacity: 120);

        for (var i = 0; i < 120; i++)
        {
            var payload = $"import-payload-{i}";

            var sw = Stopwatch.StartNew();
            _ = service.ImportProject("spm", payload);
            sw.Stop();
            samples.Add(sw.Elapsed.TotalMilliseconds);
        }

        var p95 = Percentile(samples, 95);
        Assert.True(p95 < 250, $"Expected import p95 < 250 ms but was {p95:F3} ms.");
    }

    private static double Percentile(IReadOnlyList<double> samples, int percentile)
    {
        var ordered = samples.OrderBy(x => x).ToArray();
        var index = (int)Math.Ceiling((percentile / 100d) * ordered.Length) - 1;
        return ordered[Math.Clamp(index, 0, ordered.Length - 1)];
    }
}
