﻿namespace Atata;

/// <summary>
/// A pair of type and property name.
/// Can be used to identify a property within a type.
/// </summary>
public readonly struct TypePropertyNamePair : IEquatable<TypePropertyNamePair>
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

    public static bool operator ==(TypePropertyNamePair left, TypePropertyNamePair right) =>
        left.Equals(right);

    public static bool operator !=(TypePropertyNamePair left, TypePropertyNamePair right) =>
        !(left == right);

    public override bool Equals(object obj) =>
        obj is TypePropertyNamePair other && Equals(other);

    public bool Equals(TypePropertyNamePair other) =>
        Type == other.Type && PropertyName == other.PropertyName;

    public override int GetHashCode()
    {
        int hashCode = 654856160;
        hashCode = (hashCode * -1521134295) + EqualityComparer<Type>.Default.GetHashCode(Type);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(PropertyName);
        return hashCode;
    }
}
