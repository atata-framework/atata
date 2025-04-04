#nullable enable

namespace Atata;

public class StrategyScopeLocatorLayerExecutionUnit : StrategyScopeLocatorExecutionUnit
{
    public StrategyScopeLocatorLayerExecutionUnit(
        IComponentScopeFindStrategy strategy,
        ComponentScopeFindOptions scopeFindOptions,
        SearchOptions searchOptions,
        ILayerScopeContextResolver scopeContextResolver)
        : base(strategy, scopeFindOptions, searchOptions) =>
        ScopeContextResolver = scopeContextResolver;

    public ILayerScopeContextResolver ScopeContextResolver { get; }
}
