namespace Atata.IntegrationTests;

internal static class Platforms
{
    private const string Delimeter = ",";

    public const string Windows = "Win";

    public const string Linux = "Linux";

    public const string MacOS = "MacOsX";

    public const string WindowsAndLinux = Windows + Delimeter + Linux;
}
