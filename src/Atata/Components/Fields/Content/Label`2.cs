namespace Atata
{
    /// <summary>
    /// Represents the label control (<c>&lt;label&gt;</c>).
    /// Default search is performed by the content.
    /// </summary>
    /// <typeparam name="T">The type of the content.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("label", ComponentTypeName = "label", IgnoreNameEndings = "Label")]
    [FindByContent]
    public class Label<T, TOwner> : Content<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the <c>for</c> attribute.
        /// </summary>
        public DataProvider<string, TOwner> For => Attributes.For;
    }
}
