using System;

namespace Atata
{
    /// <summary>
    /// Represents the provider of URI.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    public class UriProvider<TOwner> : DataProvider<Uri, TOwner>
        where TOwner : PageObject<TOwner>
    {
        public UriProvider(UIComponent<TOwner> component, Func<Uri> valueGetFunction, string providerName)
            : base(component, valueGetFunction, providerName)
        {
            Query = new UriQueryProvider<TOwner>(
                Component,
                () => Value.Query,
                "URI query parameters");
        }

        /// <summary>
        /// Gets the query parameters provider of the URI.
        /// </summary>
        public UriQueryProvider<TOwner> Query { get; }

        /// <summary>
        /// Gets the fragment provider of the URI.
        /// </summary>
        public DataProvider<string, TOwner> Fragment =>
            Component.GetOrCreateDataProvider("URI fragment", () => Value.Fragment);

        /// <summary>
        /// Gets the absolute path provider of the URL.
        /// </summary>
        public DataProvider<string, TOwner> AbsolutePath =>
            Component.GetOrCreateDataProvider("URI absolute path", () => Value.AbsolutePath);
    }
}
