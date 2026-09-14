namespace Atata.WebDriver;

public interface IStrategyScopeLocatorExecutionDataCollector
{
    StrategyScopeLocatorExecutionData Get(SearchOptions? searchOptions);
}
