namespace Atata.IntegrationTests;

[TestFixture]
public abstract class WebDriverSessionTestSuite : WebDriverSessionTestSuiteBase
{
    protected virtual bool ReuseDriver => true;

    protected IWebDriver PreservedDriver { get; private set; }

    [SetUp]
    public void SetUp()
    {
        AtataContextBuilder contextBuilder = ConfigureAtataContextWithWebDriverSession(
            session =>
            {
                if (ReuseDriver)
                {
                    session.UseDisposeDriver(false);

                    if (PreservedDriver is not null)
                        session.UseDriver(PreservedDriver);
                }
            });

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
