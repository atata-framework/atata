namespace Atata.WebDriver;

public interface IStrategyScopeLocatorExecutor
{
    XPathComponentScopeFindResult[] Execute(StrategyScopeLocatorExecutionData executionData);
}
