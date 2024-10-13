namespace Atata;

public sealed class AtataSessionInitLogSection : LogSection
{
    public AtataSessionInitLogSection(AtataSession session)
    {
        Message = $"Initialize {session}";
        Level = LogLevel.Trace;
    }
}
