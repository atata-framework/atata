using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class ScopeDefinitionAttribute : Attribute
    {
        private readonly string baseScopeXPath;

        protected ScopeDefinitionAttribute(string scopeXPath = null)
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

        public string ContainingClass { get; set; }
    }
}
