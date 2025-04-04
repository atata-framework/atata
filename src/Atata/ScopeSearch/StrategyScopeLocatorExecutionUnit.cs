#nullable enable

namespace Atata;

public class StrategyScopeLocatorExecutionUnit
{
    public StrategyScopeLocatorExecutionUnit(
        IComponentScopeFindStrategy strategy,
        ComponentScopeFindOptions scopeFindOptions,
        SearchOptions searchOptions)
    {
        Strategy = strategy;
        ScopeFindOptions = scopeFindOptions;
        SearchOptions = searchOptions;
    }

    public IComponentScopeFindStrategy Strategy { get; }

    public ComponentScopeFindOptions ScopeFindOptions { get; }

    public SearchOptions SearchOptions { get; }
}
