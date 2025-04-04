namespace Atata;

/// <summary>
/// Represents the base behavior class for scrolling to control.
/// Responsible for the <see cref="Control{TOwner}.ScrollTo"/> method action.
/// </summary>
public abstract class ScrollBehaviorAttribute : MulticastAttribute
{
    public abstract void Execute<TOwner>(IControl<TOwner> control)
        where TOwner : PageObject<TOwner>;
}
