using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class)]
    public class IdXPathForLabelAttribute : Attribute
    {
        public IdXPathForLabelAttribute(string xPathFormat)
        {
            XPathFormat = xPathFormat;
        }

        public string XPathFormat { get; }
    }
}
