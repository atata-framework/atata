namespace Atata
{
    public interface IStrategyScopeLocatorExecutor
    {
        XPathComponentScopeFindResult[] Execute(StrategyScopeLocatorExecutionData executionData);
    }
}
