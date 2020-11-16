using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control double-clicking by <see cref="Actions.DoubleClick(IWebElement)"/> method.
    /// </summary>
    public class DoubleClickUsingDoubleClickActionAttribute : DoubleClickBehaviorAttribute
    {
        /// <inheritdoc/>
        public override void Execute<TOwner>(IUIComponent<TOwner> component)
        {
            var scopeElement = component.Scope;

            component.Owner.Driver.Perform(
                x => x.DoubleClick(scopeElement));
        }
    }
}
