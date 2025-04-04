#nullable enable

namespace Atata;

/// <summary>
/// Represents the base behavior class for control drag and drop to offset implementation.
/// Responsible for the <see cref="Control{TOwner}.DragAndDropToOffset(int, int)"/> method action.
/// </summary>
public abstract class DragAndDropToOffsetBehaviorAttribute : MulticastAttribute
{
    public abstract void Execute<TOwner>(IUIComponent<TOwner> component, Point offset)
        where TOwner : PageObject<TOwner>;
}
