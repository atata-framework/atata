#nullable enable

namespace Atata;

public class AttributeFilter<TAttribute>
{
    /// <summary>
    /// Gets the attribute levels to search at.
    /// The default value is <see cref="AttributeLevels.All"/>.
    /// </summary>
    public AttributeLevels Levels { get; private set; } = AttributeLevels.All;

    /// <summary>
    /// Gets the predicates.
    /// </summary>
    public List<Func<TAttribute, bool>> Predicates { get; } = [];

    /// <summary>
    /// Gets the type of the target attribute.
    /// The default value is <see langword="null"/>.
    /// </summary>
    public Type? TargetAttributeType { get; private set; }

    public AttributeFilter<TAttribute> Where(Func<TAttribute, bool> predicate)
    {
        if (predicate is not null)
            Predicates.Add(predicate);
        return this;
    }

    public AttributeFilter<TAttribute> ForAttribute<T>()
        where T : Attribute
        =>
        ForAttribute(typeof(T));

    public AttributeFilter<TAttribute> ForAttribute(Type type)
    {
        TargetAttributeType = type;
        return this;
    }

    public AttributeFilter<TAttribute> At(AttributeLevels levels)
    {
        Levels = levels;
        return this;
    }
}
