using Atata.IntegrationTests;

namespace Atata.UnitTests;

public static class RetryWaitTests
{
    public sealed class Until
    {
        [Test]
        public void WithDelegateThatReturnsFalse()
        {
            RetryWait sut = new(TimeSpan.FromMilliseconds(50), TimeSpan.FromMilliseconds(5));

            using (StopwatchAsserter.WithinMilliseconds(50, 30))
            {
                bool result = sut.Until(() => false);

                Assert.That(result, Is.False);
            }
        }
    }

    public sealed class UntilAsync
    {
        [Test]
        public async Task WithDelegateThatReturnsFalse()
        {
            RetryWait sut = new(TimeSpan.FromMilliseconds(50), TimeSpan.FromMilliseconds(5));

            using (StopwatchAsserter.WithinMilliseconds(50, 30))
            {
                bool result = await sut.UntilAsync(() => Task.FromResult(false));

                Assert.That(result, Is.False);
            }
        }
    }
}
