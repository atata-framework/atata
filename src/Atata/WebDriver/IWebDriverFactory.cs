namespace Atata;

/// <summary>
/// Represents the factory of <see cref="IWebDriver"/>.
/// </summary>
public interface IWebDriverFactory
{
    /// <summary>
    /// Gets the driver alias.
    /// </summary>
    string? Alias { get; }

    /// <summary>
    /// Creates the driver instance.
    /// </summary>
    /// <param name="logManager">The log manager.</param>
    /// <returns>The created <see cref="IWebDriver" /> instance.</returns>
    IWebDriver Create(ILogManager logManager);
}
