using OpenQA.Selenium.Firefox;

namespace Atata;

/// <summary>
/// Represents the strategy that takes a full-page screenshot.
/// Works only for <see cref="FirefoxDriver"/>, invoking its <see cref="FirefoxDriver.GetFullPageScreenshot"/> method.
/// </summary>
public sealed class WebDriverFullPageScreenshotStrategy : IScreenshotStrategy
{
    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static WebDriverFullPageScreenshotStrategy Instance { get; } =
        new WebDriverFullPageScreenshotStrategy();

    /// <inheritdoc/>
    public FileContentWithExtension TakeScreenshot(WebDriverSession session)
    {
        if (!session.Driver.TryAs(out FirefoxDriver firefoxDriver))
            throw new InvalidOperationException(
                $"{GetType().FullName} works only with Driver of {typeof(FirefoxDriver).FullName} type, but was {session.Driver.GetType().FullName}.");

        Screenshot screenshot = firefoxDriver.GetFullPageScreenshot();
        return FileContentWithExtension.CreateFromByteArray(screenshot.AsByteArray, ".png");
    }
}
