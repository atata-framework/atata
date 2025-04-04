namespace Atata;

/// <summary>
/// A helper class that guarantees the uniqueness of the artifacts path.
/// </summary>
public static class ArtifactsPathUniquenessGuarantor
{
    private static readonly object s_syncLock = new();

    private static readonly HashSet<string> s_processedArtifactsPaths = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Ensures the Artifacts path is unique.
    /// If the path is already used, then adds a number suffix to the path,
    /// like <c>"_1"</c>, <c>"_2"</c>, etc.
    /// </summary>
    /// <param name="artifactsPath">The artifacts path.</param>
    /// <returns>The original path if it's unique or a path with added suffix.</returns>
    public static string EnsurePathIsUnique(string artifactsPath)
    {
        lock (s_syncLock)
        {
            bool isAddedAsNew = s_processedArtifactsPaths.Add(artifactsPath);

            if (isAddedAsNew)
            {
                return artifactsPath;
            }
            else
            {
                string uniqueArtifactsPath;
                int i = 1;

                do
                {
                    uniqueArtifactsPath = $"{artifactsPath}_{i}";
                    i++;
                }
                while (!s_processedArtifactsPaths.Add(uniqueArtifactsPath));

                return uniqueArtifactsPath;
            }
        }
    }
}
