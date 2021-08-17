using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control hovering using WebDriver's <see cref="Actions"/>.
    /// Performs <see cref="Actions.MoveToElement(IWebElement)"/> action.
    /// </summary>
    public class HoversUsingActionsAttribute : HoverBehaviorAttribute
    {
        public override void Execute<TOwner>(IUIComponent<TOwner> component) =>
            component.Owner.Driver.Perform(a => a.MoveToElement(component.Scope));
    }
}
