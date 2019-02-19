namespace Atata
{
    /// <summary>
    /// Provides the functionality to get the content of the component.
    /// </summary>
    public static class ContentExtractor
    {
        /// <summary>
        /// Gets the content of the component using <see cref="ContentSource"/> value.
        /// </summary>
        /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
        /// <param name="component">The component.</param>
        /// <param name="contentSource">The content source.</param>
        /// <returns>The content.</returns>
        public static string Get<TOwner>(IUIComponent<TOwner> component, ContentSource contentSource)
            where TOwner : PageObject<TOwner>, IPageObject<TOwner>
        {
            component.CheckNotNull(nameof(component));

            switch (contentSource)
            {
                case ContentSource.Text:
                    return component.Scope.Text;
                case ContentSource.TextContent:
                    return component.Attributes.TextContent;
                case ContentSource.InnerHtml:
                    return component.Attributes.InnerHtml;
                case ContentSource.Value:
                    return component.Attributes.Value;
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(contentSource, nameof(contentSource));
            }
        }
    }
}
