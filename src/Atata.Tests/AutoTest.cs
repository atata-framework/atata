using System.Configuration;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Atata.Tests
{
    [TestFixture]
    public abstract class AutoTest
    {
        [SetUp]
        public void SetUp()
        {
            string baseUrl = ConfigurationManager.AppSettings["TestAppUrl"];

            AtataContext.Build().
                UseChrome().
                    WithArguments("disable-extensions", "no-sandbox", "start-maximized").
                UseBaseUrl(baseUrl).
                UseNUnitTestName().
                UseNUnitTestContextLogging().
                    WithMinLevel(LogLevel.Info).
                SetUp();

            OnSetUp();
        }

        protected virtual void OnSetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
            var testResult = TestContext.CurrentContext.Result;
            if (testResult.Outcome.Status == TestStatus.Failed)
                AtataContext.Current.Log.Error(testResult.Message, testResult.StackTrace);

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
                Assert.That(control.Get(), Is.EqualTo(values[i]));

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
            Assert.That(control.Get(), Is.EqualTo(value));
        }

        protected void VerifyDoesNotEqual<T, TPage>(Field<T, TPage> control, T value)
            where TPage : PageObject<TPage>
        {
            control.Should.Not.Equal(value);

            Assert.Throws<AssertionException>(() =>
                control.Should.WithoutRetry.Equal(value));
        }
    }
}
