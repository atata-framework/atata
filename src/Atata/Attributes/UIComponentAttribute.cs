using System;
using System.Linq;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UIComponentAttribute : Attribute
    {
        public UIComponentAttribute(string elementXPath, string idFinderFormat = null)
        {
            ElementXPath = elementXPath;
            IdFinderFormat = idFinderFormat;
        }

        public string ElementXPath { get; private set; }
        public string IdFinderFormat { get; private set; }
        public string IgnoreNameEndings { get; set; }

        public string[] GetIgnoreNameEndingValues()
        {
            return IgnoreNameEndings != null ? IgnoreNameEndings.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() : new string[0];
        }
    }
}
