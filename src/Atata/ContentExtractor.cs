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
        var childText = child.textContent.trim();
        if (childText !== '') {
            if (text !== '')
                text += ' ';
            text += childText;
        }
    }
}
return text;";

        public const string GetTextContentOfFirstChildTextNodeScript =
@"var element = arguments[0];
for (var i = 0; i < element.childNodes.length; i++) {
    var child = element.childNodes[i];
    if (child.nodeType == Node.TEXT_NODE) {
        var childText = child.textContent.trim();
        if (childText !== '')
            return childText;
    }
}
return '';";

        public const string GetTextContentOfLastChildTextNodeScript =
@"var element = arguments[0];
for (var i = element.childNodes.length - 1; i >= 0; i--) {
    var child = element.childNodes[i];
    if (child.nodeType == Node.TEXT_NODE) {
        var childText = child.textContent.trim();
        if (childText !== '')
            return childText;
    }
}
return '';";

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

            return contentSource switch
            {
                ContentSource.Text =>
                    component.Scope.Text,
                ContentSource.TextContent =>
                    component.DomProperties.TextContent,
                ContentSource.InnerHtml =>
                    component.DomProperties.InnerHtml,
                ContentSource.Value =>
                    component.DomProperties.Value,
                ContentSource.ChildTextNodes =>
                    component.Script.ExecuteAgainst<string>(GetTextContentOfChildTextNodesScript),
                ContentSource.ChildTextNodesTrimmed =>
                    component.Script.ExecuteAgainst<string>(GetTextContentOfChildTextNodesTrimmedScript),
                ContentSource.ChildTextNodesTrimmedAndSpaceJoined =>
                    component.Script.ExecuteAgainst<string>(GetTextContentOfChildTextNodesTrimmedAndSpaceJoinedScript),
                ContentSource.FirstChildTextNode =>
                    component.Script.ExecuteAgainst<string>(GetTextContentOfFirstChildTextNodeScript),
                ContentSource.LastChildTextNode =>
                    component.Script.ExecuteAgainst<string>(GetTextContentOfLastChildTextNodeScript),
                _ => throw ExceptionFactory.CreateForUnsupportedEnumValue(contentSource, nameof(contentSource))
            };
        }
    }
}
