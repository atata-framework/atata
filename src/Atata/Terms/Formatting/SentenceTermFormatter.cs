#nullable enable

namespace Atata;

public class SentenceTermFormatter : ITermFormatter
{
    public string Format(string[] words, CultureInfo culture) =>
        string.Join(
            " ",
            new[] { words[0].ToUpperFirstLetter(culture) }
                .Concat(words.Skip(1).Select(x => x.Length >= 2 && x.IsUpper() ? x : x.ToLowerFirstLetter(culture))));
}
