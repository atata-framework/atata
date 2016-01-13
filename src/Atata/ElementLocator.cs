using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace Atata
{
    public class ElementLocator
    {
        public ElementLocator(IWebElement scope, string xPath)
        {
            Scope = scope;
            XPath = xPath;
        }

        public IWebElement Scope { get; private set; }
        public string XPath { get; set; }

        public IWebElement GetElement(bool isSafely, string extraXPath = null)
        {
            string xPath = string.IsNullOrWhiteSpace(extraXPath) ? XPath : XPath + extraXPath;
            return Scope.Get(By.XPath(xPath).Safely(isSafely));
        }

        public ReadOnlyCollection<IWebElement> GetElements()
        {
            return Scope.GetAll(By.XPath(XPath));
        }
    }
}
