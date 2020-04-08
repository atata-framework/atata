namespace Atata
{
    public class StrategyScopeLocatorLayerExecutionUnit : StrategyScopeLocatorExecutionUnit
    {
        public StrategyScopeLocatorLayerExecutionUnit(
            object strategy,
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
