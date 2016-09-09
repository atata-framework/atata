using System;
using System.Linq;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class UIComponentDefinitionAttribute : Attribute
    {
        private readonly string baseScopeXPath;

        protected UIComponentDefinitionAttribute(string scopeXPath = null)
        {
            baseScopeXPath = scopeXPath;
        }

        public string ScopeXPath
        {
            get
            {
                string scopeXPath = baseScopeXPath ?? "*";
                if (string.IsNullOrWhiteSpace(ContainingClass))
                    return scopeXPath;
                else
                    return string.Format("{0}[contains(concat(' ', normalize-space(@class), ' '), ' {1} ')]", scopeXPath, ContainingClass.Trim());
            }
        }

        public string ComponentTypeName { get; set; }

        public string IgnoreNameEndings { get; set; }

        public string ContainingClass { get; set; }

        public string[] GetIgnoreNameEndingValues()
        {
            return IgnoreNameEndings != null ? IgnoreNameEndings.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() : new string[0];
        }

        public string NormalizeNameIgnoringEnding(string name)
        {
            string endingToIgnore = GetIgnoreNameEndingValues().
                FirstOrDefault(x => name.EndsWith(x) && name.Length > x.Length);

            return endingToIgnore != null
                ? name.Substring(0, name.Length - endingToIgnore.Length).TrimEnd()
                : name;
        }
    }
}
