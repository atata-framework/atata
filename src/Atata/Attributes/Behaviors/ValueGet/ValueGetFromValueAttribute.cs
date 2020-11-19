namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value getting from <c>value</c> attribute.
    /// </summary>
    public class ValueGetFromValueAttribute : ValueGetBehaviorAttribute
    {
        /// <inheritdoc/>
        public override string Execute<TOwner>(IUIComponent<TOwner> component)
        {
            return component.Attributes.Value;
        }
    }
}
