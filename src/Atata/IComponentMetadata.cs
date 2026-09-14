namespace Atata;

public interface IComponentMetadata
{
    /// <summary>
    /// Gets the name of the component.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// Gets the type of the component.
    /// </summary>
    Type ComponentType { get; }

    /// <summary>
    /// Gets the type of the parent component.
    /// </summary>
    Type? ParentComponentType { get; }

    /// <summary>
    /// Gets all attributes.
    /// </summary>
    IEnumerable<Attribute> AllAttributes { get; }

    /// <summary>
    /// Determines whether this instance contains the attribute of the specified type.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <returns><see langword="true"/> if contains; otherwise, <see langword="false"/>.</returns>
    bool Contains<TAttribute>()
        where TAttribute : notnull;

    /// <summary>
    /// Tries to get the first attribute of the specified type.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <param name="attribute">The attribute.</param>
    /// <returns><see langword="true"/> if attribute is found; otherwise, <see langword="false"/>.</returns>
    bool TryGet<TAttribute>([NotNullWhen(true)] out TAttribute? attribute)
        where TAttribute : notnull;

    /// <summary>
    /// Gets the first attribute of the specified type or <see langword="null"/> if no such attribute is found.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <returns>The first attribute found or <see langword="null"/>.</returns>
    TAttribute? Get<TAttribute>()
        where TAttribute : notnull;

    /// <summary>
    /// Gets the first attribute of the specified type or <see langword="null"/> if no such attribute is found.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <param name="filterConfiguration">The filter configuration function.</param>
    /// <returns>The first attribute found or <see langword="null"/>.</returns>
    TAttribute? Get<TAttribute>(Func<AttributeFilter<TAttribute>, AttributeFilter<TAttribute>>? filterConfiguration)
        where TAttribute : notnull;

    /// <summary>
    /// Gets a sequence of attributes of the specified type.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <returns>The sequence of attributes found.</returns>
    IEnumerable<TAttribute> GetAll<TAttribute>()
        where TAttribute : notnull;

    /// <summary>
    /// Gets a sequence of attributes of the specified type.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <param name="filterConfiguration">The filter configuration function.</param>
    /// <returns>The sequence of attributes found.</returns>
    IEnumerable<TAttribute> GetAll<TAttribute>(Func<AttributeFilter<TAttribute>, AttributeFilter<TAttribute>>? filterConfiguration)
        where TAttribute : notnull;
}
