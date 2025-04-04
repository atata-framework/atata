namespace Atata;

/// <summary>
/// Specifies the tag(s) of the component.
/// The tags can be targeted in other attributes by <see cref="MulticastAttribute"/> properties.
/// </summary>
[AttributeUsage(
    AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface,
    AllowMultiple = true)]
public class TagAttribute : Attribute
{
    public TagAttribute(string value) =>
        Values = [value];

    public TagAttribute(params string[] values) =>
        Values = values ?? [];

    /// <summary>
    /// Gets the tag values.
    /// </summary>
    public IReadOnlyList<string> Values { get; }
}
