#nullable enable

namespace Atata;

public class PascalHyphenKebabTermFormatter : ITermFormatter
{
    public string Format(string[] words, CultureInfo culture) =>
        string.Join("‐", words.Select(x => char.ToUpper(x[0], culture) + x[1..].ToLower(culture)));
}
