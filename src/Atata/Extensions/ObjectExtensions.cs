namespace Atata;

public static class ObjectExtensions
{
    public static string ToString(this object value, TermCase termCase)
    {
        value.CheckNotNull("value");

        return TermResolver.ToString(value, new TermOptions { Case = termCase });
    }

    internal static string ToFormattedString(this object value, string format) =>
        string.Format(format, value);
}
