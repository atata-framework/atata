namespace Atata.NUnit.IntegrationTests;

[Parallelizable(ParallelScope.Children)]
public sealed class ParallelTests : AtataTestSuite
{
    [Test]
    public void Context_Test([Values(1, 2, 3, 4, 5, 6, 7)] int number)
    {
        Thread.Sleep(1);
        Context.Test.Name.Should().Be($"{nameof(Context_Test)}({number})");
    }

    [Test]
    public async Task Context_Test_Async([Values(1, 2, 3, 4, 5, 6, 7)] int number)
    {
        await Task.Delay(1);
        Context.Test.Name.Should().Be($"{nameof(Context_Test_Async)}({number})");
    }
}
