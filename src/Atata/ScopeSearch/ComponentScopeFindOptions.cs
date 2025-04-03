#nullable enable

namespace Atata;

/// <summary>
/// Represents options of UI component scope element finding.
/// </summary>
public sealed class ComponentScopeFindOptions : ICloneable
{
    public ComponentScopeFindOptions(UIComponent component, UIComponentMetadata metadata, FindAttribute findAttribute)
    {
        component.CheckNotNull(nameof(component));
        metadata.CheckNotNull(nameof(metadata));
        findAttribute.CheckNotNull(nameof(findAttribute));

        ControlDefinitionAttribute? definition = metadata.ComponentDefinitionAttribute as ControlDefinitionAttribute;

        int index = findAttribute.ResolveIndex(metadata);

        Component = component;
        Metadata = metadata;
        ElementXPath = definition?.ScopeXPath ?? ScopeDefinitionAttribute.DefaultScopeXPath;
        Index = index >= 0 ? index : null;
        OuterXPath = findAttribute.ResolveOuterXPath(metadata);

        Terms = findAttribute is ITermFindAttribute termFindAttribute
            ? termFindAttribute.GetTerms(metadata)
            : [];

        if (findAttribute is ITermMatchFindAttribute termMatchFindAttribute)
            Match = termMatchFindAttribute.GetTermMatch(metadata);
    }

    public UIComponent Component { get; }

    public UIComponentMetadata Metadata { get; }

    public string ElementXPath { get; }

    public string[] Terms { get; set; }

    public string? OuterXPath { get; set; }

    public int? Index { get; set; }

    public TermMatch Match { get; set; }

    public string? GetTermsAsString() =>
        Terms.Length > 0
            ? string.Join("/", Terms)
            : null;

    /// <inheritdoc cref="Clone"/>
    object ICloneable.Clone() =>
        Clone();

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>A new object that is a copy of this instance.</returns>
    public ComponentScopeFindOptions Clone() =>
        (ComponentScopeFindOptions)MemberwiseClone();
}
