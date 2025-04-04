namespace Atata;

public class LowerTermFormatter : ITermFormatter
{
    public string Format(string[] words, CultureInfo culture) =>
        string.Join(" ", words)
            .ToLower(culture);
}
