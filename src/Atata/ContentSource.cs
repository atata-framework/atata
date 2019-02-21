using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Specifies the content source of a component.
    /// </summary>
    public enum ContentSource
    {
        /// <summary>
        /// Uses <see cref="IWebElement.Text"/> property of component scope <see cref="IWebElement"/> element.
        /// </summary>
        Text,

        /// <summary>
        /// Uses <c>textContent</c> attribute of component scope <see cref="IWebElement"/> element.
        /// </summary>
        TextContent,

        /// <summary>
        /// Uses <c>innerHTML</c> attribute of component scope <see cref="IWebElement"/> element.
        /// </summary>
        InnerHtml,

        /// <summary>
        /// Uses <c>value</c> attribute of component scope <see cref="IWebElement"/> element.
        /// </summary>
        Value,

        /// <summary>
        /// Executes the script that gets <c>childNodes</c> of component scope <see cref="IWebElement"/> element, filters only <c>Node.TEXT_NODE</c> and concatenates the <c>textContent</c> values of these nodes.
        /// Basically gets only child nested text.
        /// </summary>
        ChildTextNodes,

        /// <summary>
        /// Executes the script that gets <c>childNodes</c> of component scope <see cref="IWebElement"/> element, filters only <c>Node.TEXT_NODE</c>, gets <c>textContent</c> of each node, trims each value and concatenates them.
        /// Basically gets only child nested text trimming each part.
        /// </summary>
        ChildTextNodesTrimmed
    }
}
