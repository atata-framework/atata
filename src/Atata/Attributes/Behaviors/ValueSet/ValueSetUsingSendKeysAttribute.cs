using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value set by <see cref="IWebElement.SendKeys(string)"/> method.
    /// <see cref="IWebElement.SendKeys(string)"/> method is invoked only when the value is not null or empty.
    /// </summary>
    public class ValueSetUsingSendKeysAttribute : ValueSetBehaviorAttribute
    {
        /// <inheritdoc/>
        public override void Execute<TOwner>(IUIComponent<TOwner> component, string value)
        {
            if (!string.IsNullOrEmpty(value))
                component.Scope.SendKeysWithLogging(value);
        }
    }
}
