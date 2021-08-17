namespace Atata
{
    /// <summary>
    /// Represents the behavior for control focusing by executing <c>HTMLElement.focus()</c> JavaScript.
    /// </summary>
    public class FocusesUsingScriptAttribute : FocusBehaviorAttribute
    {
        public override void Execute<TOwner>(IUIComponent<TOwner> component) =>
            component.Script.Focus();
    }
}
