using OpenQA.Selenium.Chrome;

namespace Atata.IntegrationTests.Context;

public static class DriverAtataContextBuilderTests
{
    public sealed class WithCreateRetries : UITestFixtureBase
    {
        [Test]
        public void WhenFirstAttemptFails()
        {
            bool shouldThrow = true;

            var context = ConfigureBaseAtataContext()
                .UseDriver(() =>
                {
                    if (shouldThrow)
                    {
                        shouldThrow = false;
                        throw new InvalidOperationException("Fail.");
                    }
                    else
                    {
                        var options = new ChromeOptions();
                        options.AddArguments(ChromeArguments);
                        return new ChromeDriver(options);
                    }
                })
                .Build();

            context.Driver.Should().BeOfType<ChromeDriver>();
            LogEntries.Where(x => x.Level == LogLevel.Warn).Should().ContainSingle();
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(4)]
        public void WhenAllAttemptsFail(int retries)
        {
            Assert.Throws<InvalidOperationException>(() =>
                ConfigureBaseAtataContext()
                    .UseDriver(() => throw new InvalidOperationException("Fail."))
                        .WithCreateRetries(retries)
                    .Build());

            var warnings = LogEntries.Where(x => x.Level == LogLevel.Warn).ToArray();
            warnings.Should().HaveCount(retries);

            if (retries > 0)
            {
                warnings.Should().AllSatisfy(x =>
                {
                    x.Message.Should().StartWith("Failed to create driver");
                    x.Exception.Should().BeOfType<InvalidOperationException>();
                });
            }
        }
    }
}
