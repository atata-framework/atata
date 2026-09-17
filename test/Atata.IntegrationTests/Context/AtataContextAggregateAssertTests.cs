namespace Atata.IntegrationTests.Context;

public sealed class AtataContextAggregateAssertTests : SessionlessTestSuite
{
    private StubComponent _sut;

    protected override void OnSetUp() =>
        _sut = new();

    [Test]
    public void NoFailure() =>
        Assert.DoesNotThrow(() =>
            CurrentContext.AggregateAssert(() =>
                _sut.IsTrue.Should.AtOnce.BeTrue()));

    [Test]
    public void OneFailure()
    {
        AggregateAssertionException exception = Assert.Throws<AggregateAssertionException>(() =>
            CurrentContext.AggregateAssert(() =>
                _sut.IsTrue.Should.AtOnce.BeFalse()))!;

        Assert.That(exception.Results, Has.Count.EqualTo(1));
        Assert.That(exception.Results[0].StackTrace, Does.Contain(nameof(OneFailure)));
        Assert.That(exception.Message, Does.StartWith("Failed with 1 assertion failure:"));
    }

    [Test]
    public void TwoFailures()
    {
        AggregateAssertionException exception = Assert.Throws<AggregateAssertionException>(() =>
            CurrentContext.AggregateAssert(() =>
            {
                _sut.IsTrue.Should.AtOnce.BeFalse();
                _sut.IsTrue.Should.AtOnce.BeTrue();
                _sut.IsTrue.Should.AtOnce.BeFalse();
            }))!;

        Assert.That(exception.Results, Has.Count.EqualTo(2));
        Assert.That(exception.Results.Select(x => x.StackTrace), Has.All.Contain(nameof(TwoFailures)));
        Assert.That(exception.Message, Does.StartWith("Failed with 2 assertion failures:"));
    }
}
