namespace Atata
{
    /// <summary>
    /// Represents the base behavior class for scrolling to control.
    /// </summary>
    public abstract class ScrollBehaviorAttribute : MulticastAttribute
    {
        public abstract void Execute<TOwner>(IControl<TOwner> control)
            where TOwner : PageObject<TOwner>;
    }
}
