using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value clearing by performing "Ctrl+A, Delete" keyboard shortcut.
    /// </summary>
    public class ValueClearUsingCtrlADeleteKeysAttribute : ValueClearBehaviorAttribute
    {
        /// <inheritdoc/>
        public override void Execute<TOwner>(IUIComponent<TOwner> component)
        {
            var scopeElement = component.Scope;

            component.Owner.Driver.Perform(x => x
                .KeyDown(scopeElement, Keys.Control)
                .SendKeys("a")
                .KeyUp(Keys.Control)
                .SendKeys(Keys.Delete));
        }
    }
}
