using Atata.ExtentReports;

namespace Atata;

/// <summary>
/// Provides extension methods for configuring Atata with ExtentReports.
/// </summary>
public static class ExtentAtataContextBuilderExtensions
{
    /// <summary>
    /// Enables the reporting to ExtentReports.
    /// It is strongly recommended to call this method for <see cref="AtataContext.BaseConfiguration"/>.
    /// With the default configuration the "Reports.html" file generates in the Artifacts root directory.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="configure">An action delegate to configure the <see cref="ExtentReportsBuilder"/>.</param>
    /// <returns>The same builder instance.</returns>
    public static AtataContextBuilder UseExtentReports(
        this AtataContextBuilder builder,
        Action<ExtentReportsBuilder>? configure = null)
    {
        configure?.Invoke(ExtentContext.Configuration);

        builder.LogConsumers.Add<ExtentLogConsumer>(x => x
            .WithTargetScopes(AtataContextScopes.Test | AtataContextScopes.TestSuite)
            .WithMinLevel(LogLevel.Info)
            .WithSectionEnd(LogSectionEndOption.IncludeForBlocks));

        var testAnTestSuiteEventSubscriptions = builder.EventSubscriptions.For(AtataContextScopes.Test | AtataContextScopes.TestSuite);
        testAnTestSuiteEventSubscriptions.Add(new StartExtentTestItemEventHandler());
        testAnTestSuiteEventSubscriptions.Add(new AddScreenshotToExtentLogEventHandler());
        testAnTestSuiteEventSubscriptions.Add(new AddArtifactListToExtentLogEventHandler());
        testAnTestSuiteEventSubscriptions.Add(new EndExtentTestItemEventHandler());

        builder.EventSubscriptions.For(AtataContextScopes.Global).Add(new FlushExtentReportsEventHandler());

        return builder;
    }
}
