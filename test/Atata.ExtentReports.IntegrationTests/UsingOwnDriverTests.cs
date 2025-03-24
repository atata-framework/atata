namespace Atata.ExtentReports.IntegrationTests;

public sealed class UsingOwnDriverTests : AtataTestSuite
{
    [Test]
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
