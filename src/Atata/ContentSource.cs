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
        [Term("textContent")]
        TextContent,

        /// <summary>
        /// Uses <c>innerHTML</c> attribute of component scope <see cref="IWebElement"/> element.
        /// </summary>
        [Term("Inner HTML")]
        InnerHtml,

        /// <summary>
        /// Uses <c>value</c> attribute of component scope <see cref="IWebElement"/> element.
        /// </summary>
        Value,

        /// <summary>
        /// Uses the concatenation of child nested text values.
        /// Executes the script that gets <c>childNodes</c> of component scope <see cref="IWebElement"/> element,
        /// filters only <c>Node.TEXT_NODE</c> and concatenates the <c>textContent</c> values of these nodes.
        /// </summary>
        ChildTextNodes,

        /// <summary>
        /// Uses the concatenation of child nested text values trimming each text.
        /// Executes the script that gets <c>childNodes</c> of component scope <see cref="IWebElement"/> element,
        /// filters only <c>Node.TEXT_NODE</c>, gets <c>textContent</c> of each node, trims each value and concatenates them.
        /// </summary>
        ChildTextNodesTrimmed,

        /// <summary>
        /// Uses the concatenation of child nested text values trimming each text part and joining with <c>" "</c> separator.
        /// Executes the script that gets <c>childNodes</c> of component scope <see cref="IWebElement"/> element,
        /// filters only <c>Node.TEXT_NODE</c>, gets <c>textContent</c> of each node, trims each value and concatenates them delimiting with <c>" "</c> character.
        /// </summary>
        ChildTextNodesTrimmedAndSpaceJoined,

        /// <summary>
        /// Uses the first child nested text value.
        /// Executes the script that gets <c>childNodes</c> of component scope <see cref="IWebElement"/> element,
        /// finds the first <c>Node.TEXT_NODE</c> and returns the <c>textContent</c> value of this node.
        /// Returns an empty string if there are no text nodes.
        /// </summary>
        FirstChildTextNode,

        /// <summary>
        /// Uses the last child nested text value.
        /// Executes the script that gets <c>childNodes</c> of component scope <see cref="IWebElement"/> element,
        /// finds the last <c>Node.TEXT_NODE</c> and returns the <c>textContent</c> value of this node.
        /// Returns an empty string if there are no text nodes.
        /// </summary>
        LastChildTextNode
    }
}
