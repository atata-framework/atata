namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value set by executing
    /// <c>HTMLElement.value += '{value}'; HTMLElement.dispatchEvent(new Event('change'));</c> JavaScript.
    /// </summary>
    public class TypesTextUsingScriptAttribute : TextTypeBehaviorAttribute
    {
        /// <inheritdoc/>
        public override void Execute<TOwner>(IUIComponent<TOwner> component, string value) =>
            component.Script.AddValueAndDispatchChangeEvent(value);
    }
}
