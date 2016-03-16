using OpenQA.Selenium;

namespace Atata
{
    public class FindItemByLabelStrategy : TermItemElementFindStrategy
    {
        public override string GetXPathCondition(object parameter, TermOptions termOptions)
        {
            return CreateSimpleXPathCodition("[ancestor::label[{0}]]", parameter, termOptions, ".");
        }

        protected override string GetParameterAsString(IWebElement element)
        {
            return element.Get(By.XPath("ancestor::label").Immediately()).Text;
        }
    }
}
