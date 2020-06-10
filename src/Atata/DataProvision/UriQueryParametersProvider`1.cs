using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the provider of URL query parameters.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    public class UriQueryParametersProvider<TOwner> : DataProvider<IEnumerable<KeyValuePair<string, string>>, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private const string QueryParameterProviderNameFormat = "URI query \"{0}\" parameter value";

        private const string QueryParametersProviderNameFormat = "URI query \"{0}\" parameter values";

        /// <summary>
        /// Initializes a new instance of the <see cref="UriQueryParametersProvider{TOwner}"/> class.
        /// </summary>
        /// <param name="component">The associated component.</param>
        /// <param name="valueGetFunction">The function that gets the value.</param>
        /// <param name="providerName">Name of the provider.</param>
        public UriQueryParametersProvider(UIComponent<TOwner> component, Func<IEnumerable<KeyValuePair<string, string>>> valueGetFunction, string providerName)
            : base(component, valueGetFunction, providerName)
        {
        }

        /// <summary>
        /// Gets the count provider.
        /// </summary>
        public DataProvider<int, TOwner> Count =>
            Component.GetOrCreateDataProvider($"{ProviderName} count", GetCount);

        /// <summary>
        /// Gets the provider of the parameter value specified by name.
        /// </summary>
        /// <param name="parameterName">The name of the query parameter.</param>
        /// <returns>The provider of the parameter value.</returns>
        public DataProvider<string, TOwner> this[string parameterName] =>
            Get<string>(parameterName);

        /// <summary>
        /// Gets the provider of the parameter value specified by name.
        /// </summary>
        /// <typeparam name="TValue">The type of the query parameter value.</typeparam>
        /// <param name="parameterName">The name of the query parameter.</param>
        /// <returns>The provider of the parameter value.</returns>
        public DataProvider<TValue, TOwner> Get<TValue>(string parameterName)
        {
            return Component.GetOrCreateDataProvider(
                QueryParameterProviderNameFormat.FormatWith(parameterName),
                () => GetValue<TValue>(parameterName));
        }

        /// <summary>
        /// Gets the provider of the parameter values specified by name.
        /// </summary>
        /// <param name="queryParameterName">Name of the query parameter.</param>
        /// <returns>The provider of the parameter values.</returns>
        public DataProvider<IEnumerable<string>, TOwner> GetAll(string queryParameterName)
        {
            return GetAll<string>(queryParameterName);
        }

        /// <summary>
        /// Gets the provider of the parameter values specified by name.
        /// </summary>
        /// <typeparam name="TValue">The type of the query parameter value.</typeparam>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>The provider of the parameter values.</returns>
        public DataProvider<IEnumerable<TValue>, TOwner> GetAll<TValue>(string parameterName)
        {
            return Component.GetOrCreateDataProvider<IEnumerable<TValue>>(
                QueryParametersProviderNameFormat.FormatWith(parameterName),
                () => GetAllValues<TValue>(parameterName));
        }

        /// <summary>
        /// Gets the value of the specified query parameter.
        /// </summary>
        /// <param name="parameterName">The name of the query parameter.</param>
        /// <returns>The query parameter's value.
        /// Returns <see langword="null"/> if the value is not set.</returns>
        public string GetValue(string parameterName) =>
            GetValue<string>(parameterName);

        /// <summary>
        /// Gets the value of the specified query parameter.
        /// </summary>
        /// <typeparam name="TValue">The type of the query parameter value.</typeparam>
        /// <param name="parameterName">The name of the query parameter.</param>
        /// <returns>The parameter value.
        /// Returns <see langword="null"/> if the value is not set.</returns>
        public TValue GetValue<TValue>(string parameterName)
        {
            var parameter = Value.FirstOrDefault(x => x.Key == parameterName);

            return parameter.Key == null
                ? default(TValue)
                : Convert<TValue>(parameter.Value);
        }

        /// <summary>
        /// Gets all values of the specified query parameter.
        /// </summary>
        /// <typeparam name="TValue">The type of the query parameter value.</typeparam>
        /// <param name="parameterName">The name of the query parameter.</param>
        /// <returns>
        /// The enumerable of query parameter values.
        /// Returns <see langword="null"/> if the values are not set.
        /// </returns>
        public IEnumerable<TValue> GetAllValues<TValue>(string parameterName)
        {
            return Value
                .Where(x => x.Key == parameterName)
                .Select(x => Convert<TValue>(x.Value));
        }

        private TValue Convert<TValue>(string parameterValue)
        {
            return typeof(TValue) == typeof(string)
                ? (TValue)(object)(parameterValue ?? string.Empty)
                : TermResolver.FromString<TValue>(parameterValue);
        }

        private int GetCount() =>
            Value.Count();
    }
}
