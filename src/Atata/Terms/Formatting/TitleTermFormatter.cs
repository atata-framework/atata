namespace Atata;

public class TitleTermFormatter : ITermFormatter
{
    private static readonly string[] s_wordsToKeepLower =
    [
        "a",
        "an",
        "the",
        "and",
        "or",
        "but",
        "nor",
        "as",
        "for",
        "of",
        "on",
        "in",
        "at",
        "to",
        "by",
        "to"
    ];

    public string Format(string[] words, CultureInfo culture)
    {
        List<string> updatedWords =
        [
            CapitalizeFirstLetter(words[0], culture)
        ];

        if (words.Length > 2)
            updatedWords.AddRange(words.Skip(1).Take(words.Length - 2).Select(x => CapitalizeFirstLetterExceptSpecial(x, culture)));

        if (words.Length > 1)
            updatedWords.Add(CapitalizeFirstLetter(words[^1], culture));

        return string.Join(" ", updatedWords);
    }

    private static string CapitalizeFirstLetter(string word, CultureInfo culture) =>
        word.IsUpper()
            ? word
            : word.ToUpperFirstLetter(culture);

    private static string CapitalizeFirstLetterExceptSpecial(string word, CultureInfo culture)
    {
        string wordToLower = word.ToLower(culture);

        return s_wordsToKeepLower.Contains(wordToLower)
            ? wordToLower
            : CapitalizeFirstLetter(word, culture);
    }
}
