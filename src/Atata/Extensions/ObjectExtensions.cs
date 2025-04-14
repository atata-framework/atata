namespace Atata;

public static class ObjectExtensions
{
    public static string ToString(this object value, TermCase termCase)
    {
        Guard.ThrowIfNull(value);

        return TermResolver.ToString(value, new TermOptions { Case = termCase });
    }
}
