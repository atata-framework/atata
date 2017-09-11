using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Safari;

namespace Atata
{
    public static class AtataContextBuilderExtensions
    {
        /// <summary>
        /// Use the <see cref="ChromeDriver"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="ChromeAtataContextBuilder"/> instance.</returns>
        public static ChromeAtataContextBuilder UseChrome(this AtataContextBuilder builder)
        {
            return new ChromeAtataContextBuilder(builder.BuildingContext);
        }

        /// <summary>
        /// Use the <see cref="FirefoxDriver"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="FirefoxAtataContextBuilder"/> instance.</returns>
        public static FirefoxAtataContextBuilder UseFirefox(this AtataContextBuilder builder)
        {
            return new FirefoxAtataContextBuilder(builder.BuildingContext);
        }

        /// <summary>
        /// Use the <see cref="InternetExplorerDriver"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="InternetExplorerAtataContextBuilder"/> instance.</returns>
        public static InternetExplorerAtataContextBuilder UseInternetExplorer(this AtataContextBuilder builder)
        {
            return new InternetExplorerAtataContextBuilder(builder.BuildingContext);
        }

        /// <summary>
        /// Use the <see cref="EdgeDriver"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="EdgeAtataContextBuilder"/> instance.</returns>
        public static EdgeAtataContextBuilder UseEdge(this AtataContextBuilder builder)
        {
            return new EdgeAtataContextBuilder(builder.BuildingContext);
        }

        /// <summary>
        /// Use the <see cref="OperaDriver"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="OperaAtataContextBuilder"/> instance.</returns>
        public static OperaAtataContextBuilder UseOpera(this AtataContextBuilder builder)
        {
            return new OperaAtataContextBuilder(builder.BuildingContext);
        }

        /// <summary>
        /// Use the <see cref="PhantomJSDriver"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="PhantomJSAtataContextBuilder"/> instance.</returns>
        public static PhantomJSAtataContextBuilder UsePhantomJS(this AtataContextBuilder builder)
        {
            return new PhantomJSAtataContextBuilder(builder.BuildingContext);
        }

        /// <summary>
        /// Use the <see cref="SafariDriver"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="SafariAtataContextBuilder"/> instance.</returns>
        public static SafariAtataContextBuilder UseSafari(this AtataContextBuilder builder)
        {
            return new SafariAtataContextBuilder(builder.BuildingContext);
        }

        /// <summary>
        /// Defines that the name of the test should be taken from the NUnit test.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public static AtataContextBuilder UseNUnitTestName(this AtataContextBuilder builder)
        {
            return builder.UseTestName(ResolveNUnitTestName);
        }

        private static string ResolveNUnitTestName()
        {
            dynamic testContext = GetNUnitTestContext();
            return testContext.Test.Name;
        }

        /// <summary>
        /// Defines that an error occurred during the NUnit test execution should be added to the log during the cleanup.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public static AtataContextBuilder LogNUnitError(this AtataContextBuilder builder)
        {
            return builder.OnCleanUp(() =>
            {
                dynamic testResult = GetNUnitTestResult();

                if (IsNUnitTestResultFailed(testResult))
                    AtataContext.Current.Log.Error((string)testResult.Message, (string)testResult.StackTrace);
            });
        }

        /// <summary>
        /// Defines that an error occurred during the NUnit test execution should be captured by a screenshot during the cleanup.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="title">The screenshot title.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public static AtataContextBuilder TakeScreenshotOnNUnitError(this AtataContextBuilder builder, string title = "Failed")
        {
            return builder.OnCleanUp(() =>
            {
                dynamic testResult = GetNUnitTestResult();

                if (IsNUnitTestResultFailed(testResult))
                    AtataContext.Current.Log.Screenshot(title);
            });
        }

        private static dynamic GetNUnitTestContext()
        {
            Type testContextType = Type.GetType("NUnit.Framework.TestContext,nunit.framework", true);
            PropertyInfo currentContextProperty = testContextType.GetPropertyWithThrowOnError("CurrentContext");

            return currentContextProperty.GetStaticValue();
        }

        private static dynamic GetNUnitTestResult()
        {
            dynamic testContext = GetNUnitTestContext();
            return testContext.Result;
        }

        private static bool IsNUnitTestResultFailed(dynamic testResult)
        {
            return testResult.Outcome.Status.ToString().Contains("Fail");
        }

        /// <summary>
        /// Adds the <see cref="TraceLogConsumer"/> instance that uses <see cref="Trace"/> class for logging.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="AtataContextBuilder{ILogConsumer}"/> instance.</returns>
        public static AtataContextBuilder<ILogConsumer> AddTraceLogging(this AtataContextBuilder builder)
        {
            return builder.AddLogConsumer<ILogConsumer>(new TraceLogConsumer());
        }

        /// <summary>
        /// Adds the <see cref="DebugLogConsumer"/> instance that uses <see cref="Debug"/> class for logging.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="AtataContextBuilder{ILogConsumer}"/> instance.</returns>
        public static AtataContextBuilder<ILogConsumer> AddDebugLogging(this AtataContextBuilder builder)
        {
            return builder.AddLogConsumer<ILogConsumer>(new DebugLogConsumer());
        }

        /// <summary>
        /// Adds the <see cref="NUnitTestContextLogConsumer"/> instance that uses NUnit.Framework.TestContext class for logging.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="AtataContextBuilder{ILogConsumer}"/> instance.</returns>
        public static AtataContextBuilder<ILogConsumer> AddNUnitTestContextLogging(this AtataContextBuilder builder)
        {
            return builder.AddLogConsumer<ILogConsumer>(new NUnitTestContextLogConsumer());
        }

        /// <summary>
        /// Adds the <see cref="NLogConsumer"/> instance that uses NLog.Logger class for logging.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="loggerName">Name of the logger.</param>
        /// <returns>The <see cref="AtataContextBuilder{ILogConsumer}"/> instance.</returns>
        public static AtataContextBuilder<ILogConsumer> AddNLogLogging(this AtataContextBuilder builder, string loggerName = null)
        {
            return builder.AddLogConsumer<ILogConsumer>(new NLogConsumer(loggerName));
        }

        /// <summary>
        /// Adds the <see cref="FileScreenshotConsumer"/> instance with the specified folder path.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="folderPath">The folder path.</param>
        /// <returns>The <see cref="AtataContextBuilder{FileScreenshotConsumer}"/> instance.</returns>
        [Obsolete("Use AddScreenshotFileSaving().WithFolderPath(() => folderPath) instead.")]
        public static AtataContextBuilder<FileScreenshotConsumer> AddScreenshotFileSaving(this AtataContextBuilder builder, string folderPath)
        {
            return builder.AddScreenshotConsumer(new FileScreenshotConsumer { FolderPathBuilder = () => folderPath });
        }

        /// <summary>
        /// Adds the <see cref="FileScreenshotConsumer"/> instance with the specified folder path builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="folderPathBuilder">The folder path builder.</param>
        /// <returns>The <see cref="AtataContextBuilder{FileScreenshotConsumer}"/> instance.</returns>
        [Obsolete("Use AddScreenshotFileSaving().WithFolderPath(folderPathBuilder) instead.")]
        public static AtataContextBuilder<FileScreenshotConsumer> AddScreenshotFileSaving(this AtataContextBuilder builder, Func<string> folderPathBuilder)
        {
            return builder.AddScreenshotConsumer(new FileScreenshotConsumer { FolderPathBuilder = folderPathBuilder });
        }

        /// <summary>
        /// Specifies the image format of the file screenshot consumer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="imageFormat">The image format.</param>
        /// <returns>The <see cref="AtataContextBuilder{FileScreenshotConsumer}"/> instance.</returns>
        [Obsolete("Use With(ScreenshotImageFormat imageFormat) instead.")]
        public static AtataContextBuilder<FileScreenshotConsumer> With(this AtataContextBuilder<FileScreenshotConsumer> builder, ImageFormat imageFormat)
        {
            builder.Context.ImageFormat = imageFormat.ToScreenshotImageFormat();
            return builder;
        }

        /// <summary>
        /// Defines that the logging should not use section-like pair messages (not "Starting: {action}" and "Finished: {action}", but just "{action}").
        /// </summary>
        /// <typeparam name="TTLogConsumer">The type of the log consumer.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="AtataContextBuilder{TTLogConsumer}"/> instance.</returns>
        public static AtataContextBuilder<TTLogConsumer> WithoutSectionFinish<TTLogConsumer>(this AtataContextBuilder<TTLogConsumer> builder)
            where TTLogConsumer : ILogConsumer
        {
            LogConsumerInfo consumerInfo = builder.BuildingContext.LogConsumers.Single(x => Equals(x.Consumer, builder.Context));
            consumerInfo.LogSectionFinish = false;
            return builder;
        }

        /// <summary>
        /// Specifies the minimum level of the log event to write to the log.
        /// </summary>
        /// <typeparam name="TTLogConsumer">The type of the log consumer.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="level">The level.</param>
        /// <returns>The <see cref="AtataContextBuilder{TTLogConsumer}"/> instance.</returns>
        public static AtataContextBuilder<TTLogConsumer> WithMinLevel<TTLogConsumer>(this AtataContextBuilder<TTLogConsumer> builder, LogLevel level)
            where TTLogConsumer : ILogConsumer
        {
            LogConsumerInfo consumerInfo = builder.BuildingContext.LogConsumers.Single(x => Equals(x.Consumer, builder.Context));
            consumerInfo.MinLevel = level;
            return builder;
        }
    }
}
