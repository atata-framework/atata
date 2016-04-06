using System;
using System.Linq;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class UIComponentDefinitionAttribute : Attribute
    {
        protected UIComponentDefinitionAttribute(string scopeXPath = null)
        {
            ScopeXPath = scopeXPath;
        }

        public string ScopeXPath { get; private set; }
        public string ComponentTypeName { get; set; }
        public string IgnoreNameEndings { get; set; }

        public string[] GetIgnoreNameEndingValues()
        {
            return IgnoreNameEndings != null ? IgnoreNameEndings.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() : new string[0];
        }
    }
}
