namespace Atata;

internal sealed class AtataSessionExecutionUnit : IAtataExecutionUnit
{
    private readonly AtataSession _session;

    internal AtataSessionExecutionUnit(AtataSession session) =>
        _session = session;

    public AtataContext Context =>
        EnsureIsActive().Context;

    public ILogManager Log =>
        EnsureIsActive().Log;

    public bool IsActive =>
        _session.IsActive;

    private AtataSession EnsureIsActive() =>
        _session.IsActive
            ? _session
            : throw new InvalidOperationException(
                $"The {_session.GetType().Name} with Id={_session.Id} is already not active or disposed.");
}
