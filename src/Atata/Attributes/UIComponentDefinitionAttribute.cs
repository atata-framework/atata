using System;
using System.Linq;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class UIComponentDefinitionAttribute : ScopeDefinitionAttribute, ISettingsAttribute
    {
        protected UIComponentDefinitionAttribute(string scopeXPath = null)
            : base(scopeXPath)
        {
        }

        public PropertyBag Properties { get; } = new PropertyBag();

        public string ComponentTypeName { get; set; }

        public string IgnoreNameEndings { get; set; }

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
