namespace Atata;

/// <summary>
/// Represents the builder of assembly attributes.
/// </summary>
public sealed class AssemblyAttributesBuilder
    : AttributesBuilder<AssemblyAttributesBuilder>
{
    private readonly Assembly _assembly;

    private readonly AtataAttributesContext _attributesContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyAttributesBuilder"/> class.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    /// <param name="attributesContext">The building attributes context.</param>
    public AssemblyAttributesBuilder(Assembly assembly, AtataAttributesContext attributesContext)
    {
        _assembly = assembly;
        _attributesContext = attributesContext;
    }

    protected override void OnAdd(IEnumerable<Attribute> attributes)
    {
        if (!_attributesContext.AssemblyMap.TryGetValue(_assembly, out var attributeSet))
        {
            attributeSet = [];
            _attributesContext.AssemblyMap[_assembly] = attributeSet;
        }

        attributeSet.AddRange(attributes);
    }

    protected override AssemblyAttributesBuilder ResolveNextBuilder() => this;
}
