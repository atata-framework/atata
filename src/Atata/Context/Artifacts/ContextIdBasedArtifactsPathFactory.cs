#nullable enable

namespace Atata;

public sealed class ContextIdBasedArtifactsPathFactory : IArtifactsPathFactory
{
    public string Create(AtataContext context) =>
        Sanitize(context.Id);

    private static string Sanitize(string value) =>
        value.SanitizeForFileName('_');
}
