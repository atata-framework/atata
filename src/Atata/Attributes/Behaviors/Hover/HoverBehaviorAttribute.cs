namespace Atata
{
    /// <summary>
    /// Represents the base behavior class for control hover implementation.
    /// </summary>
    public abstract class HoverBehaviorAttribute : MulticastAttribute
    {
        /// <summary>
        /// Executes the behavior implementation.
        /// </summary>
        /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
        /// <param name="component">The UI component.</param>
        public abstract void Execute<TOwner>(IUIComponent<TOwner> component)
            where TOwner : PageObject<TOwner>;
    }
}
