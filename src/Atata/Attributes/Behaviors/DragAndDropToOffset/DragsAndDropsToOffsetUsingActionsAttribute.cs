using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control dragging and dropping to offset using WebDriver's <see cref="Actions"/>.
    /// Performs <see cref="Actions.DragAndDropToOffset(IWebElement, int, int)"/> action.
    /// </summary>
    public class DragsAndDropsToOffsetUsingActionsAttribute : DragAndDropToOffsetBehaviorAttribute
    {
        public override void Execute<TOwner>(IUIComponent<TOwner> component, Point offset) =>
            component.Owner.Driver.Perform(x => x.DragAndDropToOffset(component.Scope, offset.X, offset.Y));
    }
}
