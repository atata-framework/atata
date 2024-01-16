namespace Atata;

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
    /// Gets the path provider of the URI.
    /// For example: <c>"https://example.org/some/path?arg=val#frg"</c> -> <c>"/some/path"</c>.
    /// </summary>
    public ValueProvider<string, TOwner> Path =>
        _component.CreateValueProvider("URI path", () => Value.AbsolutePath);

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
            () => Value.GetComponents(UriComponents.Path | UriComponents.Query | UriComponents.Fragment, UriFormat.UriEscaped));

    /// <summary>
    /// Gets the unescaped relative URI provider of the URI.
    /// For example: <c>"https://example.org/some/path?arg=%3F"</c> -> <c>"/some/path?arg=?"</c>.
    /// </summary>
    public ValueProvider<string, TOwner> RelativeUnescaped =>
        _component.CreateValueProvider(
            "unescaped relative URI",
            () => Value.GetComponents(UriComponents.Path | UriComponents.Query | UriComponents.Fragment, UriFormat.Unescaped));

    /// <summary>
    /// Gets the absolute URI provider of the URI.
    /// For example: <c>"https://example.org/some/path?arg=val#frg"</c> -> <c>"https://example.org/some/path?arg=val#frg"</c>.
    /// </summary>
    public ValueProvider<string, TOwner> Absolute =>
        _component.CreateValueProvider(
            "absolute URI",
            () => Value.AbsoluteUri);

    /// <summary>
    /// Gets the unescaped absolute URI provider of the URI.
    /// For example: <c>"https://example.org/some/path?arg=%3F"</c> -> <c>"https://example.org/some/path?arg=?"</c>.
    /// </summary>
    public ValueProvider<string, TOwner> AbsoluteUnescaped =>
        _component.CreateValueProvider(
            "unescaped absolute URI",
            () => Value.GetComponents(UriComponents.AbsoluteUri, UriFormat.Unescaped));

    /// <summary>
    /// Gets the specified components of the URI using the specified escaping format for special characters.
    /// </summary>
    /// <param name="components">The components.</param>
    /// <param name="format">The format.</param>
    /// <returns>A <see cref="ValueProvider{TValue, TOwner}"/> of the components string.</returns>
    public ValueProvider<string, TOwner> GetComponents(UriComponents components, UriFormat format = UriFormat.UriEscaped) =>
        _component.CreateValueProvider(
            $"URI {ConvertUriComponentsToString(components)}",
            () => Value.GetComponents(components, format));

    private static string ConvertUriComponentsToString(UriComponents components)
    {
        var flags = components.GetIndividualFlags().Select(x => x.ToString()).ToArray();

        StringBuilder builder = new StringBuilder()
            .Append('(')
            .AppendJoined(" | ", flags)
            .Append(") component");

        if (flags.Length > 1)
            builder.Append('s');

        return builder.ToString();
    }
}
