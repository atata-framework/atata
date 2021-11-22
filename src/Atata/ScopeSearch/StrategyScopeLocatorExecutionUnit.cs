namespace Atata
{
    public class StrategyScopeLocatorExecutionUnit
    {
        public StrategyScopeLocatorExecutionUnit(
            IComponentScopeFindStrategy strategy,
            ComponentScopeLocateOptions scopeLocateOptions,
            SearchOptions searchOptions)
        {
            Strategy = strategy;
            ScopeLocateOptions = scopeLocateOptions;
            SearchOptions = searchOptions;
        }

        public IComponentScopeFindStrategy Strategy { get; }

        public ComponentScopeLocateOptions ScopeLocateOptions { get; }

        public SearchOptions SearchOptions { get; }
    }
}
