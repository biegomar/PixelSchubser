namespace PixelSchubser.Mcp.Tests.Deployment;

public class SelfContainedPublishVerificationTests
{
    [Fact]
    public void SelfContainedPublishProfile_Exists()
    {
        var root = FindRepoRoot(AppContext.BaseDirectory);
        var profile = Path.Combine(root, "src", "PixelSchubser.Mcp", "Properties", "PublishProfiles", "SelfContained.pubxml");
        Assert.True(File.Exists(profile));
    }

    private static string FindRepoRoot(string startDirectory)
    {
        var dir = new DirectoryInfo(startDirectory);
        while (dir is not null)
        {
            if (Directory.Exists(Path.Combine(dir.FullName, "src")) &&
                Directory.Exists(Path.Combine(dir.FullName, "tests")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new InvalidOperationException("Repository root could not be determined.");
    }
}
