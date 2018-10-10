namespace Atata
{
    /// <summary>
    /// Specifies the definition of the page object, like scope XPath, component type name, etc.
    /// </summary>
    public class PageObjectDefinitionAttribute : UIComponentDefinitionAttribute
    {
        public PageObjectDefinitionAttribute(string scopeXPath = DefaultScopeXPath)
            : base(scopeXPath)
        {
        }
    }
}
