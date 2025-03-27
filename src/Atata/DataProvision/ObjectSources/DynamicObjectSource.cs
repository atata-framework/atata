#nullable enable

namespace Atata;

/// <summary>
/// Provides a set of methods for <see cref="DynamicObjectSource{TObject}"/> creation.
/// </summary>
public static class DynamicObjectSource
{
    /// <summary>
    /// Creates a <see cref="DynamicObjectSource{TObject}"/> for the specified <paramref name="objectGetFunction"/>.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    /// <param name="objectGetFunction">The object get function.</param>
    /// <returns>An instance of <see cref="DynamicObjectSource{TValue}"/>.</returns>
    public static DynamicObjectSource<TObject> Create<TObject>(Func<TObject> objectGetFunction) =>
        new(objectGetFunction);
}
