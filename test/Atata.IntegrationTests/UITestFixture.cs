namespace Atata.IntegrationTests;

[TestFixture]
public abstract class UITestFixture : UITestFixtureBase
{
    protected virtual bool ReuseDriver => true;

    protected IWebDriver PreservedDriver { get; private set; }

    [SetUp]
    public void SetUp()
    {
        AtataContextBuilder contextBuilder = ConfigureBaseAtataContext();

        if (ReuseDriver)
        {
            contextBuilder = contextBuilder.UseDisposeDriver(false);

            if (PreservedDriver is not null)
                contextBuilder = contextBuilder.UseDriver(PreservedDriver);
        }

        contextBuilder.EventSubscriptions.Add<DriverInitEvent>(eventData =>
        {
            if (ReuseDriver && PreservedDriver is null)
                PreservedDriver = eventData.Driver;
        });

        contextBuilder.Build();

        OnSetUp();
    }

    protected virtual void OnSetUp()
    {
    }

    [OneTimeTearDown]
    public virtual void FixtureTearDown() =>
        CleanPreservedDriver();

    private void CleanPreservedDriver()
    {
        if (PreservedDriver != null)
        {
            PreservedDriver.Dispose();
            PreservedDriver = null;
        }
    }
}
