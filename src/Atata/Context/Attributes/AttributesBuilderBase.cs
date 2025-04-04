namespace Atata;

/// <summary>
/// A base class for attributes builders.
/// </summary>
public abstract class AttributesBuilderBase
{
    protected AttributesBuilderBase(AtataContextBuilder atataContextBuilder) =>
        AtataContextBuilder = atataContextBuilder;

    protected AtataContextBuilder AtataContextBuilder { get; }

    /// <inheritdoc cref="Add(IEnumerable{Attribute})"/>
    public AtataContextBuilder Add(params Attribute[] attributes) =>
        Add(attributes!.AsEnumerable());

    /// <summary>
    /// Adds the specified attributes.
    /// </summary>
    /// <param name="attributes">The attributes.</param>
    /// <returns>The <see cref="Atata.AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Add(IEnumerable<Attribute> attributes)
    {
        if (attributes is not null && attributes.Any())
            OnAdd(attributes);

        return AtataContextBuilder;
    }

    protected abstract void OnAdd(IEnumerable<Attribute> attributes);
}
