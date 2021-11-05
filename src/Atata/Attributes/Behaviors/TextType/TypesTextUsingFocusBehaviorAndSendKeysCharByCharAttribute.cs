using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control text typing by executing <see cref="FocusBehaviorAttribute"/> behavior
    /// and then invoking <see cref="IWebElement.SendKeys(string)"/> method
    /// for character by character with interval defined in <see cref="TypesTextUsingSendKeysCharByCharAttribute.TypingIntervalInSeconds"/> property.
    /// </summary>
    public class TypesTextUsingFocusBehaviorAndSendKeysCharByCharAttribute : TypesTextUsingSendKeysCharByCharAttribute
    {
        /// <inheritdoc/>
        public override void Execute<TOwner>(IUIComponent<TOwner> component, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                component.Context.UIComponentAccessChainScopeCache.ExecuteWithin(() =>
                {
                    component.ExecuteBehavior<FocusBehaviorAttribute>(x => x.Execute(component));

                    base.Execute(component, value);
                });
            }
        }
    }
}
