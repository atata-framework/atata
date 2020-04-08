namespace Atata
{
    public class StrategyScopeLocatorExecutionUnit
    {
        public StrategyScopeLocatorExecutionUnit(
            object strategy,
            ComponentScopeLocateOptions scopeLocateOptions,
            SearchOptions searchOptions)
        {
            Strategy = strategy;
            ScopeLocateOptions = scopeLocateOptions;
            SearchOptions = searchOptions;
        }

        public object Strategy { get; }

        public ComponentScopeLocateOptions ScopeLocateOptions { get; }

        public SearchOptions SearchOptions { get; }
    }
}
