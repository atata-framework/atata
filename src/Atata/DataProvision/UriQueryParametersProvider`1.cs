#nullable enable

namespace Atata;

/// <summary>
/// Represents the provider of URL query parameters.
/// </summary>
/// <typeparam name="TOwner">The type of the owner.</typeparam>
public class UriQueryParametersProvider<TOwner> : ValueProvider<IEnumerable<KeyValuePair<string, string>>, TOwner>
    where TOwner : PageObject<TOwner>
{
    private const string QueryParameterProviderNameFormat = "URI query \"{0}\" parameter value";

    private const string QueryParametersProviderNameFormat = "URI query \"{0}\" parameter values";

    private readonly UIComponent<TOwner> _component;

    /// <summary>
    /// Initializes a new instance of the <see cref="UriQueryParametersProvider{TOwner}"/> class.
    /// </summary>
    /// <param name="component">The associated component.</param>
    /// <param name="valueGetFunction">The function that gets the value.</param>
    /// <param name="providerName">Name of the provider.</param>
    public UriQueryParametersProvider(
        UIComponent<TOwner> component,
        Func<IEnumerable<KeyValuePair<string, string>>> valueGetFunction,
        string providerName)
        : base(
            component.Owner,
            DynamicObjectSource.Create(valueGetFunction),
            providerName,
            component.Session.ExecutionUnit) =>
        _component = component;

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the count.
    /// </summary>
    public ValueProvider<int, TOwner> Count =>
        _component.CreateValueProvider($"{ProviderName} count", GetCount);

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the parameter value specified by name.
    /// </summary>
    /// <param name="parameterName">The name of the query parameter.</param>
    /// <returns>The provider of the parameter value.</returns>
    public ValueProvider<string?, TOwner> this[string parameterName] =>
        Get<string>(parameterName);

    /// <inheritdoc cref="this[string]"/>
    /// <typeparam name="TValue">The type of the query parameter value.</typeparam>
    public ValueProvider<TValue?, TOwner> Get<TValue>(string parameterName) =>
        _component.CreateValueProvider(
            QueryParameterProviderNameFormat.FormatWith(parameterName),
            () => GetValue<TValue>(parameterName));

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the parameter values specified by name.
    /// </summary>
    /// <param name="parameterName">Name of the query parameter.</param>
    /// <returns>The provider of the parameter values.</returns>
    public ValueProvider<IEnumerable<string>, TOwner> GetAll(string parameterName) =>
        _component.CreateValueProvider(
            QueryParametersProviderNameFormat.FormatWith(parameterName),
            () => GetAllValues(parameterName));

    /// <inheritdoc cref="GetAll(string)"/>
    /// <typeparam name="TValue">The type of the query parameter value.</typeparam>
    public ValueProvider<IEnumerable<TValue?>, TOwner> GetAll<TValue>(string parameterName) =>
        _component.CreateValueProvider(
            QueryParametersProviderNameFormat.FormatWith(parameterName),
            () => GetAllValues<TValue>(parameterName));

    /// <summary>
    /// Gets the value of the specified query parameter.
    /// </summary>
    /// <param name="parameterName">The name of the query parameter.</param>
    /// <returns>The query parameter's value.
    /// Returns <see langword="null"/> if the value is not set.</returns>
    public string? GetValue(string parameterName) =>
        GetValue<string>(parameterName);

    /// <summary>
    /// Gets the value of the specified query parameter.
    /// </summary>
    /// <typeparam name="TValue">The type of the query parameter value.</typeparam>
    /// <param name="parameterName">The name of the query parameter.</param>
    /// <returns>The parameter value.
    /// Returns <see langword="null"/> or default value if the value is not set.</returns>
    public TValue? GetValue<TValue>(string parameterName) =>
        GetAllValues<TValue>(parameterName).FirstOrDefault();

    /// <summary>
    /// Gets all values of the specified query parameter.
    /// </summary>
    /// <param name="parameterName">The name of the query parameter.</param>
    /// <returns>An enumerable of query parameter values.</returns>
    public IEnumerable<string> GetAllValues(string parameterName)
    {
        foreach (var parameter in Value)
        {
            if (parameter.Key == parameterName)
                yield return TermResolver.FromString<string>(parameter.Value);
        }
    }

    /// <summary>
    /// Gets all values of the specified query parameter.
    /// </summary>
    /// <typeparam name="TValue">The type of the query parameter value.</typeparam>
    /// <param name="parameterName">The name of the query parameter.</param>
    /// <returns>An enumerable of query parameter values.</returns>
    public IEnumerable<TValue?> GetAllValues<TValue>(string parameterName)
    {
        foreach (var parameter in Value)
        {
            if (parameter.Key == parameterName)
                yield return TermResolver.FromString<TValue>(parameter.Value);
        }
    }

    private int GetCount() =>
        Value.Count();
}
