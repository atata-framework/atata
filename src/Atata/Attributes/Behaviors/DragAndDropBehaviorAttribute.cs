namespace Atata
{
    /// <summary>
    /// Represents the base behavior class for drag and drop.
    /// </summary>
    public abstract class DragAndDropBehaviorAttribute : MulticastAttribute
    {
        public abstract void Execute<TOwner>(IControl<TOwner> component, IControl<TOwner> target)
            where TOwner : PageObject<TOwner>, IPageObject<TOwner>;
    }
}
