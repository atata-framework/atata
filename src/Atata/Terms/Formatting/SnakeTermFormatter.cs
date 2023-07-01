namespace Atata;

public class SnakeTermFormatter : ITermFormatter
{
    public string Format(string[] words) =>
        string.Join("_", words).ToLower(CultureInfo.CurrentCulture);
}
