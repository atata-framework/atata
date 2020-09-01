namespace Atata
{
    /// <summary>
    /// Provides the functionality to get the content of the component.
    /// </summary>
    public static class ContentExtractor
    {
        public const string GetTextContentOfChildTextNodesScript =
@"var element = arguments[0];
var text = '';
for (var i = 0; i < element.childNodes.length; i++) {
    var child = element.childNodes[i];
    if (child.nodeType == Node.TEXT_NODE)
        text += child.textContent;
}
return text.trim();";

        public const string GetTextContentOfChildTextNodesTrimmedScript =
@"var element = arguments[0];
var text = '';
for (var i = 0; i < element.childNodes.length; i++) {
    var child = element.childNodes[i];
    if (child.nodeType == Node.TEXT_NODE)
        text += child.textContent.trim();
}
return text;";

        public const string GetTextContentOfChildTextNodesTrimmedAndSpaceJoinedScript =
@"var element = arguments[0];
var text = '';
for (var i = 0; i < element.childNodes.length; i++) {
    var child = element.childNodes[i];
    if (child.nodeType == Node.TEXT_NODE) {
        if (text !== '')
            text += ' ';
        text += child.textContent.trim();
    }
}
return text;";

        public const string GetTextContentOfFirstChildTextNodeScript =
@"var element = arguments[0];
for (var i = 0; i < element.childNodes.length; i++) {
    var child = element.childNodes[i];
    if (child.nodeType == Node.TEXT_NODE)
        return child.textContent.trim();
}
return ''";

        public const string GetTextContentOfLastChildTextNodeScript =
@"var element = arguments[0];
for (var i = element.childNodes.length - 1; i >= 0; i--) {
    var child = element.childNodes[i];
    if (child.nodeType == Node.TEXT_NODE)
        return child.textContent.trim();
}
return ''";

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
                case ContentSource.ChildTextNodes:
                    return component.Script.ExecuteAgainst<string>(GetTextContentOfChildTextNodesScript);
                case ContentSource.ChildTextNodesTrimmed:
                    return component.Script.ExecuteAgainst<string>(GetTextContentOfChildTextNodesTrimmedScript);
                case ContentSource.ChildTextNodesTrimmedAndSpaceJoined:
                    return component.Script.ExecuteAgainst<string>(GetTextContentOfChildTextNodesTrimmedAndSpaceJoinedScript);
                case ContentSource.FirstChildTextNode:
                    return component.Script.ExecuteAgainst<string>(GetTextContentOfFirstChildTextNodeScript);
                case ContentSource.LastChildTextNode:
                    return component.Script.ExecuteAgainst<string>(GetTextContentOfLastChildTextNodeScript);
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(contentSource, nameof(contentSource));
            }
        }
    }
}
