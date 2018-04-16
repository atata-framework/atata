namespace Atata
{
    public class WindowTitleElementDefinitionAttribute : ScopeDefinitionAttribute
    {
        public WindowTitleElementDefinitionAttribute(string scopeXPath = DefaultScopeXPath)
            : base(scopeXPath)
        {
        }
    }
}
