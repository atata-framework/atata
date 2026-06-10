namespace Atata;

internal sealed class AtataSessionWithScopedCachingExecutionUnit : AtataSessionExecutionUnit, ISupportsScopedCaching
{
    private readonly ISupportsScopedCaching _sessionSupportingScopedCaching;

    public AtataSessionWithScopedCachingExecutionUnit(AtataSession session)
        : base(session) =>
        _sessionSupportingScopedCaching = (ISupportsScopedCaching)session;

    public void ExecuteScopedBlock(Action action) =>
        _sessionSupportingScopedCaching.ExecuteScopedBlock(action);

    public TResult ExecuteScopedBlock<TResult>(Func<TResult> function) =>
        _sessionSupportingScopedCaching.ExecuteScopedBlock(function);
}
