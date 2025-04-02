#nullable enable

namespace Atata;

public sealed class ValueXPathAttribute : ExtraXPathAttribute
{
    public ValueXPathAttribute(string xPath)
        : base(xPath)
    {
    }
}
