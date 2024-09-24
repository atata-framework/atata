namespace Atata;

public sealed class AtataSessionInitLogSection : LogSection
{
    public AtataSessionInitLogSection(AtataSession session)
    {
        Message = $"Initialize {session.GetType().Name}";
        Level = LogLevel.Trace;
    }
}
