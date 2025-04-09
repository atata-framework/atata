namespace Atata.IntegrationTests.Verification;

public class ExpectToTests : WebDriverSessionTestSuite
{
    private StubPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<StubPage>();

    [Test]
    public void NoFailure()
    {
        var expectTo = _page.IsTrue.ExpectTo;

        expectTo.BeTrue();

        CurrentContext.Dispose();
    }

    [Test]
    public void NoFailure_WithRetry()
    {
        var expectTo = _page.IsTrueInASecond.ExpectTo;

        expectTo.BeTrue();

        CurrentContext.Dispose();
    }

    [Test]
    public void OneFailure()
    {
        var expectTo = _page.IsTrue.ExpectTo.AtOnce;

        expectTo.BeFalse();

        var exception = Assert.Throws<AggregateAssertionException>(
            CurrentContext.Dispose)!;

        Assert.That(exception.Results, Has.Count.EqualTo(1));
        Assert.That(exception.Results[0].StackTrace, Does.Contain(nameof(OneFailure)));
        Assert.That(exception.Message, Does.StartWith("Failed with 1 assertion failure:"));
    }

    [Test]
    public void TwoFailures()
    {
        var expectTo = _page.IsTrue.ExpectTo.AtOnce;

        expectTo.BeFalse();
        expectTo.Not.BeTrue();

        var exception = Assert.Throws<AggregateAssertionException>(
            CurrentContext.Dispose)!;

        Assert.That(exception.Results, Has.Count.EqualTo(2));
        Assert.That(exception.Results.Select(x => x.StackTrace), Has.All.Contain(nameof(TwoFailures)));
        Assert.That(exception.Message, Does.StartWith("Failed with 2 assertion failures:"));
    }
}
