namespace Atata;

/// <summary>
/// Provides NUnit extension methods for <see cref="EventSubscriptionsBuilder"/>.
/// </summary>
public static class NUnitEventSubscriptionsBuilderExtensions
{
    /// <summary>
    /// Defines that an error occurred during the NUnit test execution
    /// should be added to the log during <see cref="AtataContext"/> deinitialization.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The same <see cref="EventSubscriptionsBuilder"/> instance.</returns>
    public static EventSubscriptionsBuilder LogNUnitError(
        this EventSubscriptionsBuilder builder)
        =>
        builder.Add(new LogNUnitErrorEventHandler());

    /// <summary>
    /// Defines that an error occurred during the NUnit test execution
    /// should be captured by a screenshot during <see cref="AtataContext"/> deinitialization.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="title">The screenshot title.</param>
    /// <returns>The same <see cref="EventSubscriptionsBuilder"/> instance.</returns>
    public static EventSubscriptionsBuilder TakeScreenshotOnNUnitError(
        this EventSubscriptionsBuilder builder,
        string title = "Failed")
        =>
        builder.Add(new TakeScreenshotOnNUnitErrorEventHandler(title));

    /// <inheritdoc cref="TakeScreenshotOnNUnitError(EventSubscriptionsBuilder, string)"/>
    /// <param name="builder">The builder.</param>
    /// <param name="kind">The kind of a screenshot.</param>
    /// <param name="title">The screenshot title.</param>
    public static EventSubscriptionsBuilder TakeScreenshotOnNUnitError(
        this EventSubscriptionsBuilder builder,
        ScreenshotKind kind,
        string title = "Failed")
        =>
        builder.Add(new TakeScreenshotOnNUnitErrorEventHandler(kind, title));

    /// <summary>
    /// Defines that an error occurred during the NUnit test execution
    /// should be captured by a page snapshot during <see cref="AtataContext"/> deinitialization.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="title">The snapshot title.</param>
    /// <returns>The same <see cref="EventSubscriptionsBuilder"/> instance.</returns>
    public static EventSubscriptionsBuilder TakePageSnapshotOnNUnitError(
        this EventSubscriptionsBuilder builder,
        string title = "Failed")
        =>
        builder.Add(new TakePageSnapshotOnNUnitErrorEventHandler(title));

    /// <summary>
    /// Defines that after <see cref="AtataContext"/> deinitialization the files stored in Artifacts directory
    /// should be added to NUnit <c>TestContext</c>.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The same <see cref="EventSubscriptionsBuilder"/> instance.</returns>
    public static EventSubscriptionsBuilder AddArtifactsToNUnitTestContext(
        this EventSubscriptionsBuilder builder)
        =>
        builder.Add(new AddArtifactsToNUnitTestContextEventHandler());

    /// <summary>
    /// Defines that after <see cref="AtataContext"/> deinitialization the files stored in the directory
    /// specified by <paramref name="directoryPath"/> should be added to NUnit <c>TestContext</c>.
    /// Directory path supports template variables.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="directoryPath">The directory path.</param>
    /// <returns>The same <see cref="EventSubscriptionsBuilder" /> instance.</returns>
    public static EventSubscriptionsBuilder AddDirectoryFilesToNUnitTestContext(
        this EventSubscriptionsBuilder builder,
        string directoryPath)
    {
        directoryPath.CheckNotNullOrWhitespace(nameof(directoryPath));
        return builder.Add(new AddDirectoryFilesToNUnitTestContextEventHandler(directoryPath));
    }

    /// <inheritdoc cref="AddDirectoryFilesToNUnitTestContext(EventSubscriptionsBuilder, Func{AtataContext, string})"/>
    public static EventSubscriptionsBuilder AddDirectoryFilesToNUnitTestContext(
        this EventSubscriptionsBuilder builder,
        Func<string> directoryPathBuilder)
    {
        directoryPathBuilder.CheckNotNull(nameof(directoryPathBuilder));
        return builder.AddDirectoryFilesToNUnitTestContext(_ => directoryPathBuilder.Invoke());
    }

    /// <summary>
    /// Defines that after <see cref="AtataContext"/> deinitialization the files stored in the directory
    /// specified by <paramref name="directoryPathBuilder"/> should be added to NUnit <c>TestContext</c>.
    /// Directory path supports template variables.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="directoryPathBuilder">The directory path builder.</param>
    /// <returns>The same <see cref="EventSubscriptionsBuilder" /> instance.</returns>
    public static EventSubscriptionsBuilder AddDirectoryFilesToNUnitTestContext(
        this EventSubscriptionsBuilder builder,
        Func<AtataContext, string> directoryPathBuilder)
    {
        directoryPathBuilder.CheckNotNull(nameof(directoryPathBuilder));
        return builder.Add(new AddDirectoryFilesToNUnitTestContextEventHandler(directoryPathBuilder));
    }
}
