namespace Atata;

public class PascalTermFormatter : ITermFormatter
{
    public string Format(string[] words, CultureInfo culture) =>
        string.Concat(words.Select(x => char.ToUpper(x[0], culture) + x[1..].ToLower(culture)));
}
