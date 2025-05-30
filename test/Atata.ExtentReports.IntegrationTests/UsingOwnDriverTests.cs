namespace Atata.ExtentReports.IntegrationTests;

[Category("Some suite category")]
[Property("Some suite property", "abc")]
public sealed class UsingOwnDriverTests : AtataTestSuite
{
    [Test]
    [Category("Some test category")]
    [Property("Some test property", "ID1234")]
    public void Test1() =>
        Go.To<HomePage>()
            .Report.Screenshot()
            .Header.Should.Contain("Atata");

    [Test]
    public void Test2() =>
        Go.To<HomePage>()
            .Report.Screenshot()
            .AggregateAssert(x => x
                .PageTitle.Should.Contain("Atata")
                .Header.Should.Contain("Atata"));
}
