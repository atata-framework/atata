namespace Atata
{
    public class PageObjectDefinitionAttribute : UIComponentDefinitionAttribute
    {
        public PageObjectDefinitionAttribute(string scopeXPath = null)
            : base(scopeXPath)
        {
        }
    }
}
