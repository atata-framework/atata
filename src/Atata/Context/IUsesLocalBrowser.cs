namespace Atata;

/// <summary>
/// Defines the need to use a local browser.
/// </summary>
public interface IUsesLocalBrowser
{
    /// <summary>
    /// Gets the name of the browser.
    /// </summary>
    string BrowserName { get; }
}
