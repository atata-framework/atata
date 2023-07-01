namespace Atata;

public class PascalTermFormatter : ITermFormatter
{
    public string Format(string[] words) =>
        string.Concat(words.Select(x => char.ToUpper(x[0], CultureInfo.CurrentCulture) + x.Substring(1).ToLower(CultureInfo.CurrentCulture)));
}
