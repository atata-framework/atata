namespace Atata;

public class PascalHyphenKebabTermFormatter : ITermFormatter
{
    public string Format(string[] words) =>
        string.Join("‐", words.Select(x => char.ToUpper(x[0], CultureInfo.CurrentCulture) + x[1..].ToLower(CultureInfo.CurrentCulture)));
}
