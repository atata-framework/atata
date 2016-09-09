using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace Atata
{
    internal class ExtendedBy : By
    {
        private readonly By by;

        internal ExtendedBy(By by)
        {
            this.by = (by as ExtendedBy)?.by ?? by;

            Description = this.by.ToString();
        }

        internal string ElementName { get; set; }

        internal string ElementKind { get; set; }

        internal SearchOptions Options { get; set; }

        public override IWebElement FindElement(ISearchContext context)
        {
            return by.FindElement(context);
        }

        public override ReadOnlyCollection<IWebElement> FindElements(ISearchContext context)
        {
            return by.FindElements(context);
        }

        public string GetElementNameWithKind()
        {
            bool hasName = !string.IsNullOrWhiteSpace(ElementName);
            bool hasKind = !string.IsNullOrWhiteSpace(ElementKind);

            if (hasName && hasKind)
                return $"\"{ElementName}\" {ElementKind}";
            else if (hasName)
                return ElementName;
            else if (hasKind)
                return ElementKind;
            else
                return null;
        }

        public override string ToString()
        {
            return by.ToString();
        }
    }
}
