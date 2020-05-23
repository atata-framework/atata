using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    public class UrlProvider<TOwner> : DataProvider<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private readonly UIComponent<TOwner> component;

        public UrlProvider(UIComponent<TOwner> component, Func<string> valueGetFunction, string providerName)
            : base(component, valueGetFunction, providerName)
        {
            this.component = component;
        }

        /// <summary>
        /// Gets the query parameters provider of the current URL.
        /// </summary>
        public QueryParameterProvider<TOwner> QueryParameters => new QueryParameterProvider<TOwner>(component, GetQueryParameters, "query parameters");

        public DataProvider<string, TOwner> Fragment => component.GetOrCreateDataProvider("URL fragment value", GetFragment);

        public DataProvider<string, TOwner> AbsolutePath => component.GetOrCreateDataProvider("URL absolute path value", GetAbsolutePath);

        private IEnumerable<KeyValuePair<string, string>> GetQueryParameters()
        {
            var uri = GetUri();

            if (uri.Query.Length == 0)
                return new List<KeyValuePair<string, string>>();

            return uri.Query.TrimStart('?').
                        Split(new[] { '&', ';' }, StringSplitOptions.RemoveEmptyEntries).
                        Select(parameter => parameter.Split(new[] { '=' }, StringSplitOptions.None)).
                        Select(c => new KeyValuePair<string, string>(c[0], c[1]));
        }

        private string GetFragment() => GetUri().Fragment;

        private string GetAbsolutePath() => GetUri().AbsolutePath;

        private Uri GetUri() => new Uri(Value);
    }
}
