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
        /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the <c>src</c> DOM property.
        /// </summary>
        public ValueProvider<string, TOwner> Source =>
            DomProperties["src"];

        /// <summary>
        /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the <c>src</c> DOM attribute.
        /// </summary>
        public ValueProvider<string, TOwner> SourceAttribue =>
            DomAttributes["src"];

        /// <summary>
        /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the <c>alt</c> DOM property.
        /// </summary>
        public ValueProvider<string, TOwner> Alt =>
            DomProperties["alt"];

        /// <summary>
        /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the value indicating whether the image file is loaded.
        /// </summary>
        public ValueProvider<bool, TOwner> IsLoaded =>
            CreateValueProvider("loaded state", GetIsLoaded);

        protected virtual bool GetIsLoaded() =>
            Script.ExecuteAgainst<bool>("return arguments[0].complete && (typeof arguments[0].naturalWidth) != 'undefined' && arguments[0].naturalWidth > 0;");
    }
}
