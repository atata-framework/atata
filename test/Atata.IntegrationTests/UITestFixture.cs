namespace Atata.IntegrationTests;

[TestFixture]
public abstract class UITestFixture : UITestFixtureBase
{
    protected virtual bool ReuseDriver => true;

    protected IWebDriver PreservedDriver { get; private set; }

    protected virtual AtataContextDriverInitializationStage DriverInitializationStage =>
        AtataContextDriverInitializationStage.Build;

    [SetUp]
    public void SetUp()
    {
        AtataContextBuilder contextBuilder = ConfigureBaseAtataContext();

        if (ReuseDriver && PreservedDriver != null)
            contextBuilder = contextBuilder.UseDriver(PreservedDriver);

        contextBuilder.EventSubscriptions.Add<DriverInitEvent>(eventData =>
        {
            if (ReuseDriver && PreservedDriver is null)
                PreservedDriver = eventData.Driver;
        });

        contextBuilder.UseDriverInitializationStage(DriverInitializationStage);

        contextBuilder.Build();

        OnSetUp();
    }

    protected virtual void OnSetUp()
    {
    }

    public override void TearDown() =>
        AtataContext.Current?.CleanUp(!ReuseDriver);

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
