namespace Atata.IntegrationTests.Verification;

public class WaitToTests : SessionlessTestSuite
{
    private StubComponent _sut;

    protected override void OnSetUp() =>
        _sut = new();

    [Test]
    public void NoFailure()
    {
        var waitTo = _sut.IsTrue.WaitTo;

        waitTo.BeTrue();
    }

    [Test]
    public void NoFailure_WithRetry()
    {
        var waitTo = _sut.IsTrueInASecond.WaitTo.WithinSeconds(5);

        waitTo.BeTrue();
    }

    [Test]
    public void Positive_Failure()
    {
        var waitTo = _sut.IsTrue.WaitTo.AtOnce;

        var exception = Assert.Throws<TimeoutException>(() =>
            waitTo.BeFalse())!;

        Assert.That(exception.Message, Does.StartWith("Timed out waiting for "));
        Assert.That(exception.InnerException, Is.Null);
    }

    [Test]
    public void Negative_Failure()
    {
        var waitTo = _sut.IsTrue.WaitTo.Not.AtOnce;

        var exception = Assert.Throws<TimeoutException>(() =>
            waitTo.BeTrue())!;

        Assert.That(exception.Message, Does.StartWith("Timed out waiting for "));
        Assert.That(exception.InnerException, Is.Null);
    }
}
