using OpenQA.Selenium;

namespace Atata
{
    public class FindItemByValueStrategy : TermItemElementFindStrategy
    {
        public override string GetXPathCondition(object parameter, TermOptions termOptions) =>
            $"[{TermResolver.CreateXPathCondition(parameter, termOptions, "@value")}]";

        protected override string GetParameterAsString(IWebElement element) =>
            element.GetValue();
    }
}
