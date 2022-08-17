namespace Atata
{
    /// <summary>
    /// Represents the base behavior class for an implementation of control value clearing.
    /// Responsible for the <see cref="EditableTextField{TValue, TOwner}.Clear"/> method action.
    /// </summary>
    public abstract class ValueClearBehaviorAttribute : MulticastAttribute
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
