namespace Atata;

/// <summary>
/// Represents the interface of the object source.
/// </summary>
/// <typeparam name="TObject">The type of the object.</typeparam>
public interface IObjectSource<out TObject>
{
    /// <summary>
    /// Gets the object value/instance.
    /// </summary>
    TObject Object { get; }

    /// <summary>
    /// Gets the name of the source provider.
    /// </summary>
    string? SourceProviderName { get; }

    /// <summary>
    /// Gets a value indicating whether the source is dynamic (value can vary for every value request).
    /// </summary>
    bool IsDynamic { get; }
}
