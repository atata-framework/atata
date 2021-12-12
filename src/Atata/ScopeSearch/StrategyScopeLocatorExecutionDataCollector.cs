using System;
using System.Linq;

namespace Atata
{
    public class StrategyScopeLocatorExecutionDataCollector : IStrategyScopeLocatorExecutionDataCollector
    {
        private readonly UIComponent _component;

        public StrategyScopeLocatorExecutionDataCollector(UIComponent component)
        {
            _component = component;
        }

        public StrategyScopeLocatorExecutionData Get(SearchOptions searchOptions)
        {
            searchOptions = searchOptions ?? new SearchOptions();

            FindAttribute[] layerFindAttributes = _component.Metadata.ResolveLayerFindAttributes().ToArray();

            FindAttribute findAttribute = _component.Metadata.ResolveFindAttribute();

            var layerExecutionUnits = layerFindAttributes
                .Select(x => CreateExecutionUnitForLayerFindAttribute(x, searchOptions))
                .ToArray();

            var finalExecutionUnit = CreateExecutionUnitForFinalFindAttribute(findAttribute, searchOptions);

            ScopeSource scopeSource = layerFindAttributes.Any() && layerFindAttributes[0].Properties.Contains(nameof(FindAttribute.ScopeSource))
                ? layerFindAttributes[0].ScopeSource
                : findAttribute.ScopeSource;

            PostProcessOuterXPath(layerExecutionUnits, finalExecutionUnit);

            return new StrategyScopeLocatorExecutionData(_component, scopeSource, searchOptions.IsSafely, layerExecutionUnits, finalExecutionUnit);
        }

        private static void PostProcessOuterXPath(StrategyScopeLocatorLayerExecutionUnit[] layerExecutionUnits, StrategyScopeLocatorExecutionUnit finalExecutionUnit)
        {
            for (int i = 0; i < layerExecutionUnits.Length; i++)
            {
                ComponentScopeFindOptions scopeFindOptions = i == layerExecutionUnits.Length - 1
                    ? finalExecutionUnit.ScopeFindOptions
                    : layerExecutionUnits[i + 1].ScopeFindOptions;

                scopeFindOptions.OuterXPath = scopeFindOptions.OuterXPath
                    ?? layerExecutionUnits[i].ScopeContextResolver.DefaultOuterXPath;
            }
        }

        private StrategyScopeLocatorExecutionUnit CreateExecutionUnitForFinalFindAttribute(FindAttribute findAttribute, SearchOptions desiredSearchOptions)
        {
            IComponentScopeFindStrategy strategy = findAttribute.CreateStrategy();

            SearchOptions searchOptions = desiredSearchOptions.Clone();

            if (!desiredSearchOptions.IsVisibilitySet)
                searchOptions.Visibility = findAttribute.Visibility;

            if (!desiredSearchOptions.IsTimeoutSet)
                searchOptions.Timeout = TimeSpan.FromSeconds(findAttribute.Timeout);

            if (!desiredSearchOptions.IsRetryIntervalSet)
                searchOptions.RetryInterval = TimeSpan.FromSeconds(findAttribute.RetryInterval);

            ComponentScopeFindOptions scopeFindOptions = ComponentScopeFindOptions.Create(_component, _component.Metadata, findAttribute);

            return new StrategyScopeLocatorExecutionUnit(strategy, scopeFindOptions, searchOptions);
        }

        private StrategyScopeLocatorLayerExecutionUnit CreateExecutionUnitForLayerFindAttribute(FindAttribute findAttribute, SearchOptions desiredSearchOptions)
        {
            IComponentScopeFindStrategy strategy = findAttribute.CreateStrategy();

            SearchOptions searchOptions = new SearchOptions
            {
                IsSafely = desiredSearchOptions.IsSafely,
                Visibility = findAttribute.Visibility,
                Timeout = TimeSpan.FromSeconds(findAttribute.Timeout),
                RetryInterval = TimeSpan.FromSeconds(findAttribute.RetryInterval)
            };

            ComponentScopeFindOptions scopeFindOptions = ComponentScopeFindOptions.Create(_component, findAttribute.Properties.Metadata, findAttribute);
            ILayerScopeContextResolver scopeContextResolver = LayerScopeContextResolverFactory.Create(findAttribute.As);

            return new StrategyScopeLocatorLayerExecutionUnit(strategy, scopeFindOptions, searchOptions, scopeContextResolver);
        }
    }
}
