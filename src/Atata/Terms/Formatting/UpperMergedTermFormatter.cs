namespace Atata;

public class UpperMergedTermFormatter : ITermFormatter
{
    public string Format(string[] words, CultureInfo culture) =>
        string.Concat(words)
            .ToUpper(culture);
}
