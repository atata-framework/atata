using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for scrolling to control using WebDriver's
    /// <see cref="Actions.ScrollToElement(IWebElement)"/> action.
    /// </summary>
    public class ScrollsUsingScrollToElementActionAttribute : ScrollBehaviorAttribute
    {
        public override void Execute<TOwner>(IControl<TOwner> control) =>
            control.Owner.Driver.Perform(a => a.ScrollToElement(control.Scope));
    }
}
