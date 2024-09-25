namespace Atata;

public sealed class AtataSessionDeInitLogSection : LogSection
{
    public AtataSessionDeInitLogSection(AtataSession session)
    {
        Message = $"Deinitialize {session.GetType().Name}";
        Level = LogLevel.Trace;
    }
}
