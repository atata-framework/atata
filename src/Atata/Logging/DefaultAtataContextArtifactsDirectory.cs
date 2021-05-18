using System;
using System.IO;

namespace Atata
{
    internal static class DefaultAtataContextArtifactsDirectory
    {
        internal const string DefaultDateTimeFormat = "yyyy-MM-dd HH_mm_ss";

        internal static string BuildPath()
        {
            string path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Logs",
                AtataContext.BuildStart.Value.ToString(DefaultDateTimeFormat));

            return string.IsNullOrEmpty(AtataContext.Current.TestName)
                ? path
                : Path.Combine(path, AtataContext.Current.TestNameSanitized);
        }
    }
}
