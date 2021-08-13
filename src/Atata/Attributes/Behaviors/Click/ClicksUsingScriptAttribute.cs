namespace Atata
{
    /// <summary>
    /// Represents the behavior for control clicking by executing <c>HTMLElement.click()</c> JavaScript method.
    /// </summary>
    public class ClicksUsingScriptAttribute : ClickBehaviorAttribute
    {
        /// <inheritdoc/>
        public override void Execute<TOwner>(IUIComponent<TOwner> component) =>
            component.Script.Click();
    }
}
