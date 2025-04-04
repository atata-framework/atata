namespace Atata;

/// <summary>
/// Provides a set of extension methods for <see cref="IReport{TOwner}"/>.
/// </summary>
public static class PageObjectReportExtensions
{
    /// <summary>
    /// Executes the specified function and represents it in a log as a setup section with the message like <c>"Set up "&lt;Some&gt;" page"</c>.
    /// The setup function time is not counted as a "Test body" execution time, but counted as "Setup" time.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <typeparam name="TPageObject">The type of the result page object.</typeparam>
    /// <param name="report">The <see cref="IReport{TOwner}"/> object.</param>
    /// <param name="function">The setup function.</param>
    /// <returns>The result page object of the <paramref name="function"/>.</returns>
    public static TPageObject Setup<TOwner, TPageObject>(this IReport<TOwner> report, Func<TOwner, TPageObject> function)
        where TPageObject : PageObject<TPageObject>
    {
        string componentFullName = UIComponentResolver.ResolvePageObjectFullName<TPageObject>();

        return report.Setup($"Set up {componentFullName}", function);
    }
}
