using NUnit.Framework;
using OpenQA.Selenium.Firefox;

namespace Atata.Tests
{
    public abstract class TestBase : UITest
    {
        [SetUp]
        public void SetUp()
        {
            NativeDriver = new FirefoxDriver();
            Logger = new SimpleLogManager(
                message =>
                {
                    TestContext.WriteLine(message);
                },
                NativeDriver);

            Logger.Info("Start test");
            NativeDriver.Manage().Window.Maximize();
        }

        [TearDown]
        public void TearDown()
        {
            Logger.Info("Finish test");
            NativeDriver.Quit();
        }
    }
}
