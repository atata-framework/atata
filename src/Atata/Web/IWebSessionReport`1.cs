#nullable enable

namespace Atata;

/// <summary>
/// An interface of web session reporting functionality belonging to particular <typeparamref name="TOwner"/>.
/// </summary>
/// <typeparam name="TOwner">The type of the owner.</typeparam>
public interface IWebSessionReport<out TOwner> : IReport<TOwner>
{
    /// <inheritdoc cref="WebSession.TakeScreenshot(string)"/>
    /// <returns>The instance of the owner object.</returns>
    TOwner Screenshot(string? title = null);

    /// <inheritdoc cref="WebSession.TakeScreenshot(ScreenshotKind, string)"/>
    /// <returns>The instance of the owner object.</returns>
    TOwner Screenshot(ScreenshotKind kind, string? title = null);

    /// <inheritdoc cref="WebSession.TakePageSnapshot(string)"/>
    /// <returns>The instance of the owner object.</returns>
    TOwner PageSnapshot(string? title = null);
}
