namespace Atata;

public static class TermCaseExtensions
{
    public static string ApplyTo(this TermCase termCase, string value, CultureInfo? culture = null) =>
        TermCaseResolver.ApplyCase(value, termCase, culture);
}
