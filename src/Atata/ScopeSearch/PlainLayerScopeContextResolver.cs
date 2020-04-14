using OpenQA.Selenium;

namespace Atata
{
    public class PlainLayerScopeContextResolver : ILayerScopeContextResolver
    {
        public string DefaultOuterXPath => ".//";

        public ISearchContext Resolve(IWebElement element)
        {
            return element;
        }
    }
}
