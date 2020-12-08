using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control clicking by <see cref="IWebElement.Click()"/> method.
    /// </summary>
    public class ClickUsingClickMethodAttribute : ClickBehaviorAttribute
    {
        /// <inheritdoc/>
        public override void Execute<TOwner>(IUIComponent<TOwner> component)
        {
            component.Scope.ClickWithLogging();
        }
    }
}
