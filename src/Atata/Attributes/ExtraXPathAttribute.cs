using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface)]
    public abstract class ExtraXPathAttribute : Attribute
    {
        protected ExtraXPathAttribute(string xPath)
        {
            if (xPath != null)
            {
                XPath = xPath.StartsWith("/") ? xPath : "/" + xPath;
            }
        }

        public string XPath { get; private set; }
    }
}
