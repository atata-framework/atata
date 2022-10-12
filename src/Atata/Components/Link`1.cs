namespace Atata
{
    /// <summary>
    /// Represents the link control (<c>&lt;a&gt;</c>).
    /// Default search is performed by the content.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("a", ComponentTypeName = "link", IgnoreNameEndings = "Button,Link")]
    [FindByContent]
    public class Link<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the <c>href</c> DOM property.
        /// </summary>
        public ValueProvider<string, TOwner> Href =>
            DomProperties["href"];

        /// <summary>
        /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the <c>href</c> DOM attribute.
        /// </summary>
        public ValueProvider<string, TOwner> HrefAttribue =>
            DomAttributes["href"];
    }
}
