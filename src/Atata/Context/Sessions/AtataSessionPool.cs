namespace Atata;

internal sealed class AtataSessionPool : IEnumerable<AtataSession>
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
        if (!_items.TryDequeue(out AtataSession session))
        {
            session = await Task.Run(
                () => TryBuildOrWaitAsync(cancellationToken),
                cancellationToken)
                .ConfigureAwait(false);
        }

        session.IsTakenFromPool = true;
        return session;
    }

    internal async Task FillAsync(int count, bool inParallel = true, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfLessThan(count, 1);
        Guard.ThrowIfGreaterThan(count, MaxCapacity - _count);

        if (inParallel && count > 1)
        {
            await FillInParallelAsync(count, cancellationToken)
                .ConfigureAwait(false);
        }
        else
        {
            await FillSequentiallyAsync(count, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private async Task FillInParallelAsync(int count, CancellationToken cancellationToken)
    {
        var buildTasks = new Task<AtataSession?>[count];

        for (int i = 0; i < count; i++)
        {
            buildTasks[i] = Task.Run(
                () => TryBuildAsync(cancellationToken),
                cancellationToken);
        }

        AtataSession?[] sessions = await Task.WhenAll(buildTasks).ConfigureAwait(false);

        foreach (var session in sessions)
        {
            if (session is null)
                throw CreateExceptionForMaxCapacityReached();

            _items.Enqueue(session);
        }
    }

    private async Task FillSequentiallyAsync(int count, CancellationToken cancellationToken)
    {
        for (int i = 0; i < count; i++)
        {
            AtataSession? session = await TryBuildAsync(cancellationToken)
                .ConfigureAwait(false);

            if (session is null)
                throw CreateExceptionForMaxCapacityReached();

            _items.Enqueue(session);
        }
    }

    private InvalidOperationException CreateExceptionForMaxCapacityReached() =>
        new($"Cannot build {AtataSessionTypeMap.ResolveSessionTypedName(_sessionBuilder)} within a session pool. Max capacity {MaxCapacity} is reached.");

    internal void Return(AtataSession session)
    {
        Guard.ThrowIfNull(session);

        if (session.OwnerContext != _sessionBuilder.TargetContext)
            throw new InvalidOperationException($"{session} cannot be returned to pool as it doesn't belong to this pool.");

        if (!session.IsTakenFromPool)
            throw new InvalidOperationException($"{session} cannot be returned to pool as it is already there.");

        session.IsTakenFromPool = false;
        session.IsShareable = false;

        _items.Enqueue(session);
    }

    private async Task<AtataSession> TryBuildOrWaitAsync(CancellationToken cancellationToken)
    {
        AtataSession? session = await TryBuildAsync(cancellationToken)
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

        return session!;
    }

    private async Task<AtataSession?> TryBuildAsync(CancellationToken cancellationToken)
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

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public IEnumerator<AtataSession> GetEnumerator()
    {
        foreach (var item in _items)
            yield return item;
    }
}
