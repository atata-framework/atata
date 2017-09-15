using System.Collections.Generic;
using System.Configuration;
using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public abstract class UITestFixture
    {
        private StringListLogConsumer stringListLogConsumer;

        public IEnumerable<LogEventInfo> LogEntries => stringListLogConsumer.Items;

        [SetUp]
        public void SetUp()
        {
            string baseUrl = ConfigurationManager.AppSettings["TestAppUrl"];

            stringListLogConsumer = new StringListLogConsumer();

            AtataContext.Configure().
                UseChrome().
                    WithArguments("disable-extensions", "no-sandbox", "start-maximized").
                UseBaseUrl(baseUrl).
                UseNUnitTestName().
                AddNUnitTestContextLogging().
                AddLogConsumer(stringListLogConsumer).
                LogNUnitError().
                SetUp();

            OnSetUp();
        }

        protected virtual void OnSetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
            AtataContext.Current.CleanUp();
        }

        protected void SetAndVerifyValues<T, TPage>(EditableField<T, TPage> control, params T[] values)
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

        protected void SetAndVerifyValue<T, TPage>(EditableField<T, TPage> control, T value)
            where TPage : PageObject<TPage>
        {
            control.Set(value);
            VerifyEquals(control, value);
        }

        protected void VerifyEquals<T, TPage>(Field<T, TPage> control, T value)
            where TPage : PageObject<TPage>
        {
            control.Should.Equal(value);
            Assert.That(control.Value, Is.EqualTo(value));
        }

        protected void VerifyDoesNotEqual<T, TPage>(Field<T, TPage> control, T value)
            where TPage : PageObject<TPage>
        {
            control.Should.Not.Equal(value);

            Assert.Throws<AssertionException>(() =>
                control.Should.AtOnce.Equal(value));
        }
    }
}
