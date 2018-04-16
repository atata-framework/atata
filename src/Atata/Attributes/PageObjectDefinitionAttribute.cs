namespace Atata
{
    public class PageObjectDefinitionAttribute : UIComponentDefinitionAttribute
    {
        public PageObjectDefinitionAttribute(string scopeXPath = DefaultScopeXPath)
            : base(scopeXPath)
        {
        }
    }
}
