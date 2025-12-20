namespace Atata;

/// <summary>
/// A pair of type and property name.
/// Can be used to identify a property within a type.
/// </summary>
public readonly record struct TypePropertyNamePair
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypePropertyNamePair"/> struct.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="propertyName">Name of the property.</param>
    public TypePropertyNamePair(Type type, string propertyName)
    {
        Type = type;
        PropertyName = propertyName;
    }

    /// <summary>
    /// Gets the type.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Gets the name of the property.
    /// </summary>
    public string PropertyName { get; }
}
