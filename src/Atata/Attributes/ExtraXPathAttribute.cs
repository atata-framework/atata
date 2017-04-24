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
                RawXPath = xPath;
                XPath = (xPath.StartsWith("/") || xPath.StartsWith("[")) ? xPath : "/" + xPath;
            }
        }

        /// <summary>
        /// Gets the raw XPath.
        /// </summary>
        public string RawXPath { get; private set; }

        /// <summary>
        /// Gets the XPath prepended with '/', if it can be applied.
        /// </summary>
        public string XPath { get; private set; }
    }
}
