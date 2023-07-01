namespace Atata;

/// <summary>
/// Represents the interface of an object provider of <typeparamref name="TObject"/> type owned by <typeparamref name="TOwner"/> object.
/// </summary>
/// <typeparam name="TObject">The type of the object.</typeparam>
/// <typeparam name="TOwner">The type of the owner.</typeparam>
public interface IObjectProvider<out TObject, out TOwner> : IObjectProvider<TObject>
{
    /// <summary>
    /// Gets a value indicating whether the provided object is dynamic (can it vary for every value request).
    /// </summary>
    bool IsDynamic { get; }

    /// <summary>
    /// Gets the owner object.
    /// </summary>
    TOwner Owner { get; }
}
