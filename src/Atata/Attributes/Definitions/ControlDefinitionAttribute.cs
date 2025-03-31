#nullable enable

namespace Atata;

/// <summary>
/// Specifies the definition of the control, like scope XPath, visibility, component type name, etc.
/// </summary>
public class ControlDefinitionAttribute : UIComponentDefinitionAttribute, IHasOptionalProperties
{
    public ControlDefinitionAttribute(string scopeXPath = DefaultScopeXPath)
        : base(scopeXPath)
    {
    }

    PropertyBag IHasOptionalProperties.OptionalProperties => OptionalProperties;

    protected PropertyBag OptionalProperties { get; } = new();

    /// <summary>
    /// Gets or sets the visibility.
    /// The default value is <see cref="Visibility.Any"/>.
    /// </summary>
    public Visibility Visibility
    {
        get => OptionalProperties.GetOrDefault(nameof(Visibility), Visibility.Any);
        set => OptionalProperties[nameof(Visibility)] = value;
    }
}
