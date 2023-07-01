namespace Atata;

public class LowerTermFormatter : ITermFormatter
{
    public string Format(string[] words) =>
        string.Join(" ", words).ToLower(CultureInfo.CurrentCulture);
}
