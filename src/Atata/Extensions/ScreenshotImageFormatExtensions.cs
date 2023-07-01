namespace Atata;

public static class ScreenshotImageFormatExtensions
{
    public static string GetExtension(this ScreenshotImageFormat format) =>
        format == ScreenshotImageFormat.Jpeg
            ? ".jpg"
            : format.ToString().ToLowerInvariant().Prepend(".");
}
