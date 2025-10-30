namespace Silksong.Modding.Templates.Tests;

internal static class PathHelper
{
    public static string RootDir { get; } = GetRootDir();
    public static string TemplateContentDir { get; } =
        Path.Combine(RootDir, "Silksong.Modding.Templates", "content");

    private static string GetRootDir()
    {
        string assemblyPath = typeof(SilksongPluginTest).Assembly.Location;
        DirectoryInfo? rootDir = new FileInfo(assemblyPath).Directory;
        // iterate up the directory tree until we find where the solution lives (or we hit the file system root)
        while (
            rootDir != null
            && !File.Exists(Path.Combine(rootDir.FullName, "Silksong.Modding.Templates.sln"))
        )
        {
            rootDir = rootDir.Parent;
        }
        string? rootPath = rootDir?.FullName;

        if (string.IsNullOrEmpty(rootPath))
        {
            throw new InvalidOperationException("The codebase root was not found");
        }
        return rootPath;
    }

    /// <summary>
    /// Builds the glob patterns matching paths relative to the content directory of the created plugin.
    /// This does 2 things fundamentally:
    /// <list type="number">
    /// <item>Moves all paths down 1 level to account for the nesting of the output directory</item>
    /// <item>Creates a version for both / and \ path separators because Microsoft couldn't be bothered to care about cross-platform in their custom glob tool</item>
    /// </list>
    /// </summary>
    /// <param name="globs">The globs to be transformed</param>
    public static IEnumerable<string> ContentRelativeGlobs(params string[] globs)
    {
        foreach (string origGlob in globs)
        {
            string unixGlob = "*/" + origGlob.Replace('\\', '/');
            string winGlob = unixGlob.Replace('/', '\\');
            yield return unixGlob;
            yield return winGlob;
        }
    }
}
