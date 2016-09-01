using System.Configuration;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium.Chrome;

namespace Atata.Tests
{
    [TestFixture]
    public abstract class AutoTest
    {
        [SetUp]
        public void SetUp()
        {
            var log = new LogManager().
                Use(new NUnitTestContextLogConsumer());

            string startUrl = ConfigurationManager.AppSettings["TestAppUrl"];

            ATContext.SetUp(
                () => new ChromeDriver().Maximize(),
                log,
                TestContext.CurrentContext.Test.Name,
                startUrl);

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
                ATContext.Current.Log.Error(testResult.Message, testResult.StackTrace);

            ATContext.CleanUp();
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

        protected void VerifyEquals<T, TPage>(EditableField<T, TPage> control, T value)
            where TPage : PageObject<TPage>
        {
            control.Should.Equal(value);
            Assert.That(control.Get(), Is.EqualTo(value));
        }

        protected void VerifyDoesNotEqual<T, TPage>(EditableField<T, TPage> control, T value)
            where TPage : PageObject<TPage>
        {
            control.Should.Not.Equal(value);

            Assert.Throws<AssertionException>(() =>
                control.Should.WithoutRetry.Equal(value));
        }
    }
}
