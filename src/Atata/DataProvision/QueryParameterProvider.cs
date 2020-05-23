using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atata
{
    public class QueryParameterProvider<TOwner> : DataProvider<IEnumerable<KeyValuePair<string, string>>, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private const string QueryParameterProviderNameFormat = "{0} query parameter value";
        private const string QueryParametersProviderNameFormat = "{0} query parameter values";
        private readonly UIComponent<TOwner> component;

        public QueryParameterProvider(UIComponent<TOwner> component, Func<IEnumerable<KeyValuePair<string, string>>> valueGetFunction, string providerName)
            : base(component, valueGetFunction, providerName)
        {
            this.component = component;
        }

        public DataProvider<int, TOwner> Count => component.GetOrCreateDataProvider("query parameters count", GetQueryParametersCount);

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> iinstance for the value of the specified query parameter from the current URL.
        /// </summary>
        /// <param name="queryParameterName">The name of the query parameter.</param>
        /// <returns>The <see cref="DataProvider{TData, TOwner}"/> instance for the query parameter's current value.</returns>
        public DataProvider<string, TOwner> this[string queryParameterName] => Get<string>(queryParameterName);

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the value of the specified query parameter from the current URL.
        /// </summary>
        /// <typeparam name="TValue">The type of the query parameter value.</typeparam>
        /// <param name="queryParameterName">The name of the query parameter.</param>
        /// <returns>The <see cref="DataProvider{TData, TOwner}"/> instance for the query parameter's current value.</returns>
        public DataProvider<TValue, TOwner> Get<TValue>(string queryParameterName)
        {
            string lowerCaseName = queryParameterName.ToLower();
            return component.GetOrCreateDataProvider(QueryParameterProviderNameFormat.FormatWith(lowerCaseName), () => GetValue<TValue>(lowerCaseName));
        }

        public DataProvider<IEnumerable<string>, TOwner> GetAll(string queryParameterName)
        {
            return GetAll<string>(queryParameterName);
        }

        public DataProvider<IEnumerable<TValue>, TOwner> GetAll<TValue>(string queryParameterName)
        {
            string lowerCaseName = queryParameterName.ToLower();
            return component.GetOrCreateDataProvider(QueryParametersProviderNameFormat.FormatWith(lowerCaseName), () => GetAllValues<TValue>(lowerCaseName));
        }

        /// <summary>
        /// Gets the value of the specified query parameter.
        /// </summary>
        /// <param name="queryParameterName">The name of the query parameter.</param>
        /// <returns>The query parameter's value.
        /// Returns <see langword="null"/> if the value is not set.</returns>
        public string GetValue(string queryParameterName)
        {
            var queryParameter = Value.FirstOrDefault(c => c.Key == queryParameterName);

            if (queryParameter.Key == null)
            {
                return null;
            }

            if (queryParameter.Value == null)
            {
                return string.Empty;
            }

            return queryParameter.Value;
        }

        /// <summary>
        /// Gets the value of the specified query parameter.
        /// </summary>
        /// <typeparam name="TValue">The type of the query parameter value.</typeparam>
        /// <param name="queryParameterName">The name of the query parameter.</param>
        /// <returns>The query parameter's value.
        /// Returns <see langword="null"/> if the value is not set.</returns>
        public TValue GetValue<TValue>(string queryParameterName)
        {
            var valueAsString = GetValue(queryParameterName);

            return TermResolver.FromString<TValue>(valueAsString, considerEmptyString: true);
        }

        /// <summary>
        /// Gets all values of the specified query parameter.
        /// </summary>
        /// <typeparam name="TValue">The type of the query parameter value.</typeparam>
        /// <param name="attributeName">The name of the query parameter.</param>
        /// <returns>The list of query parameter's values.
        /// Returns <see langword="null"/> if the values are not set.</returns>
        public IEnumerable<TValue> GetAllValues<TValue>(string attributeName)
        {
            var valuesAsString = Value.Where(c => c.Key == attributeName).Select(c => c.Value);
            var valuesAsTValue = new List<TValue>();

            foreach (var value in valuesAsString)
            {
                valuesAsTValue.Add(TermResolver.FromString<TValue>(value, considerEmptyString: true));
            }

            return valuesAsTValue;
        }

        private int GetQueryParametersCount() => Value.Count();
    }
}
