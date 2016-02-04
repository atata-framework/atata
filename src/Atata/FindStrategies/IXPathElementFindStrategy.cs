namespace Atata
{
    public interface IXPathElementFindStrategy : IElementFindStrategy
    {
        string BuildXPath(ElementFindOptions options);
    }
}
