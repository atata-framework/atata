namespace Atata;

public class AttributeFilter<TAttribute>
{
    public AttributeLevels Levels { get; private set; } = AttributeLevels.All;

    public List<Func<TAttribute, bool>> Predicates { get; } = [];

    public Type TargetAttributeType { get; private set; }

    public AttributeFilter<TAttribute> Where(Func<TAttribute, bool> predicate)
    {
        if (predicate != null)
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
