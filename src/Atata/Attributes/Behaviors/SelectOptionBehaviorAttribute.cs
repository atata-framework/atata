using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the base behavior class for option selection of <see cref="Select{T, TOwner}"/> control.
    /// </summary>
    public abstract class SelectOptionBehaviorAttribute : TermSettingsAttribute
    {
        protected SelectOptionBehaviorAttribute()
        {
        }

        protected SelectOptionBehaviorAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        protected SelectOptionBehaviorAttribute(TermMatch match)
            : base(match)
        {
        }

        protected SelectOptionBehaviorAttribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }

        public abstract string FormatOptionXPathCondition(string value);

        public abstract string GetOptionRawValue(IWebElement optionElement);
    }
}
