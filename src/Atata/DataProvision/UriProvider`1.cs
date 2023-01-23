using System;

namespace Atata
{
    /// <summary>
    /// Represents the provider of URI.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    public class UriProvider<TOwner> : ValueProvider<Uri, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private readonly UIComponent<TOwner> _component;

        public UriProvider(UIComponent<TOwner> component, Func<Uri> valueGetFunction, string providerName)
            : base(component.Owner, DynamicObjectSource.Create(valueGetFunction), providerName)
        {
            _component = component;

            Query = new UriQueryProvider<TOwner>(
                component,
                () => Value.Query,
                "URI query");
        }

        /// <summary>
        /// Gets the query parameters provider of the URI.
        /// For example: <c>"https://example.org/some/path?arg=val#frg"</c> -> <c>"?arg=val"</c>.
        /// </summary>
        public UriQueryProvider<TOwner> Query { get; }

        /// <summary>
        /// Gets the fragment provider of the URI.
        /// For example: <c>"https://example.org/some/path?arg=val#frg"</c> -> <c>"#frg"</c>.
        /// </summary>
        public ValueProvider<string, TOwner> Fragment =>
            _component.CreateValueProvider("URI fragment", () => Value.Fragment);

        /// <summary>
        /// Gets the absolute path provider of the URI.
        /// For example: <c>"https://example.org/some/path?arg=val#frg"</c> -> <c>"/some/path"</c>.
        /// </summary>
        public ValueProvider<string, TOwner> AbsolutePath =>
            _component.CreateValueProvider("URI absolute path", () => Value.AbsolutePath);

        /// <summary>
        /// Gets the absolute path and query provider of the URI.
        /// For example: <c>"https://example.org/some/path?arg=val#frg"</c> -> <c>"/some/path?arg=val"</c>.
        /// </summary>
        public ValueProvider<string, TOwner> PathAndQuery =>
            _component.CreateValueProvider("URI path with query", () => Value.PathAndQuery);

        /// <summary>
        /// Gets the relative URI provider of the URI.
        /// For example: <c>"https://example.org/some/path?arg=val#frg"</c> -> <c>"/some/path?arg=val#frg"</c>.
        /// </summary>
        public ValueProvider<string, TOwner> Relative =>
            _component.CreateValueProvider(
                "relative URI",
                () => Value.GetComponents(UriComponents.Path | UriComponents.Query | UriComponents.Fragment, UriFormat.Unescaped));
    }
}
