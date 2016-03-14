using OpenQA.Selenium;

namespace Atata
{
    public class FindItemByValueStrategy : TermItemElementFindStrategy
    {
        public FindItemByValueStrategy(ITermSettings termSettings)
            : base(termSettings)
        {
        }

        public override string GetXPathCondition(object parameter)
        {
            return CreateXPathCodition("[{0}]", parameter, "@value");
        }

        protected override string GetParameterAsString(IWebElement element)
        {
            return element.GetValue();
        }
    }
}
