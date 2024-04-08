namespace Atata;

/// <summary>
/// Represents the behavior for drag and drop using WebDriver's <see cref="Actions"/>.
/// Performs <see cref="Actions.DragAndDrop(IWebElement, IWebElement)"/> action.
/// </summary>
public class DragsAndDropsUsingActionsAttribute : DragAndDropBehaviorAttribute
{
    public override void Execute<TOwner>(IControl<TOwner> component, IControl<TOwner> target) =>
        component.Session.Driver.Perform(x => x.DragAndDrop(component.Scope, target.Scope));
}
