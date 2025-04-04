namespace Atata;

public class IdXPathForLabelAttribute : MulticastAttribute
{
    public IdXPathForLabelAttribute(string? xPathFormat) =>
        XPathFormat = xPathFormat;

    public string? XPathFormat { get; }
}
