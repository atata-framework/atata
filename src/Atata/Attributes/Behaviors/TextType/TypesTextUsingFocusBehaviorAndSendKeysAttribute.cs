using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control text typing by executing <see cref="FocusBehaviorAttribute"/> behavior
    /// and then invoking <see cref="IWebElement.SendKeys(string)"/> method.
    /// </summary>
    public class TypesTextUsingFocusBehaviorAndSendKeysAttribute : TypesTextUsingSendKeysAttribute
    {
        /// <inheritdoc/>
        public override void Execute<TOwner>(IUIComponent<TOwner> component, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                AtataContext.Current.UIComponentScopeCache.ExecuteWithin(() =>
                {
                    component.ExecuteBehavior<FocusBehaviorAttribute>(x => x.Execute(component));

                    base.Execute(component, value);
                });
            }
        }
    }
}
