namespace Atata;

/// <summary>
/// Represents the behavior for scrolling to control using WebDriver's
/// <see cref="Actions.MoveToElement(IWebElement)"/> action.
/// </summary>
public class ScrollsUsingMoveToElementActionAttribute : ScrollBehaviorAttribute
{
    public override void Execute<TOwner>(IControl<TOwner> control) =>
        control.Owner.Driver.Perform(a => a.MoveToElement(control.Scope));
}
