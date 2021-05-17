using System.IO;

namespace Atata
{
    internal static class DefaultAtataContextArtifactsDirectory
    {
        internal const string DefaultDateTimeFormat = "yyyy-MM-dd HH_mm_ss";

        internal static string BuildDefaultPath() =>
            Path.Combine(
                "Logs",
                AtataContext.BuildStart.Value.ToString(DefaultDateTimeFormat),
                AtataContext.Current.TestNameSanitized);
    }
}
