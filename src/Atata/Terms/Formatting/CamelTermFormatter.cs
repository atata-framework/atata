namespace Atata;

public class CamelTermFormatter : ITermFormatter
{
    public string Format(string[] words) =>
        string.Concat(
            new[] { words[0].ToLower(CultureInfo.CurrentCulture) }
                .Concat(words.Skip(1).Select(x => char.ToUpper(x[0], CultureInfo.CurrentCulture) + x[1..].ToLower(CultureInfo.CurrentCulture))));
}
