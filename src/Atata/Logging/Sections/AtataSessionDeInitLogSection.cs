#nullable enable

namespace Atata;

public sealed class AtataSessionDeInitLogSection : LogSection
{
    public AtataSessionDeInitLogSection(AtataSession session)
    {
        Message = $"Deinitialize {session}";
        Level = LogLevel.Trace;
    }
}
