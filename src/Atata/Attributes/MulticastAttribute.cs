namespace Atata;

/// <summary>
/// A base class for Atata attributes that can be applied to a component at any level
/// (declared, parent component, assembly, global and component).
/// </summary>
[AttributeUsage(
    AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Assembly,
    AllowMultiple = true)]
public abstract class MulticastAttribute : Attribute
{
    private bool? _targetSelf;

    /// <summary>
    /// Gets or sets the target component names.
    /// </summary>
    public string[]? TargetNames { get; set; }

    /// <summary>
    /// Gets or sets the target component name.
    /// </summary>
    public string? TargetName
    {
        get => TargetNames?.FirstOrDefault();
        set => TargetNames = value == null ? null : [value];
    }

    /// <summary>
    /// Gets or sets the target component types.
    /// </summary>
    public Type[]? TargetTypes { get; set; }

    /// <summary>
    /// Gets or sets the target component type.
    /// </summary>
    public Type? TargetType
    {
        get => TargetTypes?.FirstOrDefault();
        set => TargetTypes = value == null ? null : [value];
    }

    /// <summary>
    /// Gets or sets the target component tags.
    /// </summary>
    public string[]? TargetTags { get; set; }

    /// <summary>
    /// Gets or sets the target component tag.
    /// </summary>
    public string? TargetTag
    {
        get => TargetTags?.FirstOrDefault();
        set => TargetTags = value == null ? null : [value];
    }

    /// <summary>
    /// Gets or sets the target component's parent types.
    /// </summary>
    public Type[]? TargetParentTypes { get; set; }

    /// <summary>
    /// Gets or sets the target component's parent type.
    /// </summary>
    public Type? TargetParentType
    {
        get => TargetParentTypes?.FirstOrDefault();
        set => TargetParentTypes = value == null ? null : [value];
    }

    /// <summary>
    /// Gets or sets the target component names to exclude.
    /// </summary>
    public string[]? ExcludeTargetNames { get; set; }

    /// <summary>
    /// Gets or sets the target component name to exclude.
    /// </summary>
    public string? ExcludeTargetName
    {
        get => ExcludeTargetNames?.FirstOrDefault();
        set => ExcludeTargetNames = value == null ? null : [value];
    }

    /// <summary>
    /// Gets or sets the target component types to exclude.
    /// </summary>
    public Type[]? ExcludeTargetTypes { get; set; }

    /// <summary>
    /// Gets or sets the target component type to exclude.
    /// </summary>
    public Type? ExcludeTargetType
    {
        get => ExcludeTargetTypes?.FirstOrDefault();
        set => ExcludeTargetTypes = value == null ? null : [value];
    }

    /// <summary>
    /// Gets or sets the target component tags to exclude.
    /// </summary>
    public string[]? ExcludeTargetTags { get; set; }

    /// <summary>
    /// Gets or sets the target component tag to exclude.
    /// </summary>
    public string? ExcludeTargetTag
    {
        get => ExcludeTargetTags?.FirstOrDefault();
        set => ExcludeTargetTags = value == null ? null : [value];
    }

    /// <summary>
    /// Gets or sets the target component's parent types to exclude.
    /// </summary>
    public Type[]? ExcludeTargetParentTypes { get; set; }

    /// <summary>
    /// Gets or sets the target component's parent type to exclude.
    /// </summary>
    public Type? ExcludeTargetParentType
    {
        get => ExcludeTargetParentTypes?.FirstOrDefault();
        set => ExcludeTargetParentTypes = value == null ? null : [value];
    }

    /// <summary>
    /// Gets or sets a value indicating whether to target the component where this attribute is declared.
    /// </summary>
    public bool TargetSelf
    {
        get => _targetSelf ?? !IsTargetSpecified;
        set => _targetSelf = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether to target the component where this attribute is declared and its children.
    /// </summary>
    public bool TargetSelfAndChildren
    {
        get => TargetSelf && TargetChildren;
        set
        {
            TargetSelf = value;
            TargetChildren = value;
        }
    }

    /// <summary>
    /// Gets a value indicating whether this instance has any target specified.
    /// </summary>
    public virtual bool IsTargetSpecified =>
        TargetNames?.Length > 0 ||
        TargetTypes?.Length > 0 ||
        TargetTags?.Length > 0 ||
        TargetParentTypes?.Length > 0 ||
        ExcludeTargetNames?.Length > 0 ||
        ExcludeTargetTypes?.Length > 0 ||
        ExcludeTargetTags?.Length > 0 ||
        ExcludeTargetParentTypes?.Length > 0;

    /// <summary>
    /// Gets or sets a value indicating whether any type is targeted.
    /// When is set to <see langword="true"/>, sets <c>typeof(object)</c> to <see cref="TargetType"/> property.
    /// When is set to <see langword="false"/>, sets <see langword="null"/> to <see cref="TargetType"/> property.
    /// </summary>
    public bool TargetAnyType
    {
        get => TargetTypes?.Any(x => x == typeof(object)) ?? false;
        set => TargetTypes = value ? [typeof(object)] : null;
    }

    /// <summary>
    /// Gets or sets a value indicating whether all children are targeted.
    /// Actually, is a wrapper over the <see cref="TargetAnyType"/> property.
    /// </summary>
    public bool TargetAllChildren
    {
        get => TargetAnyType;
        set => TargetAnyType = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether children are targeted.
    /// </summary>
    public bool TargetChildren
    {
        get => IsTargetSpecified;
        set
        {
            if (value)
            {
                if (!IsTargetSpecified)
                    TargetAnyType = true;
            }
            else
            {
                TargetNames = null;
                TargetTypes = null;
                TargetTags = null;
                TargetParentTypes = null;
                ExcludeTargetNames = null;
                ExcludeTargetTypes = null;
                ExcludeTargetTags = null;
                ExcludeTargetParentTypes = null;
            }
        }
    }

    /// <summary>
    /// Determines whether the component name applies the name criteria.
    /// </summary>
    /// <param name="name">The component name.</param>
    /// <returns>
    /// <see langword="true"/> if the name applies the criteria; otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsNameApplicable(string? name) =>
        (TargetNames is null or [] || (name is not null && TargetNames.Contains(name)))
            && (ExcludeTargetNames is null || ExcludeTargetNames.Length == 0 || !ExcludeTargetNames.Contains(name));

    /// <summary>
    /// Determines whether the component tags apply the tag criteria.
    /// </summary>
    /// <param name="tags">The component tags.</param>
    /// <returns>
    /// <see langword="true"/> if the tags apply the criteria; otherwise, <see langword="false"/>.
    /// </returns>
    public bool AreTagsApplicable(IEnumerable<string> tags)
    {
        tags.CheckNotNull(nameof(tags));

        return (TargetTags is null or [] || TargetTags.Intersect(tags).Any())
            && (ExcludeTargetTags is null || ExcludeTargetTags.Length == 0 || !ExcludeTargetTags.Intersect(tags).Any());
    }

    /// <summary>
    /// Calculates the target rank.
    /// </summary>
    /// <param name="metadata">The metadata.</param>
    /// <returns>The rank.</returns>
    public virtual int? CalculateTargetRank(UIComponentMetadata metadata)
    {
        if (!IsNameApplicable(metadata.Name))
            return null;

        int? depthOfTypeInheritance = GetDepthOfInheritance(metadata.ComponentType, TargetTypes, ExcludeTargetTypes);
        if (depthOfTypeInheritance is null)
            return null;

        var tags = metadata.GetAll<TagAttribute>().SelectMany(x => x.Values).Distinct().ToArray();
        if (!AreTagsApplicable(tags))
            return null;

        int? depthOfParentTypeInheritance = metadata.ParentComponentType is not null
            ? GetDepthOfInheritance(metadata.ParentComponentType, TargetParentTypes, ExcludeTargetParentTypes)
            : TargetParentTypes?.Length > 0
                ? null
                : -1;

        if (depthOfParentTypeInheritance is null)
            return null;

        int rank = 0;
        int rankFactor = 100000;

        if (TargetNames?.Length > 0)
            rank += rankFactor;
        rankFactor /= 2;

        if (depthOfTypeInheritance >= 0)
            rank += Math.Max(rankFactor - (depthOfTypeInheritance.Value * 100), 0);
        rankFactor /= 2;

        if (TargetTags?.Length > 0)
            rank += rankFactor;
        rankFactor /= 2;

        if (depthOfParentTypeInheritance >= 0)
            rank += Math.Max(rankFactor - (depthOfParentTypeInheritance.Value * 100), 0);

        return rank;
    }

    protected static int? GetDepthOfInheritance(Type typeToCheck, Type[]? targetTypes, Type[]? excludeTargetTypes = null)
    {
        if (excludeTargetTypes?.Any(typeToCheck.IsInheritedFromOrIs) ?? false)
            return null;

        return targetTypes?.Length > 0
            ? targetTypes.Select(typeToCheck.GetDepthOfInheritance).Where(x => x is not null).Min()
            : -1;
    }
}
