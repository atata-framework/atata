using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for option selection of <see cref="Select{T, TOwner}"/> control using option <c>text</c>.
    /// </summary>
    public class SelectByTextAttribute : SelectOptionBehaviorAttribute
    {
        public SelectByTextAttribute()
        {
        }

        public SelectByTextAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public SelectByTextAttribute(TermMatch match)
            : base(match)
        {
        }

        public SelectByTextAttribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }

        public override string FormatOptionXPathCondition(string value)
        {
            return Match.CreateXPathCondition(value);
        }

        public override string GetOptionRawValue(IWebElement optionElement)
        {
            return optionElement.Text;
        }
    }
}
