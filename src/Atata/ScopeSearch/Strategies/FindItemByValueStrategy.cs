using OpenQA.Selenium;

namespace Atata
{
    public class FindItemByValueStrategy : TermItemElementFindStrategy
    {
        public override string GetXPathCondition(object parameter, TermOptions termOptions)
        {
            return CreateSimpleXPathCodition("[{0}]", parameter, termOptions, "@value");
        }

        protected override string GetParameterAsString(IWebElement element)
        {
            return element.GetValue();
        }
    }
}
