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
        /// Adds arguments to be appended to the Chrome.exe command line.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The <see cref="ChromeAtataContextBuilder"/> instance.</returns>
        public static ChromeAtataContextBuilder WithArguments(this ChromeAtataContextBuilder builder, params string[] arguments)
        {
            return builder.WithOptions(options => options.AddArguments(arguments));
        }

        /// <summary>
        /// Adds arguments to be appended to the Opera.exe command line.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The <see cref="OperaAtataContextBuilder"/> instance.</returns>
        public static OperaAtataContextBuilder WithArguments(this OperaAtataContextBuilder builder, params string[] arguments)
        {
            return builder.WithOptions(options => options.AddArguments(arguments));
        }

        /// <summary>
        /// Defines that the name of the test should be taken from the NUnit test.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public static AtataContextBuilder UseNUnitTestName(this AtataContextBuilder builder)
        {
            dynamic testContext = GetNUnitTestContext();
            string testName = testContext.Test.Name;

            return builder.UseTestName(testName);
        }

        /// <summary>
        /// Defines that an error occured during the NUnit test execution should be added to the log upon the clean up.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public static AtataContextBuilder LogNUnitError(this AtataContextBuilder builder)
        {
            return builder.OnCleanUp(() =>
            {
                dynamic testContext = GetNUnitTestContext();
                var testResult = testContext.Result;

                if ((int)testResult.Outcome.Status == 3)
                    AtataContext.Current.Log.Error((string)testResult.Message, (string)testResult.StackTrace);
            });
        }

        private static object GetNUnitTestContext()
        {
            Type testContextType = Type.GetType("NUnit.Framework.TestContext,nunit.framework", true);
            PropertyInfo currentContextProperty = testContextType.GetPropertyWithThrowOnError("CurrentContext");

            return currentContextProperty.GetStaticValue();
        }

        /// <summary>
        /// Adds the <see cref="TraceLogConsumer"/> instance that uses <see cref="Trace"/> class for logging.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="AtataContextBuilder{ILogConsumer}"/> instance.</returns>
        public static AtataContextBuilder<ILogConsumer> UseTraceLogging(this AtataContextBuilder builder)
        {
            return builder.UseLogConsumer<ILogConsumer>(new TraceLogConsumer());
        }

        /// <summary>
        /// Adds the <see cref="DebugLogConsumer"/> instance that uses <see cref="Debug"/> class for logging.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="AtataContextBuilder{ILogConsumer}"/> instance.</returns>
        public static AtataContextBuilder<ILogConsumer> UseDebugLogging(this AtataContextBuilder builder)
        {
            return builder.UseLogConsumer<ILogConsumer>(new DebugLogConsumer());
        }

        /// <summary>
        /// Adds the <see cref="NUnitTestContextLogConsumer"/> instance that uses NUnit.Framework.TestContext class for logging.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The <see cref="AtataContextBuilder{ILogConsumer}"/> instance.</returns>
        public static AtataContextBuilder<ILogConsumer> UseNUnitTestContextLogging(this AtataContextBuilder builder)
        {
            return builder.UseLogConsumer<ILogConsumer>(new NUnitTestContextLogConsumer());
        }

        /// <summary>
        /// Adds the <see cref="NLogConsumer"/> instance that uses NLog.Logger class for logging.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="loggerName">Name of the logger.</param>
        /// <returns>The <see cref="AtataContextBuilder{ILogConsumer}"/> instance.</returns>
        public static AtataContextBuilder<ILogConsumer> UseNLogLogging(this AtataContextBuilder builder, string loggerName = null)
        {
            return builder.UseLogConsumer<ILogConsumer>(new NLogConsumer(loggerName));
        }

        /// <summary>
        /// Adds the <see cref="FileScreenshotConsumer"/> instance with the specified folder path.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="folderPath">The folder path.</param>
        /// <returns>The <see cref="AtataContextBuilder{FileScreenshotConsumer}"/> instance.</returns>
        public static AtataContextBuilder<FileScreenshotConsumer> UseScreenshotFileSaving(this AtataContextBuilder builder, string folderPath)
        {
            return builder.UseScreenshotConsumer(new FileScreenshotConsumer(folderPath));
        }

        /// <summary>
        /// Adds the <see cref="FileScreenshotConsumer"/> instance with the specified folder path creator.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="folderPathCreator">The folder path creator.</param>
        /// <returns>The <see cref="AtataContextBuilder{FileScreenshotConsumer}"/> instance.</returns>
        public static AtataContextBuilder<FileScreenshotConsumer> UseScreenshotFileSaving(this AtataContextBuilder builder, Func<string> folderPathCreator)
        {
            return builder.UseScreenshotConsumer(new FileScreenshotConsumer(folderPathCreator));
        }

        /// <summary>
        /// Specifies the image format of the log consumer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="imageFormat">The image format.</param>
        /// <returns>The <see cref="AtataContextBuilder{FileScreenshotConsumer}"/> instance.</returns>
        public static AtataContextBuilder<FileScreenshotConsumer> With(this AtataContextBuilder<FileScreenshotConsumer> builder, ImageFormat imageFormat)
        {
            builder.Context.ImageFormat = imageFormat;
            return builder;
        }

        /// <summary>
        /// Defines that the logging should not use section-like messages (not "Starting: {action}" and "Finished: {action}", but just "{action}").
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
