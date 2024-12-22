namespace Atata;

public class CapitalizedTermFormatter : ITermFormatter
{
    public string Format(string[] words, CultureInfo culture) =>
        string.Join(" ", words.Select(x => x.IsUpper() ? x : x.ToUpperFirstLetter(culture)));
}
