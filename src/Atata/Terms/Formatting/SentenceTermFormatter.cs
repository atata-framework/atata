namespace Atata;

public class SentenceTermFormatter : ITermFormatter
{
    public string Format(string[] words) =>
        string.Join(
            " ",
            new[] { words[0].ToUpperFirstLetter() }.Concat(words.Skip(1).Select(x => x.Length >= 2 && x.IsUpper() ? x : x.ToLowerFirstLetter())));
}
