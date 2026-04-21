namespace Atata;

public class StrategyScopeLocatorExecutionDataCollector : IStrategyScopeLocatorExecutionDataCollector
{
    private readonly UIComponent _component;

    public StrategyScopeLocatorExecutionDataCollector(UIComponent component) =>
        _component = component;

    public StrategyScopeLocatorExecutionData Get(SearchOptions searchOptions)
    {
        searchOptions ??= new SearchOptions();

        FindAttribute[] layerFindAttributes = _component.Metadata.ResolveLayerFindAttributes().ToArray();

        FindAttribute findAttribute = _component.Metadata.ResolveFindAttribute();

        var layerExecutionUnits = CreateExecutionUnitForLayerFindAttributes(layerFindAttributes, searchOptions);

        var finalExecutionUnit = CreateExecutionUnitForFinalFindAttribute(findAttribute, searchOptions);

        ScopeSource scopeSource = (layerFindAttributes.Any() && layerFindAttributes[0].OptionalProperties.Contains(nameof(FindAttribute.ScopeSource))
            ? layerFindAttributes[0]
            : findAttribute)
            .ResolveScopeSource(_component.Metadata);

        PostProcessOuterXPath(layerExecutionUnits, finalExecutionUnit);

        return new(_component, scopeSource, layerExecutionUnits, finalExecutionUnit);
    }

    private static void PostProcessOuterXPath(StrategyScopeLocatorLayerExecutionUnit[] layerExecutionUnits, StrategyScopeLocatorExecutionUnit finalExecutionUnit)
    {
        for (int i = 0; i < layerExecutionUnits.Length; i++)
        {
            ComponentScopeFindOptions scopeFindOptions = i == layerExecutionUnits.Length - 1
                ? finalExecutionUnit.ScopeFindOptions
                : layerExecutionUnits[i + 1].ScopeFindOptions;

            scopeFindOptions.OuterXPath ??= layerExecutionUnits[i].ScopeContextResolver.DefaultOuterXPath;
        }
    }

    private StrategyScopeLocatorExecutionUnit CreateExecutionUnitForFinalFindAttribute(
        FindAttribute findAttribute,
        SearchOptions desiredSearchOptions)
    {
        IComponentScopeFindStrategy strategy = findAttribute.CreateStrategy(_component.Metadata);

        SearchOptions searchOptions = desiredSearchOptions.Clone();

        if (!desiredSearchOptions.IsVisibilitySet)
            searchOptions.Visibility = findAttribute.ResolveVisibility(_component.Metadata);

        if (!desiredSearchOptions.IsTimeoutSet)
            searchOptions.Timeout = TimeSpan.FromSeconds(findAttribute.ResolveTimeout(_component.Metadata));

        if (!desiredSearchOptions.IsRetryIntervalSet)
            searchOptions.RetryInterval = TimeSpan.FromSeconds(findAttribute.ResolveRetryInterval(_component.Metadata));

        ComponentScopeFindOptions scopeFindOptions = ComponentScopeFindOptions.Create(_component, _component.Metadata, findAttribute);

        return new StrategyScopeLocatorExecutionUnit(strategy, scopeFindOptions, searchOptions);
    }

    private StrategyScopeLocatorLayerExecutionUnit[] CreateExecutionUnitForLayerFindAttributes(
        FindAttribute[] findAttributes,
        SearchOptions desiredSearchOptions)
    {
        if (findAttributes.Length > 0)
        {
            UIComponentMetadata metadata = _component.Metadata.CreateMetadataForLayerFindAttribute();

            return findAttributes
                .Select(attribute => CreateExecutionUnitForLayerFindAttribute(metadata, attribute, desiredSearchOptions))
                .ToArray();
        }
        else
        {
            return [];
        }
    }

    private StrategyScopeLocatorLayerExecutionUnit CreateExecutionUnitForLayerFindAttribute(
        UIComponentMetadata metadata,
        FindAttribute findAttribute,
        SearchOptions desiredSearchOptions)
    {
        IComponentScopeFindStrategy strategy = findAttribute.CreateStrategy(metadata);

        TimeSpan findAttributeTimeout = TimeSpan.FromSeconds(findAttribute.Timeout);
        TimeSpan resultTimeout = desiredSearchOptions.IsTimeoutSet && desiredSearchOptions.Timeout < findAttributeTimeout
            ? desiredSearchOptions.Timeout
            : findAttributeTimeout;

        TimeSpan findAttributeRetryInterval = TimeSpan.FromSeconds(findAttribute.RetryInterval);
        TimeSpan resultRetryInterval = desiredSearchOptions.IsRetryIntervalSet && desiredSearchOptions.RetryInterval < findAttributeRetryInterval
            ? desiredSearchOptions.RetryInterval
            : findAttributeRetryInterval;

        SearchOptions searchOptions = new()
        {
            IsSafely = desiredSearchOptions.IsSafely,
            Visibility = findAttribute.Visibility,
            Timeout = resultTimeout,
            RetryInterval = resultRetryInterval
        };

        ComponentScopeFindOptions scopeFindOptions = ComponentScopeFindOptions.Create(_component, metadata, findAttribute);
        ILayerScopeContextResolver scopeContextResolver = LayerScopeContextResolverFactory.Create(findAttribute.As);

        return new StrategyScopeLocatorLayerExecutionUnit(strategy, scopeFindOptions, searchOptions, scopeContextResolver);
    }
}
