namespace Atata;

/// <summary>
/// An attributes context associated with <see cref="AtataContext"/>.
/// </summary>
public sealed class AtataAttributesContext : ICloneable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AtataAttributesContext"/> class.
    /// </summary>
    public AtataAttributesContext()
        : this([], [], [], [])
    {
    }

    private AtataAttributesContext(
        List<Attribute> global,
        Dictionary<Assembly, List<Attribute>> assemblyMap,
        Dictionary<Type, List<Attribute>> componentMap,
        Dictionary<TypePropertyNamePair, List<Attribute>> propertyMap)
    {
        Global = global;
        AssemblyMap = assemblyMap;
        ComponentMap = componentMap;
        PropertyMap = propertyMap;
    }

    /// <summary>
    /// Gets the list of global attributes.
    /// </summary>
    public List<Attribute> Global { get; }

    /// <summary>
    /// Gets the map of assembly attributes.
    /// </summary>
    public Dictionary<Assembly, List<Attribute>> AssemblyMap { get; }

    /// <summary>
    /// Gets the map of component attributes.
    /// </summary>
    public Dictionary<Type, List<Attribute>> ComponentMap { get; }

    /// <summary>
    /// Gets the map of component property attributes.
    /// </summary>
    public Dictionary<TypePropertyNamePair, List<Attribute>> PropertyMap { get; }

    /// <inheritdoc cref="Clone"/>
    object ICloneable.Clone() =>
        Clone();

    /// <summary>
    /// Creates a copy of the current instance.
    /// </summary>
    /// <returns>The copied <see cref="AtataAttributesContext"/> instance.</returns>
    public AtataAttributesContext Clone() =>
        new(
            new List<Attribute>(Global),
            AssemblyMap.ToDictionary(x => x.Key, x => x.Value.ToList()),
            ComponentMap.ToDictionary(x => x.Key, x => x.Value.ToList()),
            PropertyMap.ToDictionary(x => x.Key, x => x.Value.ToList()));
}
