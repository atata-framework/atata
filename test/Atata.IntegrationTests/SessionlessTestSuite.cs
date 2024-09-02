namespace Atata.IntegrationTests;

public abstract class SessionlessTestSuite : TestSuiteBase
{
    [SetUp]
    public void SetUp()
    {
        ConfigureSessionlessAtataContext()
            .Build();

        OnSetUp();
    }

    protected virtual void OnSetUp()
    {
    }
}
