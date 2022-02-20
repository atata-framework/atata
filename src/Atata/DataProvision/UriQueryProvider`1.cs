using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the provider of URI query.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    public class UriQueryProvider<TOwner> : ValueProvider<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UriQueryProvider{TOwner}"/> class.
        /// </summary>
        /// <param name="component">The associated component.</param>
        /// <param name="valueGetFunction">The function that gets the value.</param>
        /// <param name="providerName">Name of the provider.</param>
        public UriQueryProvider(UIComponent<TOwner> component, Func<string> valueGetFunction, string providerName)
            : base(component.Owner, DynamicObjectSource.Create(valueGetFunction), providerName)
        {
            Parameters = new UriQueryParametersProvider<TOwner>(
                component,
                GetQueryParameters,
                "URI query parameters");
        }

        /// <summary>
        /// Gets the query parameters provider of the URL.
        /// </summary>
        public UriQueryParametersProvider<TOwner> Parameters { get; }

        private static KeyValuePair<string, string> ExtractParameterKeyValue(string quertyParameterString)
        {
            string[] parts = quertyParameterString.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

            return new KeyValuePair<string, string>(parts[0], parts.Length > 1 ? parts[1] : null);
        }

        private IEnumerable<KeyValuePair<string, string>> GetQueryParameters()
        {
            var query = Value;

            if (query.Length == 0)
                return new List<KeyValuePair<string, string>>();

            return query.TrimStart('?')
                .Split(new[] { '&', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(ExtractParameterKeyValue);
        }
    }
}
