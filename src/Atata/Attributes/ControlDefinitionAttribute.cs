namespace Atata
{
    public class ControlDefinitionAttribute : UIComponentDefinitionAttribute
    {
        public ControlDefinitionAttribute(string scopeXPath = null, string idFinderFormat = null)
            : base(scopeXPath)
        {
            IdFinderFormat = idFinderFormat;
        }

        public string IdFinderFormat { get; set; }
    }
}
