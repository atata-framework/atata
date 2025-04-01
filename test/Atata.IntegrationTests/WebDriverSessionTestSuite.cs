namespace Atata.IntegrationTests;

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

                    session.EventSubscriptions.Add<WebDriverInitCompletedEvent>(
                        eventData => PreservedDriver ??= eventData.Driver);
                }
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
        if (PreservedDriver is not null)
        {
            PreservedDriver.Dispose();
            PreservedDriver = null;
        }
    }
}
