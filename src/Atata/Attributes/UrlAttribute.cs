namespace Atata
{
    /// <summary>
    /// Specifies the URL to navigate to during initialization of page object.
    /// Applies to page object types.
    /// </summary>
    public class UrlAttribute : MulticastAttribute
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
