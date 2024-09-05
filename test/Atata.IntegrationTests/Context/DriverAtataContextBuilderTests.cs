namespace Atata.IntegrationTests.Context;

public static class DriverAtataContextBuilderTests
{
    public sealed class WithCreateRetries : WebDriverSessionTestSuiteBase
    {
        [Test]
        public void WhenFirstAttemptFails()
        {
            bool shouldThrow = true;

            var context = BuildAtataContextWithWebDriverSession(x => x
                .UseDriver(() =>
                {
                    if (shouldThrow)
                    {
                        shouldThrow = false;
                        throw new InvalidOperationException("Fail.");
                    }
                    else
                    {
                        return FakeWebDriver.Create();
                    }
                }));

            context.GetWebDriverSession().Driver.Should().NotBeNull();
            LogEntries.Where(x => x.Level == LogLevel.Warn).Should().ContainSingle();
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(4)]
        public void WhenAllAttemptsFail(int retries)
        {
            AssertThrowsWithInnerException<WebDriverInitializationException, InvalidOperationException>(() =>
                BuildAtataContextWithWebDriverSession(x => x
                    .UseDriver(() => throw new InvalidOperationException("Fail."))
                        .WithCreateRetries(retries)));

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

    public sealed class WithInitialHealthCheck : WebDriverSessionTestSuiteBase
    {
        [Test]
        public void WithFalse_WhenInitialHealthCheckFunctionIsSet()
        {
            var checkFunctionMock = new Mock<Func<IWebDriver, bool>>();

            var context = BuildAtataContextWithWebDriverSession(x => x
                .UseFakeDriver()
                    .WithInitialHealthCheck(false)
                    .WithInitialHealthCheckFunction(checkFunctionMock.Object));

            context.GetWebDriverSession().Driver.Should().NotBeNull();
            checkFunctionMock.Verify(x => x(It.IsAny<IWebDriver>()), Times.Never);
        }

        [Test]
        public void WhenCheckReturnsTrue()
        {
            var context = BuildAtataContextWithWebDriverSession(x => x
                .UseFakeDriver()
                    .WithInitialHealthCheck()
                    .WithInitialHealthCheckFunction(_ => true));

            context.GetWebDriverSession().Driver.Should().NotBeNull();
            LogEntries.Where(x => x.Level == LogLevel.Warn).Should().BeEmpty();
        }

        [Test]
        public void WhenFirstCheckThrowsException()
        {
            bool shouldThrow = true;

            BuildAtataContextWithWebDriverSession(x => x
                .UseFakeDriver()
                    .WithInitialHealthCheck()
                    .WithInitialHealthCheckFunction(_ =>
                    {
                        if (shouldThrow)
                        {
                            shouldThrow = false;
                            throw new InvalidOperationException("Fail.");
                        }

                        return true;
                    }));

            var warning = LogEntries.Where(x => x.Level == LogLevel.Warn).Should().ContainSingle().Subject;
            warning.Message.Should().Contain("initial health check failed");
            warning.Exception.Should().BeOfType<InvalidOperationException>();
        }

        [Test]
        public void When2FirstChecksReturnFalse()
        {
            int countOfReturnedFalse = 0;

            BuildAtataContextWithWebDriverSession(x => x
                .UseFakeDriver()
                    .WithInitialHealthCheck()
                    .WithInitialHealthCheckFunction(_ =>
                    {
                        if (countOfReturnedFalse < 2)
                        {
                            countOfReturnedFalse++;
                            return false;
                        }

                        return true;
                    }));

            var warnings = LogEntries.Where(x => x.Level == LogLevel.Warn).ToArray();
            warnings.Should().HaveCount(2);
            warnings.Should().AllSatisfy(x =>
            {
                x.Message.Should().Contain("initial health check failed");
                x.Exception.Should().BeNull();
            });
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(4)]
        public void WhenAllChecksThrowException_WithCreateRetries(int retries)
        {
            var builder = ConfigureAtataContextWithWebDriverSession(x => x
                .UseFakeDriver()
                    .WithCreateRetries(retries)
                    .WithInitialHealthCheck()
                    .WithInitialHealthCheckFunction(_ => throw new InvalidOperationException("Fail.")));

            AssertThrowsWithInnerException<WebDriverInitializationException, InvalidOperationException>(() =>
                builder.Build());

            var warnings = LogEntries.Where(x => x.Level == LogLevel.Warn).ToArray();
            warnings.Should().HaveCount(retries);

            if (retries > 0)
            {
                warnings.Should().AllSatisfy(x =>
                {
                    x.Message.Should().Contain("initial health check failed");
                    x.Exception.Should().BeOfType<InvalidOperationException>();
                });
            }
        }
    }
}
