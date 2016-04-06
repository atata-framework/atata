namespace Atata
{
    public class ControlDefinitionAttribute : UIComponentDefinitionAttribute
    {
        public ControlDefinitionAttribute(string scopeXPath)
            : this(scopeXPath, null)
        {
        }

        public ControlDefinitionAttribute(string scopeXPath, string idFinderFormat)
            : base(scopeXPath)
        {
            IdFinderFormat = idFinderFormat;
        }

        public string IdFinderFormat { get; private set; }
    }
}
