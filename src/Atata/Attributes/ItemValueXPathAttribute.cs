#nullable enable

namespace Atata;

public sealed class ItemValueXPathAttribute : ExtraXPathAttribute
{
    public ItemValueXPathAttribute(string xPath)
        : base(xPath)
    {
    }
}
