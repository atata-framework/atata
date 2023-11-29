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

    public string Format(string[] words)
    {
        List<string> updatedWords =
        [
            CapitalizeFirstLetter(words[0])
        ];

        if (words.Length > 2)
            updatedWords.AddRange(words.Skip(1).Take(words.Length - 2).Select(CapitalizeFirstLetterExceptSpecial));

        if (words.Length > 1)
            updatedWords.Add(CapitalizeFirstLetter(words[words.Length - 1]));

        return string.Join(" ", updatedWords);
    }

    private static string CapitalizeFirstLetter(string word) =>
        word.IsUpper()
            ? word
            : word.ToUpperFirstLetter();

    private static string CapitalizeFirstLetterExceptSpecial(string word)
    {
        string wordToLower = word.ToLower(CultureInfo.CurrentCulture);
        return s_wordsToKeepLower.Contains(wordToLower) ? wordToLower : CapitalizeFirstLetter(word);
    }
}
