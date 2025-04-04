namespace Atata;

/// <summary>
/// Provides a set of Angular settings.
/// </summary>
public static class AngularSettings
{
    /// <summary>
    /// Gets or sets the default Angular root selector.
    /// The default value is <c>"[ng-app]"</c>.
    /// </summary>
    public static string RootSelector { get; set; } = "[ng-app]";
}
