﻿namespace Atata.IntegrationTests.Context;

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
                        return FakeWebDriver.Create();
                    }
                })
                .Build();

            context.Driver.Should().NotBeNull();
            LogEntries.Where(x => x.Level == LogLevel.Warn).Should().ContainSingle();
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(4)]
        public void WhenAllAttemptsFail(int retries)
        {
            AssertThrowsWithInnerException<WebDriverInitializationException, InvalidOperationException>(() =>
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

    public sealed class WithInitialHealthCheck : UITestFixtureBase
    {
        [Test]
        public void WithFalse_WhenInitialHealthCheckFunctionIsSet()
        {
            var checkFunctionMock = new Mock<Func<IWebDriver, bool>>();

            var context = ConfigureBaseAtataContext()
                .UseFakeDriver()
                    .WithInitialHealthCheck(false)
                    .WithInitialHealthCheckFunction(checkFunctionMock.Object)
                .Build();

            context.Driver.Should().NotBeNull();
            checkFunctionMock.Verify(x => x(It.IsAny<IWebDriver>()), Times.Never);
        }

        [Test]
        public void WhenCheckReturnsTrue()
        {
            var context = ConfigureBaseAtataContext()
                .UseFakeDriver()
                    .WithInitialHealthCheck()
                    .WithInitialHealthCheckFunction(_ => true)
                .Build();

            context.Driver.Should().NotBeNull();
            LogEntries.Where(x => x.Level == LogLevel.Warn).Should().BeEmpty();
        }

        [Test]
        public void WhenFirstCheckThrowsException()
        {
            bool shouldThrow = true;

            ConfigureBaseAtataContext()
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
                    })
                .Build();

            var warning = LogEntries.Where(x => x.Level == LogLevel.Warn).Should().ContainSingle().Subject;
            warning.Message.Should().Contain("initial health check failed");
            warning.Exception.Should().BeOfType<InvalidOperationException>();
        }

        [Test]
        public void When2FirstChecksReturnFalse()
        {
            int countOfReturnedFalse = 0;

            ConfigureBaseAtataContext()
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
                    })
                .Build();

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
            AssertThrowsWithInnerException<WebDriverInitializationException, InvalidOperationException>(() =>
                ConfigureBaseAtataContext()
                    .UseFakeDriver()
                        .WithCreateRetries(retries)
                        .WithInitialHealthCheck()
                        .WithInitialHealthCheckFunction(_ => throw new InvalidOperationException("Fail."))
                    .Build());

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
