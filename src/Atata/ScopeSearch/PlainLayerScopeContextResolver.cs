using OpenQA.Selenium;

namespace Atata
{
    public class PlainLayerScopeContextResolver : ILayerScopeContextResolver
    {
        public ISearchContext Resolve(IWebElement element)
        {
            return element;
        }
    }
}
