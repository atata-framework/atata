#nullable enable

namespace Atata;

public class SnakeTermFormatter : ITermFormatter
{
    public string Format(string[] words, CultureInfo culture) =>
        string.Join("_", words).ToLower(culture);
}
