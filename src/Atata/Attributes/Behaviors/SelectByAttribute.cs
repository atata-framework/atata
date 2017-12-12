using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for option selection of <see cref="Select{T, TOwner}"/> control using specified option attribute.
    /// </summary>
    public class SelectByAttribute : SelectOptionBehaviorAttribute
    {
        public SelectByAttribute(string attributeName)
        {
            AttributeName = attributeName;
        }

        public SelectByAttribute(string attributeName, TermCase termCase)
            : base(termCase)
        {
            AttributeName = attributeName;
        }

        public SelectByAttribute(string attributeName, TermMatch match)
            : base(match)
        {
            AttributeName = attributeName;
        }

        public SelectByAttribute(string attributeName, TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
            AttributeName = attributeName;
        }

        /// <summary>
        /// Gets the name of the attribute.
        /// </summary>
        public string AttributeName { get; }

        public override string FormatOptionXPathCondition(string value)
        {
            return Match.CreateXPathCondition(value, "@" + AttributeName);
        }

        public override string GetOptionRawValue(IWebElement optionElement)
        {
            return optionElement.GetValue();
        }
    }
}
