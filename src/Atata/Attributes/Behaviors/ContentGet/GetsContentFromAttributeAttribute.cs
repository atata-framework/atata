namespace Atata
{
    /// <summary>
    /// Represents the behavior for component content getting from HTML attribute by attribute name.
    /// </summary>
    public class GetsContentFromAttributeAttribute : ContentGetBehaviorAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetsContentFromAttributeAttribute"/> class using the name of HTML attribute.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        public GetsContentFromAttributeAttribute(string attributeName)
        {
            AttributeName = attributeName;
        }

        /// <summary>
        /// Gets the name of HTML attribute.
        /// </summary>
        public string AttributeName { get; }

        public override string Execute<TOwner>(IUIComponent<TOwner> component) =>
            component.Attributes[AttributeName];
    }
}
