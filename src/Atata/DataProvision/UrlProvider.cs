using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the provider of URL.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    public class UrlProvider<TOwner> : DataProvider<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
        public UrlProvider(UIComponent<TOwner> component, Func<string> valueGetFunction, string providerName)
            : base(component, valueGetFunction, providerName)
        {
            QueryParameters = new UrlQueryParametersProvider<TOwner>(
                Component,
                GetQueryParameters,
                "URL query parameters");
        }

        /// <summary>
        /// Gets the query parameters provider of the URL.
        /// </summary>
        public UrlQueryParametersProvider<TOwner> QueryParameters { get; }

        /// <summary>
        /// Gets the fragment provider of the URL.
        /// </summary>
        public DataProvider<string, TOwner> Fragment =>
            Component.GetOrCreateDataProvider("URL fragment", GetFragment);

        /// <summary>
        /// Gets the absolute path provider of the URL.
        /// </summary>
        public DataProvider<string, TOwner> AbsolutePath =>
            Component.GetOrCreateDataProvider("URL absolute path", GetAbsolutePath);

        private IEnumerable<KeyValuePair<string, string>> GetQueryParameters()
        {
            var uri = GetUri();

            if (uri.Query.Length == 0)
                return new List<KeyValuePair<string, string>>();

            return uri.Query.TrimStart('?')
                .Split(new[] { '&', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(parameter => parameter.Split(new[] { '=' }, StringSplitOptions.None))
                .Select(c => new KeyValuePair<string, string>(c[0], c[1]));
        }

        private string GetFragment() =>
            GetUri().Fragment;

        private string GetAbsolutePath() =>
            GetUri().AbsolutePath;

        private Uri GetUri() => new Uri(Value);
    }
}
