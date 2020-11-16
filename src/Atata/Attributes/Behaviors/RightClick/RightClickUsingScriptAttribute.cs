namespace Atata
{
    /// <summary>
    /// Represents the behavior for control right-clicking by executing <c>HTMLElement.dispatchEvent(new Event('contextmenu'))</c> JavaScript.
    /// </summary>
    public class RightClickUsingScriptAttribute : RightClickBehaviorAttribute
    {
        /// <inheritdoc/>
        public override void Execute<TOwner>(IUIComponent<TOwner> component)
        {
            component.Script.DispatchEvent("contextmenu");
        }
    }
}
