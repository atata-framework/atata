using OpenQA.Selenium;

namespace Atata
{
    public class FindItemByLabelStrategy : TermItemElementFindStrategy
    {
        private readonly UIComponent component;

        public FindItemByLabelStrategy(UIComponent component)
        {
            this.component = component;
        }

        public override string GetXPathCondition(object parameter, TermOptions termOptions)
        {
            ISearchContext scopeContext = component.ScopeSource.GetScopeContext(component, SearchOptions.SafelyAtOnce());

            IWebElement label = scopeContext.GetWithLogging(
                By.XPath($".//label[{TermResolver.CreateXPathCondition(parameter, termOptions)}]").
                    SafelyAtOnce().
                    Label(TermResolver.ToDisplayString(parameter)));

            if (label != null)
            {
                string elementId = label.GetAttribute("for");

                if (!string.IsNullOrEmpty(elementId))
                    return $"[@id='{elementId}']";
            }

            return $"[ancestor::label[{TermResolver.CreateXPathCondition(parameter, termOptions)}]]";
        }

        protected override string GetParameterAsString(IWebElement element)
        {
            string elementId = element.GetAttribute("id");

            if (!string.IsNullOrEmpty(elementId))
            {
                ISearchContext scopeContext = component.ScopeSource.GetScopeContext(component, SearchOptions.SafelyAtOnce());

                IWebElement label = scopeContext.GetWithLogging(
                    By.XPath($".//label[@for='{elementId}']").
                    SafelyAtOnce());

                if (label != null)
                    return label.Text;
            }

            return element.GetWithLogging(By.XPath("ancestor::label").AtOnce()).Text;
        }
    }
}
