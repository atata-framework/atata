namespace Atata
{
    /// <summary>
    /// Represents the behavior for component content getting from
    /// the specified source of <see cref="ContentSource"/> enumeration type.
    /// </summary>
    public class GetsContentFromSourceAttribute : ContentGetBehaviorAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetsContentFromSourceAttribute"/> class using <see cref="ContentSource"/> value.
        /// </summary>
        /// <param name="source">The source.</param>
        public GetsContentFromSourceAttribute(ContentSource source) =>
            Source = source;

        /// <summary>
        /// Gets the kind of source.
        /// </summary>
        public ContentSource Source { get; }

        public override string Execute<TOwner>(IUIComponent<TOwner> component) =>
            ContentExtractor.Get(component, Source);
    }
}
