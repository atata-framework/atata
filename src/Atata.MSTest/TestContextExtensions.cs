namespace Atata.MSTest;

/// <summary>
/// Provides extension methods for the <see cref="TestContext"/> class to manage <see cref="AtataContext"/> instances.
/// </summary>
public static class TestContextExtensions
{
    private const string AtataContextPropertyKey = nameof(AtataContext);

    /// <summary>
    /// Sets the <see cref="AtataContext"/> instance into the <see cref="TestContext"/> properties.
    /// </summary>
    /// <param name="testContext">The test context.</param>
    /// <param name="atataContext">The <see cref="AtataContext"/> instance to set.</param>
    public static void SetAtataContext(this TestContext testContext, AtataContext atataContext) =>
        testContext.Properties[AtataContextPropertyKey] = atataContext;

    /// <summary>
    /// Removes the <see cref="AtataContext"/> instance from the <see cref="TestContext"/> properties.
    /// </summary>
    /// <param name="testContext">The test context.</param>
    public static void RemoveAtataContext(this TestContext testContext) =>
        testContext.Properties.Remove(AtataContextPropertyKey);

    /// <summary>
    /// Gets the <see cref="AtataContext"/> instance from the <see cref="TestContext"/> properties.
    /// Throws an <see cref="AtataContextNotFoundException"/> if the context is not found.
    /// </summary>
    /// <param name="testContext">The test context.</param>
    /// <returns>The <see cref="AtataContext"/> instance.</returns>
    /// <exception cref="AtataContextNotFoundException">
    /// Thrown when the <see cref="AtataContext"/> instance is not found in the <see cref="TestContext"/> properties.
    /// </exception>
    public static AtataContext GetAtataContext(this TestContext testContext) =>
        testContext.GetAtataContextOrNull()
            ?? throw new AtataContextNotFoundException(
                $"Failed to find {nameof(AtataContext)} instance in the given {nameof(TestContext)} instance.");

    /// <summary>
    /// Gets the <see cref="AtataContext"/> instance from the <see cref="TestContext"/> properties, or <see langword="null"/> if not found.
    /// </summary>
    /// <param name="testContext">The test context.</param>
    /// <returns>The <see cref="AtataContext"/> instance, or <see langword="null"/> if not found.</returns>
    public static AtataContext? GetAtataContextOrNull(this TestContext testContext) =>
        testContext.Properties[AtataContextPropertyKey] as AtataContext;
}
