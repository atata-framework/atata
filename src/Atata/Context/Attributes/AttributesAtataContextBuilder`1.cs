namespace Atata;

/// <summary>
/// Represents the base class for attributes builders.
/// </summary>
/// <typeparam name="TNextBuilder">The type of the next builder to return by <c>Add</c> methods.</typeparam>
public abstract class AttributesAtataContextBuilder<TNextBuilder>
{
    /// <summary>
    /// Adds the specified attributes.
    /// </summary>
    /// <param name="attributes">The attributes.</param>
    /// <returns>An instance of <typeparamref name="TNextBuilder"/>.</returns>
    public TNextBuilder Add(params Attribute[] attributes) =>
        Add(attributes?.AsEnumerable());

    /// <summary>
    /// Adds the specified attributes.
    /// </summary>
    /// <param name="attributes">The attributes.</param>
    /// <returns>An instance of <typeparamref name="TNextBuilder"/>.</returns>
    public TNextBuilder Add(IEnumerable<Attribute> attributes)
    {
        if (attributes != null && attributes.Any())
            OnAdd(attributes);

        return ResolveNextBuilder();
    }

    protected abstract void OnAdd(IEnumerable<Attribute> attributes);

    protected abstract TNextBuilder ResolveNextBuilder();
}
