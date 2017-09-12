using OpenQA.Selenium.Remote;

namespace Atata
{
    /// <summary>
    /// Represents the driver factory.
    /// </summary>
    public interface IDriverFactory
    {
        /// <summary>
        /// Gets the alias.
        /// </summary>
        string Alias { get; }

        /// <summary>
        /// Creates the driver instance.
        /// </summary>
        /// <returns>The created <see cref="RemoteWebDriver"/> instance.</returns>
        RemoteWebDriver Create();
    }
}
