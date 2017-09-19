namespace Atata
{
    /// <summary>
    /// Represents the behavior for scrolling to control using JavaScript.
    /// Performs "element.scrollIntoView(true)" function.
    /// </summary>
    public class ScrollUsingScrollIntoViewAttribute : ScrollBehaviorAttribute
    {
        public override void Execute<TOwner>(IControl<TOwner> control)
        {
            control.Owner.Driver.ExecuteScript("arguments[0].scrollIntoView(true);", control.Scope);
        }
    }
}
