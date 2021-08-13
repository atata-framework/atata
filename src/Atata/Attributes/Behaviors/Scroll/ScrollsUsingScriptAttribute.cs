namespace Atata
{
    /// <summary>
    /// Represents the behavior for scrolling to control using JavaScript.
    /// Performs <c>element.scrollIntoView()</c> function.
    /// </summary>
    public class ScrollsUsingScriptAttribute : ScrollBehaviorAttribute
    {
        public override void Execute<TOwner>(IControl<TOwner> control) =>
            control.Script.ScrollIntoView();
    }
}
