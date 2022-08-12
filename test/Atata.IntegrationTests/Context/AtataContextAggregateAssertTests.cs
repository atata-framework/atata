namespace Atata.IntegrationTests.Context;

public class AtataContextAggregateAssertTests : UITestFixture
{
    private StubPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<StubPage>();

    [Test]
    public void NoFailure()
    {
        Assert.DoesNotThrow(() =>
            AtataContext.Current.AggregateAssert(() =>
                _page.IsTrue.Should.AtOnce.BeTrue()));
    }

    [Test]
    public void OneFailure()
    {
        AggregateAssertionException exception = Assert.Throws<AggregateAssertionException>(() =>
            AtataContext.Current.AggregateAssert(() =>
                _page.IsTrue.Should.AtOnce.BeFalse()));

        Assert.That(exception.Results, Has.Count.EqualTo(1));
        Assert.That(exception.Results[0].StackTrace, Does.Contain(nameof(OneFailure)));
        Assert.That(exception.Message, Does.StartWith("Failed with 1 assertion failure:"));
    }

    [Test]
    public void TwoFailures()
    {
        AggregateAssertionException exception = Assert.Throws<AggregateAssertionException>(() =>
            AtataContext.Current.AggregateAssert(() =>
            {
                _page.IsTrue.Should.AtOnce.BeFalse();
                _page.IsTrue.Should.AtOnce.BeTrue();
                _page.IsTrue.Should.AtOnce.BeFalse();
            }));

        Assert.That(exception.Results, Has.Count.EqualTo(2));
        Assert.That(exception.Results.Select(x => x.StackTrace), Has.All.Contain(nameof(TwoFailures)));
        Assert.That(exception.Message, Does.StartWith("Failed with 2 assertion failures:"));
    }
}
