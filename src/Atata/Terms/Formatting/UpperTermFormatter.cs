namespace Atata;

public class UpperTermFormatter : ITermFormatter
{
    public string Format(string[] words) =>
        string.Join(" ", words)
            .ToUpper(CultureInfo.CurrentCulture);
}
