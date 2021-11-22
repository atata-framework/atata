namespace Atata
{
    public class StrategyScopeLocatorLayerExecutionUnit : StrategyScopeLocatorExecutionUnit
    {
        public StrategyScopeLocatorLayerExecutionUnit(
            IComponentScopeFindStrategy strategy,
            ComponentScopeLocateOptions scopeLocateOptions,
            SearchOptions searchOptions,
            ILayerScopeContextResolver scopeContextResolver)
            : base(strategy, scopeLocateOptions, searchOptions)
        {
            ScopeContextResolver = scopeContextResolver;
        }

        public ILayerScopeContextResolver ScopeContextResolver { get; }
    }
}
