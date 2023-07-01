namespace Atata;

public class LowerMergedTermFormatter : ITermFormatter
{
    public string Format(string[] words) =>
        string.Concat(words).ToLower(CultureInfo.CurrentCulture);
}
