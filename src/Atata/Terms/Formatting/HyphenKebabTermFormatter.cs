namespace Atata;

public class HyphenKebabTermFormatter : ITermFormatter
{
    public string Format(string[] words, CultureInfo culture) =>
        string.Join("‐", words.Select(x => x.ToLower(culture)));
}
