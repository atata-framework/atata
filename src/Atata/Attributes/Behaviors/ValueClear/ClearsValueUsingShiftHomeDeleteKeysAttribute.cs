using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value clearing by performing "Shift+Home, Delete" keyboard shortcut.
    /// Note that "End" key is not pressed in the beginning of the shortcut, as the caret on element by default goes to the end.
    /// </summary>
    public class ClearsValueUsingShiftHomeDeleteKeysAttribute : ValueClearBehaviorAttribute
    {
        /// <inheritdoc/>
        public override void Execute<TOwner>(IUIComponent<TOwner> component)
        {
            var scopeElement = component.Scope;

            component.Owner.Driver.Perform(x => x
                .KeyDown(scopeElement, Keys.Shift)
                .SendKeys(Keys.Home)
                .KeyUp(Keys.Shift)
                .SendKeys(Keys.Delete));
        }
    }
}
