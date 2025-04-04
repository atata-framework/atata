#nullable enable

namespace Atata;

public class KebabTermFormatter : ITermFormatter
{
    public string Format(string[] words, CultureInfo culture) =>
        string.Join("-", words.Select(x => x.ToLower(culture)));
}
