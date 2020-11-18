using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value clearing by performing "Home, Shift+End, Delete" keyboard shortcut.
    /// </summary>
    public class ValueClearUsingHomeShiftEndDeleteKeysAttribute : ValueClearBehaviorAttribute
    {
        /// <inheritdoc/>
        public override void Execute<TOwner>(IUIComponent<TOwner> component)
        {
            var scopeElement = component.Scope;

            component.Owner.Driver.Perform(x => x
                .SendKeys(scopeElement, Keys.Home)
                .KeyDown(Keys.Shift)
                .SendKeys(Keys.End)
                .KeyUp(Keys.Shift)
                .SendKeys(Keys.Delete));
        }
    }
}
