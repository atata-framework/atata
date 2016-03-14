using OpenQA.Selenium;

namespace Atata
{
    public class FindItemByLabelStrategy : TermItemElementFindStrategy
    {
        public FindItemByLabelStrategy(ITermSettings termSettings)
            : base(termSettings)
        {
        }

        public override string GetXPathCondition(object parameter)
        {
            return CreateXPathCodition("[ancestor::label[{0}]]", parameter, ".");
        }

        protected override string GetParameterAsString(IWebElement element)
        {
            return element.Get(By.XPath("ancestor::label").Immediately()).Text;
        }
    }
}
