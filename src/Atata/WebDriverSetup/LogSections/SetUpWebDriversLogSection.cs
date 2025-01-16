#nullable enable

namespace Atata;

public sealed class SetUpWebDriversLogSection : LogSection
{
    internal SetUpWebDriversLogSection(IReadOnlyList<string> browserNames)
    {
        Message = browserNames.Count == 1
            ? $"Set up web driver for {browserNames[0]}"
            : $"Set up web drivers for: {string.Join(", ", browserNames)}";

        Level = LogLevel.Trace;
    }
}
