using System;

namespace Atata
{
    /// <summary>
    /// Specifies the content source of a component.
    /// Source can be specified either using a value of <see cref="ContentSource"/> or HTML attribute name.
    /// </summary>
    [Obsolete("Use " + nameof(GetsContentFromSourceAttribute) + " or " + nameof(GetsContentFromAttributeAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class ContentSourceAttribute : ContentGetBehaviorAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentSourceAttribute"/> class using <see cref="ContentSource"/> value.
        /// </summary>
        /// <param name="source">The source.</param>
        public ContentSourceAttribute(ContentSource source)
        {
            Source = source;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentSourceAttribute"/> class using the name of HTML attribute.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        public ContentSourceAttribute(string attributeName)
        {
            AttributeName = attributeName;
        }

        /// <summary>
        /// Gets the kind of source.
        /// </summary>
        public ContentSource? Source { get; }

        /// <summary>
        /// Gets the name of HTML attribute.
        /// </summary>
        public string AttributeName { get; }

        public override string Execute<TOwner>(IUIComponent<TOwner> component)
        {
            return Source.HasValue
                ? ContentExtractor.Get(component, Source.Value)
                : !string.IsNullOrWhiteSpace(AttributeName)
                ? component.Attributes[AttributeName]
                : throw new InvalidOperationException($"Cannot execute as none of {Source} or {AttributeName} properties is set.");
        }
    }
}
