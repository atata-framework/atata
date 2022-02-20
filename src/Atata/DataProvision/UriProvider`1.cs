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
                "URI query parameters");
        }

        /// <summary>
        /// Gets the query parameters provider of the URI.
        /// </summary>
        public UriQueryProvider<TOwner> Query { get; }

        /// <summary>
        /// Gets the fragment provider of the URI.
        /// </summary>
        public ValueProvider<string, TOwner> Fragment =>
            _component.CreateValueProvider("URI fragment", () => Value.Fragment);

        /// <summary>
        /// Gets the absolute path provider of the URL.
        /// </summary>
        public ValueProvider<string, TOwner> AbsolutePath =>
            _component.CreateValueProvider("URI absolute path", () => Value.AbsolutePath);
    }
}
