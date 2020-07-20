using System;

namespace Atata
{
    /// <summary>
    /// Specifies the URL to navigate to during initialization of page object.
    /// Applies to page object types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UrlAttribute : Attribute
    {
        public UrlAttribute(string url)
        {
            Url = url;
        }

        /// <summary>
        /// Gets the URL to navigate to.
        /// </summary>
        public string Url { get; }
    }
}
