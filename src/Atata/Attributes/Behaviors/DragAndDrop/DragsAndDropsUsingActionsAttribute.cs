using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for drag and drop using WebDriver's <see cref="Actions"/>.
    /// Performs <see cref="Actions.ClickAndHold(IWebElement)"/>, <see cref="Actions.MoveToElement(IWebElement)"/> and <see cref="Actions.Release(IWebElement)"/> actions.
    /// </summary>
    public class DragsAndDropsUsingActionsAttribute : DragAndDropBehaviorAttribute
    {
        public override void Execute<TOwner>(IControl<TOwner> component, IControl<TOwner> target)
        {
            AtataContext.Current.Driver.Perform(x => x.ClickAndHold(component.Scope));

            IWebElement targetScope = target.Scope;
            AtataContext.Current.Driver.Perform(x => x.MoveToElement(targetScope).Release(targetScope));
        }
    }
}
