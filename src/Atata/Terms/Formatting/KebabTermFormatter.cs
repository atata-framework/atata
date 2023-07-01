namespace Atata;

public class KebabTermFormatter : ITermFormatter
{
    public string Format(string[] words) =>
        string.Join("-", words.Select(x => x.ToLower(CultureInfo.CurrentCulture)));
}
