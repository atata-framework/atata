namespace Atata
{
    /// <summary>
    /// Represents the image control (<c>&lt;img&gt;</c>).
    /// Default search finds the first occurring <c>&lt;img&gt;</c> element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("img", ComponentTypeName = "image")]
    public class Image<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the <c>src</c> attribute.
        /// </summary>
        public DataProvider<string, TOwner> Source => Attributes.Src;

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the value indicating whether the image file is loaded.
        /// </summary>
        public DataProvider<bool, TOwner> IsLoaded => GetOrCreateDataProvider("loaded state", GetIsLoaded);

        protected virtual bool GetIsLoaded()
        {
            return Script.ExecuteAgainst<bool>("return arguments[0].complete && (typeof arguments[0].naturalWidth) != 'undefined' && arguments[0].naturalWidth > 0;");
        }
    }
}
