namespace Atata.WebDriver;

internal static class StringExtensions
{
    internal static string FormatWith(this string format, params object[] args) =>
        string.Format(format, args);
}
