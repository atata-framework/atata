namespace Atata;

/// <summary>
/// Represents the root builder of <see cref="AtataAttributesContext"/>.
/// </summary>
public sealed class AttributesBuilder
{
    /// <summary>
    /// The regex pattern for Atata assembly names.
    /// </summary>
    public const string AtataAssembliesNamePattern = @"^Atata($|\..+)";

    /// <summary>
    /// Initializes a new instance of the <see cref="AttributesBuilder"/> class.
    /// </summary>
    /// <param name="attributesContext">The building attributes context.</param>
    public AttributesBuilder(AtataAttributesContext attributesContext) =>
        AttributesContext = attributesContext;

    internal AtataAttributesContext AttributesContext { get; }

    /// <summary>
    /// Gets the attributes builder of global level.
    /// </summary>
    public GlobalAttributesBuilder Global =>
        new(AttributesContext);

    /// <summary>
    /// Creates and returns the attributes builder for the assembly with the specified name.
    /// </summary>
    /// <param name="assemblyName">Name of the assembly.</param>
    /// <returns>An instance of <see cref="AssemblyAttributesBuilder"/>.</returns>
    public AssemblyAttributesBuilder Assembly(string assemblyName)
    {
        var assembly = AssemblyFinder.Find(assemblyName);
        return Assembly(assembly);
    }

    /// <summary>
    /// Creates and returns the attributes builder for the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    /// <returns>An instance of <see cref="AssemblyAttributesBuilder"/>.</returns>
    public AssemblyAttributesBuilder Assembly(Assembly assembly) =>
        new(assembly, AttributesContext);

    /// <summary>
    /// Creates and returns the attributes builder for the component specified by generic <typeparamref name="TComponent"/> parameter type.
    /// </summary>
    /// <typeparam name="TComponent">The type of the component.</typeparam>
    /// <returns>An instance of <see cref="ComponentAttributesBuilder{TComponent}"/>.</returns>
    public ComponentAttributesBuilder<TComponent> Component<TComponent>() =>
        new(AttributesContext);

    /// <summary>
    /// Creates and returns the attributes builder for the component with the specified type name.
    /// </summary>
    /// <param name="typeName">Name of the type.</param>
    /// <returns>An instance of <see cref="ComponentAttributesBuilder"/>.</returns>
    public ComponentAttributesBuilder Component(string typeName)
    {
        Assembly[] assemblies = AssemblyFinder.FindAllByPatterns(
            AtataAssembliesNamePattern,
            AtataContext.GlobalProperties.AssemblyNamePatternToFindTypes);
        Type type = TypeFinder.FindInAssemblies(typeName, assemblies);

        return Component(type);
    }

    /// <summary>
    /// Creates and returns the attributes builder for the component of the specified type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>An instance of <see cref="ComponentAttributesBuilder"/>.</returns>
    public ComponentAttributesBuilder Component(Type type) =>
        new(type, AttributesContext);
}
