namespace Atata
{
    public class ControlDefinitionAttribute : UIComponentDefinitionAttribute
    {
        public ControlDefinitionAttribute(string scopeXPath = null)
            : base(scopeXPath)
        {
        }

        public string IdXPathFormat { get; set; }
    }
}
