using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value clearing by <see cref="IWebElement.Clear()"/> method.
    /// </summary>
    public class ClearsValueUsingClearMethodAttribute : ValueClearBehaviorAttribute
    {
        /// <inheritdoc/>
        public override void Execute<TOwner>(IUIComponent<TOwner> component) =>
            component.Scope.ClearWithLogging();
    }
}
