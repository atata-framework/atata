using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value clearing by trying to execute <see cref="IWebElement.Clear"/> method.
    /// If <see cref="InvalidElementStateException"/> occurs, then clears the value by executing
    /// <c>HTMLElement.value = ''; HTMLElement.dispatchEvent(new Event('change'));</c> JavaScript.
    /// </summary>
    public class ClearsValueUsingClearMethodOrScriptAttribute : ValueClearBehaviorAttribute
    {
        /// <inheritdoc/>
        public override void Execute<TOwner>(IUIComponent<TOwner> component)
        {
            try
            {
                component.Scope.ClearWithLogging();
            }
            catch (InvalidElementStateException)
            {
                component.Script.SetValueAndDispatchChangeEvent(string.Empty);
            }
        }
    }
}
