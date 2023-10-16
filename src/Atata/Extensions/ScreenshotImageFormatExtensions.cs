namespace Atata;

// TODO: Atata v3. Remove class.
public static class ScreenshotImageFormatExtensions
{
    [Obsolete("Don't use this method as it will be removed.")] // Obsolete since v2.11.0.
    public static string GetExtension(this ScreenshotImageFormat format) =>
        format == ScreenshotImageFormat.Jpeg
            ? ".jpg"
            : format.ToString().ToLowerInvariant().Prepend(".");
}
