namespace Atata;

public class MidSentenceTermFormatter : ITermFormatter
{
    public string Format(string[] words, CultureInfo culture) =>
        string.Join(
            " ",
            words.Select(x => x.Length >= 2 && x.IsUpper() ? x : x.ToLowerFirstLetter(culture)));
}
