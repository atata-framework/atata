namespace Atata;

public class UpperTermFormatter : ITermFormatter
{
    public string Format(string[] words, CultureInfo culture) =>
        string.Join(" ", words)
            .ToUpper(culture);
}
