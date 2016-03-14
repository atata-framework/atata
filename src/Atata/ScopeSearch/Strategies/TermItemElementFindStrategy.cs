using Humanizer;
using OpenQA.Selenium;

namespace Atata
{
    public abstract class TermItemElementFindStrategy : IItemElementFindStrategy
    {
        protected TermItemElementFindStrategy(ITermSettings termSettings)
        {
            TermSettings = termSettings;
        }

        protected ITermSettings TermSettings { get; private set; }

        public abstract string GetXPathCondition(object parameter);

        public T GetParameter<T>(IWebElement element)
        {
            string value = GetParameterAsString(element);
            return TermResolver.FromString<T>(value, TermSettings);
        }

        protected abstract string GetParameterAsString(IWebElement element);

        protected string CreateXPathCodition(string locatorXPathFormat, object parameter, string operand)
        {
            string xPathCondition = TermResolver.CreateXPathCondition(parameter, TermSettings, operand);
            return locatorXPathFormat.FormatWith(xPathCondition);
        }

        public string ConvertToString(object parameter)
        {
            return TermResolver.ToString(parameter, TermSettings);
        }
    }
}
