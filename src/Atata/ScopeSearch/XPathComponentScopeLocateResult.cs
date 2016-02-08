namespace Atata
{
    public class XPathComponentScopeLocateResult : ComponentScopeLocateResult
    {
        public XPathComponentScopeLocateResult(string xPath)
        {
            XPath = xPath;
        }

        public string XPath { get; private set; }
    }
}
