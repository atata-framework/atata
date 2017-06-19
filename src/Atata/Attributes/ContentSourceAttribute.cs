using System;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Specifies the content source of a component.
    /// </summary>
    public class ContentSourceAttribute : MulticastAttribute
    {
        public ContentSourceAttribute(ContentSource source)
        {
            GetContent = CreateGetContentDelegate(source);
        }

        public ContentSourceAttribute(string attributeName)
        {
            GetContent = element => element.GetAttribute(attributeName);
        }

        /// <summary>
        /// Gets the method that returns a content for specified <see cref="IWebElement"/> element.
        /// </summary>
        public Func<IWebElement, string> GetContent { get; }

        private Func<IWebElement, string> CreateGetContentDelegate(ContentSource source)
        {
            switch (source)
            {
                case ContentSource.Text:
                    return element => element.Text;
                case ContentSource.TextContent:
                    return element => element.GetAttribute("textContent").Trim();
                case ContentSource.InnerHtml:
                    return element => element.GetAttribute("innerHTML").Trim();
                case ContentSource.Value:
                    return element => element.GetAttribute("value");
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(source, nameof(source));
            }
        }
    }
}
