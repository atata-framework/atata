namespace Atata;

/// <summary>
/// Represents the interface of an object provider of <typeparamref name="TObject"/> type.
/// </summary>
/// <typeparam name="TObject">The type of the object.</typeparam>
public interface IObjectProvider<out TObject>
{
    /// <summary>
    /// Gets the object value/instance.
    /// </summary>
    TObject Object { get; }

    /// <summary>
    /// Gets the name of the provider.
    /// </summary>
    string ProviderName { get; }

    /// <summary>
    /// Gets the associated execution unit.
    /// </summary>
    IAtataExecutionUnit? ExecutionUnit { get; }
}
