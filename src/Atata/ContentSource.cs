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
        /// Uses 'textContent' attribute of component scope <see cref="IWebElement"/> element.
        /// </summary>
        TextContent,

        /// <summary>
        /// Uses 'innerHTML' attribute of component scope <see cref="IWebElement"/> element.
        /// </summary>
        InnerHtml,

        /// <summary>
        /// Uses 'value' attribute of component scope <see cref="IWebElement"/> element.
        /// </summary>
        Value
    }
}
