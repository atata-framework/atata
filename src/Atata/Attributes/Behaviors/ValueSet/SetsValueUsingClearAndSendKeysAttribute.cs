using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value set by invoking
    /// <see cref="IWebElement.Clear()"/> and <see cref="IWebElement.SendKeys(string)"/> methods.
    /// <see cref="IWebElement.SendKeys(string)"/> method is invoked only when the value is not null or empty.
    /// </summary>
    public class SetsValueUsingClearAndSendKeysAttribute : ValueSetBehaviorAttribute
    {
        /// <inheritdoc/>
        public override void Execute<TOwner>(IUIComponent<TOwner> component, string value)
        {
            var scopeElement = component.Scope;

            scopeElement.ClearWithLogging();

            if (!string.IsNullOrEmpty(value))
                scopeElement.SendKeysWithLogging(value);
        }
    }
}
