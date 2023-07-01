namespace Atata;

/// <summary>
/// Represents the root builder of <see cref="AtataAttributesContext"/>.
/// </summary>
public class AttributesAtataContextBuilder : AtataContextBuilder
{
    /// <summary>
    /// The regex pattern for Atata assembly names.
    /// </summary>
    public const string AtataAssembliesNamePattern = @"^Atata($|\..+)";

    /// <summary>
    /// Initializes a new instance of the <see cref="AttributesAtataContextBuilder"/> class.
    /// </summary>
    /// <param name="buildingContext">The building context.</param>
    public AttributesAtataContextBuilder(AtataBuildingContext buildingContext)
        : base(buildingContext)
    {
    }

    /// <summary>
    /// Gets the attributes builder of global level.
    /// </summary>
    public GlobalAttributesAtataContextBuilder Global =>
        new(BuildingContext);

    /// <summary>
    /// Creates and returns the attributes builder for the assembly with the specified name.
    /// </summary>
    /// <param name="assemblyName">Name of the assembly.</param>
    /// <returns>An instance of <see cref="AssemblyAttributesAtataContextBuilder"/>.</returns>
    public AssemblyAttributesAtataContextBuilder Assembly(string assemblyName)
    {
        var assembly = AssemblyFinder.Find(assemblyName);
        return Assembly(assembly);
    }

    /// <summary>
    /// Creates and returns the attributes builder for the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    /// <returns>An instance of <see cref="AssemblyAttributesAtataContextBuilder"/>.</returns>
    public AssemblyAttributesAtataContextBuilder Assembly(Assembly assembly) =>
        new(assembly, BuildingContext);

    /// <summary>
    /// Creates and returns the attributes builder for the component specified by generic <typeparamref name="TComponent"/> parameter type.
    /// </summary>
    /// <typeparam name="TComponent">The type of the component.</typeparam>
    /// <returns>An instance of <see cref="ComponentAttributesAtataContextBuilder{TComponent}"/>.</returns>
    public ComponentAttributesAtataContextBuilder<TComponent> Component<TComponent>() =>
        new(BuildingContext);

    /// <summary>
    /// Creates and returns the attributes builder for the component with the specified type name.
    /// </summary>
    /// <param name="typeName">Name of the type.</param>
    /// <returns>An instance of <see cref="ComponentAttributesAtataContextBuilder"/>.</returns>
    public ComponentAttributesAtataContextBuilder Component(string typeName)
    {
        string assemblyNamePattern = BuildingContext.AssemblyNamePatternToFindComponentTypes
            ?? BuildingContext.DefaultAssemblyNamePatternToFindTypes;

        Assembly[] assemblies = AssemblyFinder.FindAllByPatterns(AtataAssembliesNamePattern, assemblyNamePattern);
        Type type = TypeFinder.FindInAssemblies(typeName, assemblies);

        return Component(type);
    }

    /// <summary>
    /// Creates and returns the attributes builder for the component of the specified type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>An instance of <see cref="ComponentAttributesAtataContextBuilder"/>.</returns>
    public ComponentAttributesAtataContextBuilder Component(Type type) =>
        new(type, BuildingContext);
}
