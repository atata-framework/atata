namespace Atata
{
    /// <summary>
    /// Represents the behavior for control clicking by executing <c>HTMLElement.dispatchEvent(new Event('dblclick'))</c> JavaScript.
    /// </summary>
    public class DoubleClickUsingScriptAttribute : DoubleClickBehaviorAttribute
    {
        /// <inheritdoc/>
        public override void Execute<TOwner>(IUIComponent<TOwner> component)
        {
            component.Script.DispatchEvent("dblclick");
        }
    }
}
