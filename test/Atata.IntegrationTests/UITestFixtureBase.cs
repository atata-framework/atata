using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Atata.IntegrationTests
{
    [TestFixture]
    public abstract class UITestFixtureBase
    {
        public const int TestAppPort = 50549;

        /// <summary>
        /// Usage of 2046 on Azure DevOps pipeline port often leads to failure during WebDriver instance creation.
        /// </summary>
        private readonly int[] _portsToIgnore = new[] { 2046 };

        private EventListLogConsumer _eventListLogConsumer;

        public static string BaseUrl { get; set; } = $"http://localhost:{TestAppPort}";

        protected IEnumerable<LogEventInfo> LogEntries => _eventListLogConsumer.Items;

        protected AtataContextBuilder ConfigureBaseAtataContext()
        {
            _eventListLogConsumer = new EventListLogConsumer();

            return AtataContext.Configure()
                .UseChrome()
                    .WithArguments(GetChromeArguments())
                    .WithPortsToIgnore(_portsToIgnore)
                .UseBaseUrl(BaseUrl)
                .UseCulture("en-US")
                .UseNUnitTestName()
                .UseNUnitTestSuiteName()
                .UseNUnitTestSuiteType()
                .LogConsumers.AddNUnitTestContext()
                .LogConsumers.Add(_eventListLogConsumer)
                    .WithMessageNestingLevelIndent(string.Empty)
                .LogNUnitError()
                .OnCleanUpAddArtifactsToNUnitTestContext();
        }

        private static IEnumerable<string> GetChromeArguments()
        {
            yield return "disable-extensions";
            yield return "start-maximized";
            yield return "disable-infobars";
            yield return "headless";
        }

        [TearDown]
        public virtual void TearDown()
        {
            AtataContext.Current?.CleanUp();
        }

        protected static void SetAndVerifyValues<T, TPage>(EditableField<T, TPage> control, params T[] values)
            where TPage : PageObject<TPage>
        {
            control.Should.Exist();

            for (int i = 0; i < values.Length; i++)
            {
                control.Set(values[i]);
                control.Should.Equal(values[i]);
                Assert.That(control.Value, Is.EqualTo(values[i]));

                if (i > 0 && !Equals(values[i], values[i - 1]))
                    control.Should.Not.Equal(values[i - 1]);
            }
        }

        protected static void SetAndVerifyValue<T, TPage>(EditableField<T, TPage> control, T value)
            where TPage : PageObject<TPage>
        {
            control.Set(value);
            VerifyEquals(control, value);
        }

        protected static void VerifyEquals<T, TPage>(Field<T, TPage> control, T value)
            where TPage : PageObject<TPage>
        {
            control.Should.Equal(value);
            Assert.That(control.Value, Is.EqualTo(value));
        }

        protected static void VerifyDoesNotEqual<T, TPage>(Field<T, TPage> control, T value)
            where TPage : PageObject<TPage>
        {
            control.Should.Not.Equal(value);

            Assert.Throws<AssertionException>(() =>
                control.Should.AtOnce.Equal(value));
        }

        protected static TException AssertThrowsWithInnerException<TException, TInnerException>(TestDelegate code)
            where TException : Exception
            where TInnerException : Exception
        {
            TException exception = Assert.Throws<TException>(code);

            Assert.That(exception.InnerException, Is.InstanceOf<TInnerException>(), "Invalid inner exception.");

            return exception;
        }

        protected static TException AssertThrowsWithoutInnerException<TException>(TestDelegate code)
            where TException : Exception
        {
            TException exception = Assert.Throws<TException>(code);

            Assert.That(exception.InnerException, Is.Null, "Inner exception should be null.");

            return exception;
        }

        protected static AssertionException AssertThrowsAssertionExceptionWithUnableToLocateMessage(TestDelegate code)
        {
            AssertionException exception = AssertThrowsWithoutInnerException<AssertionException>(code);

            Assert.That(exception.Message, Does.Contain("Actual: unable to locate"));

            return exception;
        }

        protected static void AssertThatFileShouldContainText(string filePath, params string[] texts)
        {
            FileAssert.Exists(filePath);

            using FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader reader = new(fileStream);

            string fileContent = reader.ReadToEnd();
            fileContent.Should().ContainAll(texts);
        }

        protected static void AssertThatFileShouldNotContainText(string filePath, params string[] texts)
        {
            FileAssert.Exists(filePath);
            string fileContent = File.ReadAllText(filePath);
            fileContent.Should().NotContainAll(texts);
        }

        protected void VerifyLastLogMessages(LogLevel minLogLevel, params string[] expectedMessages)
        {
            string[] actualMessages = GetLastLogMessages(minLogLevel, expectedMessages.Length);

            Assert.That(actualMessages, Is.EqualTo(expectedMessages));
        }

        protected void VerifyLastLogMessagesContain(LogLevel minLogLevel, params string[] expectedMessages)
        {
            string[] actualMessages = GetLastLogMessages(minLogLevel, expectedMessages.Length);

            for (int i = 0; i < expectedMessages.Length; i++)
            {
                Assert.That(actualMessages[i], Does.Contain(expectedMessages[i]));
            }
        }

        protected void VerifyLastLogEntries(params (LogLevel Level, string Message, Exception Exception)[] expectedLogEntries)
        {
            LogEventInfo[] actualLogEntries = GetLastLogEntries(LogLevel.Trace, expectedLogEntries.Length);

            for (int i = 0; i < expectedLogEntries.Length; i++)
            {
                Assert.That(actualLogEntries[i].Level, Is.EqualTo(expectedLogEntries[i].Level));
                Assert.That(actualLogEntries[i].Message, Is.EqualTo(expectedLogEntries[i].Message));
                Assert.That(actualLogEntries[i].Exception, Is.EqualTo(expectedLogEntries[i].Exception));
            }
        }

        protected LogEventInfo[] GetLastLogEntries(int count) =>
            GetLastLogEntries(LogLevel.Trace, count);

        protected LogEventInfo[] GetLastLogEntries(LogLevel minLogLevel, int count) =>
            LogEntries.Reverse().Where(x => x.Level >= minLogLevel).Take(count).Reverse().ToArray();

        protected string[] GetLastLogMessages(LogLevel minLogLevel, int count)
        {
            return GetLastLogEntries(minLogLevel, count).Select(x => x.Message).ToArray();
        }

        protected void AssertThatLastLogSectionIsVerificationAndEmpty()
        {
            var entries = GetLastLogEntries(2);
            entries[0].SectionStart.Should().BeOfType<VerificationLogSection>();
            entries[1].SectionEnd.Should().Be(entries[0].SectionStart);
        }

        protected void AssertThatLastLogSectionIsVerificationWithExecuteBehavior()
        {
            var entries = GetLastLogEntries(4);
            entries[0].SectionStart.Should().BeOfType<VerificationLogSection>();
            entries[1].SectionStart.Should().BeOfType<ExecuteBehaviorLogSection>();
            entries[2].SectionEnd.Should().Be(entries[1].SectionStart);
            entries[3].SectionEnd.Should().Be(entries[0].SectionStart);
        }

        protected void AssertThatLastLogSectionIsVerificationWith2ElementFindSections()
        {
            var entries = GetLastLogEntries(6);
            entries[0].SectionStart.Should().BeOfType<VerificationLogSection>();
            entries[1].SectionStart.Should().BeOfType<ElementFindLogSection>();
            entries[2].SectionEnd.Should().Be(entries[1].SectionStart);
            entries[3].SectionStart.Should().BeOfType<ElementFindLogSection>();
            entries[4].SectionEnd.Should().Be(entries[3].SectionStart);
            entries[5].SectionEnd.Should().Be(entries[0].SectionStart);
        }

        protected void AssertThatLastLogSectionIsVerificationWithExecuteBehaviorAnd3ElementFindSections()
        {
            var entries = GetLastLogEntries(10);
            entries[0].SectionStart.Should().BeOfType<VerificationLogSection>();
            entries[1].SectionStart.Should().BeOfType<ExecuteBehaviorLogSection>();
            entries[2].SectionStart.Should().BeOfType<ElementFindLogSection>();
            entries[3].SectionEnd.Should().Be(entries[2].SectionStart);
            entries[4].SectionStart.Should().BeOfType<ElementFindLogSection>();
            entries[5].SectionEnd.Should().Be(entries[4].SectionStart);
            entries[6].SectionStart.Should().BeOfType<ElementFindLogSection>();
            entries[7].SectionEnd.Should().Be(entries[6].SectionStart);
            entries[8].SectionEnd.Should().Be(entries[1].SectionStart);
            entries[9].SectionEnd.Should().Be(entries[0].SectionStart);
        }
    }
}
