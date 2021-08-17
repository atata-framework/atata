namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value getting from <see cref="IUIComponent{TOwner}.Content"/> property.
    /// </summary>
    public class GetsValueFromContentAttribute : ValueGetBehaviorAttribute
    {
        /// <inheritdoc/>
        public override string Execute<TOwner>(IUIComponent<TOwner> component) =>
            component.Content;
    }
}
