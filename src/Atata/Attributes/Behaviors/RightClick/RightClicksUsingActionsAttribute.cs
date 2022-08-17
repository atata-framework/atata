using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control right-clicking by using a set of actions:
    /// <see cref="Actions.MoveToElement(IWebElement)"/> or
    /// <see cref="Actions.MoveToElement(IWebElement, int, int)"/> and then
    /// <see cref="Actions.ContextClick()"/>.
    /// </summary>
    public class RightClicksUsingActionsAttribute : RightClickBehaviorAttribute
    {
        /// <summary>
        /// Gets or sets the kind of the offset.
        /// The default value is <see cref="UIComponentOffsetKind.FromCenterInPercents"/>.
        /// </summary>
        public UIComponentOffsetKind OffsetKind { get; set; }

        /// <summary>
        /// Gets or sets the horizontal offset to which to move the mouse.
        /// </summary>
        public int OffsetX { get; set; }

        /// <summary>
        /// Gets or sets the vertical offset to which to move the mouse.
        /// </summary>
        public int OffsetY { get; set; }

        /// <inheritdoc/>
        public override void Execute<TOwner>(IUIComponent<TOwner> component)
        {
            var scopeElement = component.Scope;

            component.Owner.Driver.Perform(
                x => x.MoveToElement(scopeElement, OffsetX, OffsetY, OffsetKind).ContextClick());
        }
    }
}
