#nullable enable

namespace Atata;

/// <summary>
/// Represents the base behavior class for control drag and drop implementation.
/// Responsible for the <see cref="Control{TOwner}.DragAndDropTo(Control{TOwner})"/> and
/// <see cref="Control{TOwner}.DragAndDropTo(System.Func{TOwner, Control{TOwner}})"/> methods action.
/// </summary>
public abstract class DragAndDropBehaviorAttribute : MulticastAttribute
{
    public abstract void Execute<TOwner>(IControl<TOwner> component, IControl<TOwner> target)
        where TOwner : PageObject<TOwner>;
}
