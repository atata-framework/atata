using Atata.NUnit;

namespace Atata;

public static class NUnitAtataContextBuilderExtensions
{
    /// <summary>
    /// Defines that the name of the test should be taken from NUnit test.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder UseNUnitTestName(this AtataContextBuilder builder) =>
        builder.UseTestName(NUnitAdapter.GetCurrentTestName);

    /// <summary>
    /// Defines that the name of the test suite should be taken from NUnit test fixture.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder UseNUnitTestSuiteName(this AtataContextBuilder builder) =>
        builder.UseTestSuiteName(NUnitAdapter.GetCurrentTestFixtureName);

    /// <summary>
    /// Defines that the type of the test suite should be taken from NUnit test fixture.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder UseNUnitTestSuiteType(this AtataContextBuilder builder) =>
        builder.UseTestSuiteType(NUnitAdapter.GetCurrentTestFixtureType);

    /// <summary>
    /// Defines that the test traits should be taken from properties of NUnit test or fixture .
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder UseNUnitTestTraits(this AtataContextBuilder builder) =>
        builder.UseTestTraits(NUnitAdapter.GetCurrentTestTraits);

    /// <summary>
    /// Sets <see cref="NUnitWarningReportStrategy"/> as the strategy for warning assertion reporting.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder UseNUnitWarningReportStrategy(this AtataContextBuilder builder) =>
        builder.UseWarningReportStrategy(NUnitWarningReportStrategy.Instance);

    /// <summary>
    /// Uses the <see cref="NUnitAssertionFailureReportStrategy"/> as the strategy for assertion failure reporting.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder UseNUnitAssertionFailureReportStrategy(this AtataContextBuilder builder) =>
        builder.UseAssertionFailureReportStrategy(NUnitAssertionFailureReportStrategy.Instance);

    /// <summary>
    /// Sets <see cref="NUnitAggregateAssertionStrategy"/> as the aggregate assertion strategy.
    /// The <see cref="NUnitAggregateAssertionStrategy"/> uses NUnit's <see cref="Assert.EnterMultipleScope"/> method for aggregate assertion.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder UseNUnitAggregateAssertionStrategy(this AtataContextBuilder builder) =>
        builder.UseAggregateAssertionStrategy(NUnitAggregateAssertionStrategy.Instance);

    [Obsolete("Use UseNUnitAssertionExceptionFactory() instead.")] // Obsolete since v4.0.0.
    public static AtataContextBuilder UseNUnitAssertionExceptionType(this AtataContextBuilder builder) =>
        builder.UseNUnitAssertionExceptionFactory();

    /// <summary>
    /// Sets the type of <c>NUnit.Framework.AssertionException</c> as the assertion exception type.
    /// The default value is a type of <see cref="AssertionException"/>.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder UseNUnitAssertionExceptionFactory(this AtataContextBuilder builder) =>
        builder.UseAssertionExceptionFactory(NUnitAssertionExceptionFactory.Instance);

    /// <summary>
    /// Enables all Atata features for NUnit.
    /// Executes the following methods:
    /// <list type="bullet">
    /// <item><see cref="UseNUnitTestName"/></item>
    /// <item><see cref="UseNUnitTestSuiteName"/></item>
    /// <item><see cref="UseNUnitTestSuiteType"/></item>
    /// <item><see cref="UseNUnitAssertionExceptionFactory"/></item>
    /// <item><see cref="UseNUnitAggregateAssertionStrategy"/></item>
    /// <item><see cref="UseNUnitWarningReportStrategy"/></item>
    /// <item><see cref="UseNUnitAssertionFailureReportStrategy"/></item>
    /// <item><see cref="NUnitLogConsumersBuilderExtensions.AddNUnitTestContext(LogConsumersBuilder, Action{LogConsumerBuilder{NUnitTestContextLogConsumer}}?)"/> for <see cref="AtataContextBuilder.LogConsumers"/> property</item>
    /// <item><see cref="NUnitEventSubscriptionsBuilderExtensions.LogNUnitError{TRootBuilder}(EventSubscriptionsBuilder{TRootBuilder})"/> for <see cref="AtataContextBuilder.EventSubscriptions"/> property</item>
    /// <item><see cref="NUnitEventSubscriptionsBuilderExtensions.TakeScreenshotOnNUnitError{TRootBuilder}(EventSubscriptionsBuilder{TRootBuilder}, string)"/> for <see cref="AtataContextBuilder.EventSubscriptions"/> property</item>
    /// <item><see cref="NUnitEventSubscriptionsBuilderExtensions.TakePageSnapshotOnNUnitError{TRootBuilder}(EventSubscriptionsBuilder{TRootBuilder}, string)"/> for <see cref="AtataContextBuilder.EventSubscriptions"/> property</item>
    /// <item><see cref="NUnitEventSubscriptionsBuilderExtensions.AddArtifactsToNUnitTestContext{TRootBuilder}(EventSubscriptionsBuilder{TRootBuilder})"/> for <see cref="AtataContextBuilder.EventSubscriptions"/> property</item>
    /// </list>
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    [Obsolete("It is recommended to inherit AtataGlobalFixture and AtataTestSuite classes for global and test suites. Otherwise call all methods directly instead.")] // Obsolete since v4.0.0.
    public static AtataContextBuilder UseAllNUnitFeatures(this AtataContextBuilder builder)
    {
        builder.UseNUnitTestName();
        builder.UseNUnitTestSuiteName();
        builder.UseNUnitTestSuiteType();
        builder.UseNUnitAssertionExceptionFactory();
        builder.UseNUnitAggregateAssertionStrategy();
        builder.UseNUnitWarningReportStrategy();
        builder.UseNUnitAssertionFailureReportStrategy();
        builder.LogConsumers.AddNUnitTestContext();
        builder.EventSubscriptions.LogNUnitError();
        builder.EventSubscriptions.TakeScreenshotOnNUnitError();
        builder.EventSubscriptions.TakePageSnapshotOnNUnitError();
        builder.EventSubscriptions.AddArtifactsToNUnitTestContext();

        return builder;
    }

    /// <summary>
    /// Enables all Atata features for SpecFlow+NUnit.
    /// Executes the following methods:
    /// <list type="bullet">
    /// <item><see cref="UseNUnitTestName"/></item>
    /// <item><see cref="UseNUnitTestSuiteName"/></item>
    /// <item><see cref="UseNUnitTestSuiteType"/></item>
    /// <item><see cref="UseNUnitAssertionExceptionFactory"/></item>
    /// <item><see cref="UseNUnitAggregateAssertionStrategy"/></item>
    /// <item><see cref="UseNUnitWarningReportStrategy"/></item>
    /// <item><see cref="UseNUnitAssertionFailureReportStrategy"/></item>
    /// <item><see cref="NUnitEventSubscriptionsBuilderExtensions.LogNUnitError{TRootBuilder}(EventSubscriptionsBuilder{TRootBuilder})"/> for <see cref="AtataContextBuilder.EventSubscriptions"/> property</item>
    /// <item><see cref="NUnitEventSubscriptionsBuilderExtensions.TakeScreenshotOnNUnitError{TRootBuilder}(EventSubscriptionsBuilder{TRootBuilder}, string)"/> for <see cref="AtataContextBuilder.EventSubscriptions"/> property</item>
    /// <item><see cref="NUnitEventSubscriptionsBuilderExtensions.TakePageSnapshotOnNUnitError{TRootBuilder}(EventSubscriptionsBuilder{TRootBuilder}, string)"/> for <see cref="AtataContextBuilder.EventSubscriptions"/> property</item>
    /// </list>
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    [Obsolete("It is recommended to use Atata.Reqnroll.NUnit package instead.")] // Obsolete since v4.0.0.
    public static AtataContextBuilder UseSpecFlowNUnitFeatures(this AtataContextBuilder builder)
    {
        builder.UseNUnitTestName();
        builder.UseNUnitTestSuiteName();
        builder.UseNUnitTestSuiteType();
        builder.UseNUnitAssertionExceptionFactory();
        builder.UseNUnitAggregateAssertionStrategy();
        builder.UseNUnitWarningReportStrategy();
        builder.UseNUnitAssertionFailureReportStrategy();
        builder.EventSubscriptions.LogNUnitError();
        builder.EventSubscriptions.TakeScreenshotOnNUnitError();
        builder.EventSubscriptions.TakePageSnapshotOnNUnitError();

        return builder;
    }
}
