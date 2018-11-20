using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Specifies the content source of a component.
    /// </summary>
    public enum ContentSource
    {
        /// <summary>
        /// Uses Text property of component scope <see cref="IWebElement"/> element.
        /// </summary>
        Text,

        /// <summary>
        /// Uses <c>'textContent'</c> attribute of component scope <see cref="IWebElement"/> element.
        /// </summary>
        TextContent,

        /// <summary>
        /// Uses <c>innerHTML</c> attribute of component scope <see cref="IWebElement"/> element.
        /// </summary>
        InnerHtml,

        /// <summary>
        /// Uses <c>'value'</c> attribute of component scope <see cref="IWebElement"/> element.
        /// </summary>
        Value
    }
}
