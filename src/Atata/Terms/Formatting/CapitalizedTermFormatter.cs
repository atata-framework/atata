namespace Atata;

public class CapitalizedTermFormatter : ITermFormatter
{
    public string Format(string[] words) =>
        string.Join(" ", words.Select(x => x.IsUpper() ? x : x.ToUpperFirstLetter()));
}
