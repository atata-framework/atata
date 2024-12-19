namespace Atata;

internal sealed class AtataSessionPool
{
    private readonly IAtataSessionBuilder _sessionBuilder;

    private readonly int _maxCapacity;

    private readonly ConcurrentQueue<AtataSession> _items = [];

    private int _count;

    internal AtataSessionPool(IAtataSessionBuilder sessionBuilder, int maxCapacity)
    {
        _sessionBuilder = sessionBuilder;
        _maxCapacity = maxCapacity;
    }

    internal int MaxCapacity =>
        _maxCapacity;

    internal int QueuedCount =>
        _items.Count;

    internal async ValueTask<AtataSession> GetAsync(CancellationToken cancellationToken = default)
    {
        AtataSession session;

        if (!_items.TryDequeue(out session))
        {
            session = await Task.Run(
                () => TryBuildOrWaitAsync(cancellationToken),
                cancellationToken)
                .ConfigureAwait(false);
        }

        session.IsTakenFromPool = true;
        return session;
    }

    internal async Task FillAsync(int count, CancellationToken cancellationToken = default)
    {
        count.CheckGreaterOrEqual(nameof(count), 1);
        count.CheckLessOrEqual(nameof(count), MaxCapacity - _count);

        for (int i = 0; i < count; i++)
        {
            var session = await TryBuildAsync(cancellationToken)
                .ConfigureAwait(false);

            _items.Enqueue(session);

            if (session is null)
                throw new InvalidOperationException(
                    $"Cannot build {AtataSessionTypeMap.ResolveSessionTypedName(_sessionBuilder)} within a session pool. Max capacity {MaxCapacity} is reached.");
        }
    }

    internal void Return(AtataSession session)
    {
        session.CheckNotNull(nameof(session));

        if (session.OwnerContext != _sessionBuilder.TargetContext)
            throw new InvalidOperationException($"{session} cannot be returned to pool as it doesn't belong to this pool.");

        if (!session.IsTakenFromPool)
            throw new InvalidOperationException($"{session} cannot be returned to pool as it is already there.");

        session.IsTakenFromPool = false;

        _items.Enqueue(session);
    }

    private async Task<AtataSession> TryBuildOrWaitAsync(CancellationToken cancellationToken)
    {
        AtataSession session = await TryBuildAsync(cancellationToken)
            .ConfigureAwait(false);

        if (session is null)
        {
            RetryWait wait = new(_sessionBuilder.SessionWaitingTimeout, _sessionBuilder.SessionWaitingRetryInterval);

            bool succeeded = await wait.UntilAsync(
                x => x.TryDequeue(out session),
                _items,
                cancellationToken)
                .ConfigureAwait(false);

            if (!succeeded)
                throw wait.CreateTimeoutExceptionFor(
                    $"{AtataSessionTypeMap.ResolveSessionTypedName(_sessionBuilder)} from a session pool");
        }

        return session;
    }

    private async Task<AtataSession> TryBuildAsync(CancellationToken cancellationToken = default)
    {
        if (_count < _maxCapacity)
        {
            Interlocked.Increment(ref _count);

            if (_count <= _maxCapacity)
            {
                try
                {
                    return await _sessionBuilder.BuildAsync(cancellationToken)
                        .ConfigureAwait(false);
                }
                catch
                {
                    Interlocked.Decrement(ref _count);
                    throw;
                }
            }
        }

        return null;
    }
}
