namespace Atata;

internal static class WebDriverArtifactFileUtils
{
    private static readonly char[] s_charactersToTrimInFileName = [' ', '_', '-'];

    internal static string SanitizeFileName(string filePath)
    {
        string fileName = Path.GetFileName(filePath);
        string directoryPath = filePath[..(filePath.Length - fileName.Length)];

        fileName = fileName.Trim(s_charactersToTrimInFileName)
            .Replace(' ', '_');

        return directoryPath.Length == 0
            ? fileName
            : directoryPath + fileName;
    }
}
