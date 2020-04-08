using System.Linq;

namespace Atata
{
    public class StrategyScopeLocatorExecutionDataCollector : IStrategyScopeLocatorExecutionDataCollector
    {
        private readonly UIComponent component;

        public StrategyScopeLocatorExecutionDataCollector(UIComponent component)
        {
            this.component = component;
        }

        public StrategyScopeLocatorExecutionData Get(SearchOptions searchOptions)
        {
            searchOptions = searchOptions ?? new SearchOptions();

            FindAttribute[] layerFindAttributes = component.Metadata.ResolveLayerFindAttributes().ToArray();

            FindAttribute findAttribute = component.Metadata.ResolveFindAttribute();

            var layerExecutionUnits = layerFindAttributes
                .Select(x => CreateExecutionUnitForLayerFindAttribute(x, searchOptions))
                .ToArray();

            var finalExecutionUnit = CreateExecutionUnitForFinalFindAttribute(findAttribute, searchOptions);

            ScopeSource scopeSource = layerFindAttributes.Any() && layerFindAttributes[0].Properties.Contains(nameof(FindAttribute.ScopeSource))
                ? layerFindAttributes[0].ScopeSource
                : findAttribute.ScopeSource;

            return new StrategyScopeLocatorExecutionData(component, scopeSource, searchOptions.IsSafely, layerExecutionUnits, finalExecutionUnit);
        }

        private StrategyScopeLocatorExecutionUnit CreateExecutionUnitForFinalFindAttribute(FindAttribute findAttribute, SearchOptions desiredSearchOptions)
        {
            object strategy = findAttribute.CreateStrategy();

            SearchOptions searchOptions = desiredSearchOptions.Clone();

            // TODO: Set Timeout and RetryInterval too.
            if (!desiredSearchOptions.IsVisibilitySet)
                searchOptions.Visibility = findAttribute.Visibility;

            ComponentScopeLocateOptions scopeLocateOptions = ComponentScopeLocateOptions.Create(component, component.Metadata, findAttribute);

            return new StrategyScopeLocatorExecutionUnit(strategy, scopeLocateOptions, searchOptions);
        }

        private StrategyScopeLocatorExecutionUnit CreateExecutionUnitForLayerFindAttribute(FindAttribute findAttribute, SearchOptions desiredSearchOptions)
        {
            object strategy = findAttribute.CreateStrategy();

            // TODO: Set Timeout and RetryInterval too.
            SearchOptions searchOptions = new SearchOptions
            {
                IsSafely = desiredSearchOptions.IsSafely,
                Visibility = findAttribute.Visibility
            };

            ComponentScopeLocateOptions scopeLocateOptions = ComponentScopeLocateOptions.Create(component, findAttribute.Properties.Metadata, findAttribute);

            return new StrategyScopeLocatorExecutionUnit(strategy, scopeLocateOptions, searchOptions);
        }
    }
}
