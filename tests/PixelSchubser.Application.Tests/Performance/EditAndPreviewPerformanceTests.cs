using System.Diagnostics;
using PixelSchubser.Application.Services;

namespace PixelSchubser.Application.Tests.Performance;

public sealed class EditAndPreviewPerformanceTests
{
    [Fact]
    public void EditAction_P95_MustStayUnder50Ms()
    {
        var service = new InMemoryProjectAutomationService();
        var projectId = service.CreateProject("perf-edit").Value!;
        var samples = new List<double>(capacity: 120);

        for (var i = 0; i < 120; i++)
        {
            var sw = Stopwatch.StartNew();
            _ = service.SetPixel(projectId, 0, i % 24, (i / 24) % 21, (i % 15) + 1);
            sw.Stop();
            samples.Add(sw.Elapsed.TotalMilliseconds);
        }

        var p95 = Percentile(samples, 95);
        Assert.True(p95 < 50, $"Expected edit p95 < 50 ms but was {p95:F3} ms.");
    }

    [Fact]
    public void PreviewRender_P95_MustStayUnder250Ms()
    {
        var service = new InMemoryProjectAutomationService();
        var projectId = service.CreateProject("perf-preview").Value!;
        var samples = new List<double>(capacity: 120);

        for (var i = 0; i < 120; i++)
        {
            _ = service.SetPixel(projectId, 0, i % 24, (i / 24) % 21, (i % 15) + 1);

            var sw = Stopwatch.StartNew();
            _ = service.GetPreview(projectId);
            sw.Stop();
            samples.Add(sw.Elapsed.TotalMilliseconds);
        }

        var p95 = Percentile(samples, 95);
        Assert.True(p95 < 250, $"Expected preview p95 < 250 ms but was {p95:F3} ms.");
    }

    private static double Percentile(IReadOnlyList<double> samples, int percentile)
    {
        var ordered = samples.OrderBy(x => x).ToArray();
        var index = (int)Math.Ceiling((percentile / 100d) * ordered.Length) - 1;
        return ordered[Math.Clamp(index, 0, ordered.Length - 1)];
    }
}
