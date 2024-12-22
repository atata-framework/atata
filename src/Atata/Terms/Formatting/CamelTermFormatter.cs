namespace Atata;

public class CamelTermFormatter : ITermFormatter
{
    public string Format(string[] words, CultureInfo culture) =>
        string.Concat(
            new[] { words[0].ToLower(culture) }
                .Concat(words.Skip(1).Select(x => char.ToUpper(x[0], culture) + x[1..].ToLower(culture))));
}
