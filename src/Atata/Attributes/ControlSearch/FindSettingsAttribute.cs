namespace Atata;

/// <summary>
/// Defines the finding settings to apply to the targeted control(s).
/// Adds to or overrides properties of <see cref="FindAttribute"/>.
/// </summary>
public class FindSettingsAttribute : AttributeSettingsAttribute, IHasOptionalProperties
{
    PropertyBag IHasOptionalProperties.OptionalProperties => OptionalProperties;

    protected PropertyBag OptionalProperties { get; } = new PropertyBag();

    /// <summary>
    /// Gets or sets the index of the control.
    /// The default value is <c>-1</c>, meaning that the index is not used.
    /// </summary>
    public int Index
    {
        get => OptionalProperties.GetOrDefault(nameof(Index), -1);
        set => OptionalProperties[nameof(Index)] = value;
    }

    /// <summary>
    /// Gets or sets the visibility.
    /// The default value is <see cref="Visibility.Any"/>.
    /// </summary>
    public Visibility Visibility
    {
        get => OptionalProperties.GetOrDefault(nameof(Visibility), Visibility.Any);
        set => OptionalProperties[nameof(Visibility)] = value;
    }

    /// <summary>
    /// Gets or sets the scope source.
    /// The default value is <see cref="ScopeSource.Parent"/>.
    /// </summary>
    public ScopeSource ScopeSource
    {
        get => OptionalProperties.GetOrDefault(nameof(ScopeSource), ScopeSource.Parent);
        set => OptionalProperties[nameof(ScopeSource)] = value;
    }

    /// <summary>
    /// Gets or sets the outer XPath.
    /// The default value is null, meaning that the control is searchable as descendant (using ".//" XPath) in scope source.
    /// </summary>
    public string OuterXPath
    {
        get => OptionalProperties.GetOrDefault<string>(nameof(OuterXPath));
        set => OptionalProperties[nameof(OuterXPath)] = value;
    }

    /// <summary>
    /// Gets or sets the strategy type for the control search.
    /// Strategy type should implement <see cref="IComponentScopeFindStrategy"/>.
    /// The default value is <see langword="null"/>, meaning that the default strategy of the specific <see cref="FindAttribute"/> should be used.
    /// </summary>
    public Type Strategy
    {
        get => OptionalProperties.GetOrDefault<Type>(nameof(Strategy));
        set => OptionalProperties[nameof(Strategy)] = value;
    }

    /// <summary>
    /// Gets or sets the element find timeout in seconds.
    /// The default value is taken from <see cref="AtataContext.ElementFindTimeout"/> property of <see cref="AtataContext.Current"/>.
    /// </summary>
    public double Timeout
    {
        get => OptionalProperties.GetOrDefault<double?>(nameof(Timeout))
            ?? (WebSession.Current?.ElementFindTimeout ?? RetrySettings.Timeout).TotalSeconds;
        set => OptionalProperties[nameof(Timeout)] = value;
    }

    /// <summary>
    /// Gets or sets the element find retry interval in seconds.
    /// The default value is taken from <see cref="AtataContext.ElementFindRetryInterval"/> property of <see cref="AtataContext.Current"/>.
    /// </summary>
    public double RetryInterval
    {
        get => OptionalProperties.GetOrDefault<double?>(nameof(RetryInterval))
            ?? (WebSession.Current?.ElementFindRetryInterval ?? RetrySettings.Interval).TotalSeconds;
        set => OptionalProperties[nameof(RetryInterval)] = value;
    }
}
