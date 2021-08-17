namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value getting from <c>value</c> attribute.
    /// </summary>
    public class GetsValueFromValueAttribute : ValueGetBehaviorAttribute
    {
        /// <inheritdoc/>
        public override string Execute<TOwner>(IUIComponent<TOwner> component) =>
            component.Attributes.Value;
    }
}
