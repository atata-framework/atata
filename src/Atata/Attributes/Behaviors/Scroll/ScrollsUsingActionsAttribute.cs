using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for scrolling to control using WebDriver's <see cref="Actions"/>.
    /// Performs <see cref="Actions.MoveToElement(IWebElement)"/> action.
    /// </summary>
    public class ScrollsUsingActionsAttribute : ScrollBehaviorAttribute
    {
        public override void Execute<TOwner>(IControl<TOwner> control) =>
            control.Owner.Driver.Perform(a => a.MoveToElement(control.Scope));
    }
}
