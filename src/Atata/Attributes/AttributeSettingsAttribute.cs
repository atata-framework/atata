namespace Atata;

/// <summary>
/// Represents the base attribute settings class for other attributes.
/// </summary>
public abstract class AttributeSettingsAttribute : MulticastAttribute
{
    /// <summary>
    /// Gets or sets the target attribute types.
    /// </summary>
    public Type[] TargetAttributeTypes { get; set; }

    /// <summary>
    /// Gets or sets the target attribute type.
    /// </summary>
    public Type TargetAttributeType
    {
        get => TargetAttributeTypes?.FirstOrDefault();
        set => TargetAttributeTypes = value == null ? null : new[] { value };
    }

    /// <summary>
    /// Gets or sets the target attribute types to exclude.
    /// </summary>
    public Type[] ExcludeTargetAttributeTypes { get; set; }

    /// <summary>
    /// Gets or sets the target attribute type to exclude.
    /// </summary>
    public Type ExcludeTargetAttributeType
    {
        get => ExcludeTargetAttributeTypes?.FirstOrDefault();
        set => ExcludeTargetAttributeTypes = value == null ? null : new[] { value };
    }

    public virtual int? CalculateTargetAttributeRank(Type targetAttributeType)
    {
        int? depthOfTypeInheritance = GetDepthOfInheritance(targetAttributeType, TargetAttributeTypes, ExcludeTargetAttributeTypes);
        if (depthOfTypeInheritance == null)
            return null;

        int rankFactor = 100000;

        return depthOfTypeInheritance >= 0
            ? Math.Max(rankFactor - (depthOfTypeInheritance.Value * 100), 0)
            : 0;
    }
}
