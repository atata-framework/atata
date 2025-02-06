#nullable enable

namespace Atata;

public sealed class ContextIdBasedHierarchicalArtifactsPathFactory : IArtifactsPathFactory
{
    public string Create(AtataContext context)
    {
        if (context.Scope == AtataContextScope.Global)
            return string.Empty;

        string path = Sanitize(context.Id);
        string? parentPath = context.ParentContext?.ArtifactsRelativePath;

        return parentPath is null
            ? path
            : Path.Combine(parentPath, path);
    }

    private static string Sanitize(string value) =>
        value.SanitizeForFileName('_');
}
