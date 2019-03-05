using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public abstract class UITestFixtureBase
    {
        public const string BaseUrl = "http://localhost:50549";

        private StringListLogConsumer stringListLogConsumer;

        protected IEnumerable<LogEventInfo> LogEntries => stringListLogConsumer.Items;

        protected AtataContextBuilder ConfigureBaseAtataContext()
        {
            stringListLogConsumer = new StringListLogConsumer();

            return AtataContext.Configure().
                UseChrome().
                    WithArguments("disable-extensions", "start-maximized", "disable-infobars").
#if NETCOREAPP2_0
                    WithFixOfCommandExecutionDelay().
                    WithLocalDriverPath().
#endif
                UseBaseUrl(BaseUrl).
                UseCulture("en-us").
                UseNUnitTestName().
                AddNUnitTestContextLogging().
                AddLogConsumer(stringListLogConsumer).
                LogNUnitError();
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

        protected void VerifyLastLogMessages(params string[] expectedMessages)
        {
            Assert.That(GetLastLogMessages(expectedMessages.Length), Is.EqualTo(expectedMessages));
        }

        protected void VerifyLastLogMessagesContain(params string[] expectedMessages)
        {
            string[] actualMessages = GetLastLogMessages(expectedMessages.Length);

            for (int i = 0; i < expectedMessages.Length; i++)
            {
                Assert.That(actualMessages[i], Does.Contain(expectedMessages[i]));
            }
        }

        protected void VerifyLastLogEntries(params (LogLevel Level, string Message, Exception Exception)[] expectedLogEntries)
        {
            LogEventInfo[] actualLogEntries = GetLastLogEntries(expectedLogEntries.Length);

            for (int i = 0; i < expectedLogEntries.Length; i++)
            {
                Assert.That(actualLogEntries[i].Level, Is.EqualTo(expectedLogEntries[i].Level));
                Assert.That(actualLogEntries[i].Message, Is.EqualTo(expectedLogEntries[i].Message));
                Assert.That(actualLogEntries[i].Exception, Is.EqualTo(expectedLogEntries[i].Exception));
            }
        }

        protected LogEventInfo[] GetLastLogEntries(int count)
        {
            return LogEntries.Reverse().Take(count).Reverse().ToArray();
        }

        protected string[] GetLastLogMessages(int count)
        {
            return GetLastLogEntries(count).Select(x => x.Message).ToArray();
        }
    }
}
