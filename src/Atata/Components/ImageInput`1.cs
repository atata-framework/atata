namespace Atata
{
    /// <summary>
    /// Represents the image input control (<c>&lt;input type="image"&gt;</c>).
    /// Default search is performed by <c>alt</c> attribute using <see cref="FindByAltAttribute"/>.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='image']", ComponentTypeName = "image input")]
    [ControlFinding(FindTermBy.Alt)]
    public class ImageInput<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the <c>src</c> attribute.
        /// </summary>
        public DataProvider<string, TOwner> Source => Attributes.Src;

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the <c>alt</c> attribute.
        /// </summary>
        public DataProvider<string, TOwner> Alt => Attributes.Alt;
    }
}
