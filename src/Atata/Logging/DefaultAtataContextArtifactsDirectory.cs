using System;
using System.Globalization;
using System.IO;

namespace Atata
{
    internal static class DefaultAtataContextArtifactsDirectory
    {
        internal const string DefaultDateTimeFormat = "yyyy-MM-dd HH_mm_ss";

        internal static string BuildPath()
        {
            AtataContext context = AtataContext.Current;

            DateTime buildStart = context?.BuildStartInTimeZone
                ?? AtataContext.BuildStart.Value;

            string path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Logs",
                buildStart.ToString(DefaultDateTimeFormat, CultureInfo.InvariantCulture));

            return string.IsNullOrEmpty(context?.TestName)
                ? path
                : Path.Combine(path, context.TestNameSanitized);
        }
    }
}
