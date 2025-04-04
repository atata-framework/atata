#nullable enable

namespace Atata;

public class LowerMergedTermFormatter : ITermFormatter
{
    public string Format(string[] words, CultureInfo culture) =>
        string.Concat(words)
            .ToLower(culture);
}
