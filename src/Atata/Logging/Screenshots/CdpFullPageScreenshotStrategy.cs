using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Edge;

namespace Atata;

/// <summary>
/// Represents a <see cref="WebDriverSession"/> screenshot strategy that takes a full-page screenshot using CDP.
/// Works only for <see cref="ChromeDriver"/> and <see cref="EdgeDriver"/>.
/// </summary>
public sealed class CdpFullPageScreenshotStrategy : IScreenshotStrategy<WebDriverSession>
{
    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static CdpFullPageScreenshotStrategy Instance { get; } =
        new CdpFullPageScreenshotStrategy();

    /// <inheritdoc/>
    public FileContentWithExtension TakeScreenshot(WebDriverSession session)
    {
        var devTools = session.Driver.As<IDevTools>();
        var devToolsSession = devTools.GetDevToolsSession();

        var commandParameters = new JObject
        {
            ["captureBeyondViewport"] = true
        };

        var commandResult = devToolsSession.SendCommand(
            "Page.captureScreenshot",
            commandParameters)
            .GetAwaiter()
            .GetResult();

        var data = commandResult["data"].ToString();
        return FileContentWithExtension.CreateFromBase64String(data, ".png");
    }
}
