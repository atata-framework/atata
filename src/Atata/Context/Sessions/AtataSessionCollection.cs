#nullable enable

namespace Atata;

public sealed class AtataSessionCollection : IReadOnlyCollection<AtataSession>, IDisposable
{
    private readonly AtataContext _context;

    private readonly List<IAtataSessionBuilder> _sessionBuilders = [];

    private readonly List<AtataSession> _sessionListOrderedByAdding = [];

    private readonly LinkedList<AtataSession> _sessionLinkedListOderedByCurrentUsage = [];

    private readonly ReaderWriterLockSlim _sessionLinkedListOderedByCurrentUsageLock = new();

    private AtataSessionPoolContainer? _poolContainer;

    internal AtataSessionCollection(AtataContext context) =>
        _context = context;

    public int Count =>
        _sessionListOrderedByAdding.Count;

    public AtataSession this[int index]
    {
        get
        {
            index.CheckIndexNonNegative();
            index.CheckIndexLessThanSize(_sessionListOrderedByAdding.Count);

            return _sessionListOrderedByAdding[index];
        }
    }

    public TSession Get<TSession>()
        where TSession : AtataSession
    {
        _sessionLinkedListOderedByCurrentUsageLock.EnterReadLock();

        try
        {
            return _sessionLinkedListOderedByCurrentUsage.OfType<TSession>().FirstOrDefault()
                ?? throw AtataSessionNotFoundException.For<TSession>();
        }
        finally
        {
            _sessionLinkedListOderedByCurrentUsageLock.ExitReadLock();
        }
    }

    /// <summary>
    /// Gets a session of <typeparamref name="TSession"/> type with the specified index
    /// among sessions of the <typeparamref name="TSession"/> type.
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    /// <param name="index">The index.</param>
    /// <returns>A session.</returns>
    public TSession Get<TSession>(int index)
       where TSession : AtataSession
    {
        index.CheckIndexNonNegative();

        return _sessionListOrderedByAdding.OfType<TSession>().ElementAtOrDefault(index)
            ?? throw AtataSessionNotFoundException.ByIndex<TSession>(
                index,
                _sessionListOrderedByAdding.OfType<TSession>().Count());
    }

    /// <summary>
    /// Gets a first session of <typeparamref name="TSession"/> type with the specified name
    /// among sessions of the <typeparamref name="TSession"/> type.
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    /// <param name="name">The name.</param>
    /// <returns>A session.</returns>
    public TSession Get<TSession>(string name)
        where TSession : AtataSession
        =>
        _sessionListOrderedByAdding.OfType<TSession>().FirstOrDefault(x => x.Name == name)
            ?? throw AtataSessionNotFoundException.ByName<TSession>(
                name,
                _sessionListOrderedByAdding.OfType<TSession>().Count());

    internal void AddBuilders(IEnumerable<IAtataSessionBuilder> sessionBuilders) =>
        _sessionBuilders.AddRange(sessionBuilders);

    public TSessionBuilder Add<TSessionBuilder>(Action<TSessionBuilder>? configure = null)
        where TSessionBuilder : IAtataSessionBuilder, new()
    {
        TSessionBuilder sessionBuilder = new()
        {
            TargetContext = _context
        };

        configure?.Invoke(sessionBuilder);

        _sessionBuilders.Add(sessionBuilder);

        return sessionBuilder;
    }

    public async ValueTask StartPoolAsync<TSessionBuilder>(Action<TSessionBuilder>? configure = null, CancellationToken cancellationToken = default)
        where TSessionBuilder : IAtataSessionBuilder, new()
    {
        TSessionBuilder sessionBuilder = Add(configure);

        await StartPoolAsync(sessionBuilder, cancellationToken)
            .ConfigureAwait(false);
    }

    internal async ValueTask StartPoolAsync(IAtataSessionBuilder sessionBuilder, CancellationToken cancellationToken = default)
    {
        ValidatePoolCapacitySettings(sessionBuilder.PoolInitialCapacity, sessionBuilder.PoolMaxCapacity);

        _poolContainer ??= new();
        AtataSessionPool pool = _poolContainer.AddPool(sessionBuilder);

        string poolAsString = BuildSessionPoolName(sessionBuilder);

        if (sessionBuilder.PoolInitialCapacity > 0)
        {
            await _context.Log.ExecuteSectionAsync(
                new LogSection($"Initialize {poolAsString}", LogLevel.Trace),
                async () => await pool.FillAsync(sessionBuilder.PoolInitialCapacity, cancellationToken)
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
        }
        else
        {
            _context.Log.Trace($"Initialized {poolAsString}");
        }
    }

    private static void ValidatePoolCapacitySettings(int initialCapacity, int maxCapacity)
    {
        if (maxCapacity < 1)
            throw new AtataSessionBuilderValidationException(
                $"Pool max capacity {maxCapacity} should be a positive value.");

        if (initialCapacity < 0)
            throw new AtataSessionBuilderValidationException(
                $"Pool initial capacity {initialCapacity} should be a non-negative value.");

        if (initialCapacity > maxCapacity)
            throw new AtataSessionBuilderValidationException(
                $"Pool initial capacity {initialCapacity} should not be greater than max capacity {maxCapacity}.");
    }

    private static string BuildSessionPoolName(IAtataSessionBuilder sessionBuilder)
    {
        var stringBuilder = new StringBuilder(AtataSessionTypeMap.ResolveSessionTypedName(sessionBuilder))
            .Append(" pool");

        bool renderInitialCapacity = sessionBuilder.PoolInitialCapacity != AtataSession.DefaultPoolInitialCapacity;
        bool renderMaxCapacity = sessionBuilder.PoolMaxCapacity != AtataSession.DefaultPoolMaxCapacity;

        if (renderInitialCapacity || renderMaxCapacity)
        {
            stringBuilder.Append(" { ");

            if (renderInitialCapacity)
                stringBuilder.Append($"InitialCapacity={sessionBuilder.PoolInitialCapacity}");

            if (renderMaxCapacity)
            {
                if (renderInitialCapacity)
                    stringBuilder.Append(", ");

                stringBuilder.Append($"MaxCapacity={sessionBuilder.PoolMaxCapacity}");
            }

            stringBuilder.Append(" }");
        }

        return stringBuilder.ToString();
    }

    public async Task<TSession> BuildAsync<TSession>(string? sessionName = null, CancellationToken cancellationToken = default)
        where TSession : AtataSession
        =>
        (TSession)await BuildAsync(typeof(TSession), sessionName, cancellationToken);

    public async Task<AtataSession> BuildAsync(Type sessionType, string? sessionName = null, CancellationToken cancellationToken = default)
    {
        var builder = _sessionBuilders.Find(x => x.Name == sessionName && AtataSessionTypeMap.ResolveSessionTypeByBuilderType(x.GetType()) == sessionType)
            ?? throw AtataSessionBuilderNotFoundException.BySessionType(sessionType, sessionName, _context);

        return await builder.BuildAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask<TSession> BorrowAsync<TSession>(string? sessionName = null, CancellationToken cancellationToken = default)
        where TSession : AtataSession
        =>
        (TSession)await BorrowAsync(typeof(TSession), sessionName, cancellationToken);

    public async ValueTask<AtataSession> BorrowAsync(Type sessionType, string? sessionName = null, CancellationToken cancellationToken = default)
    {
        AtataSession? session = FindSessionToBorrowInContextAncestors(sessionType, sessionName);

        if (session is null)
        {
            string sessionTypedName = AtataSession.BuildTypedName(sessionType, sessionName);
            throw new AtataSessionNotFoundException(
                $"Failed to find {sessionTypedName} to borrow for {_context}.");
        }

        if (!session.TryBorrowTo(_context))
        {
            _context.Log.Trace($"Waiting for {session} to borrow");

            RetryWait wait = new(session.SessionWaitingTimeout, session.SessionWaitingRetryInterval);

            bool borrowed = await wait.UntilAsync(
                x => x.TryBorrowTo(_context),
                session,
                cancellationToken)
                .ConfigureAwait(false);

            if (!borrowed)
                throw wait.CreateTimeoutExceptionFor($"{session} to borrow for {_context}.");
        }

        return session;
    }

    private AtataSession? FindSessionToBorrowInContextAncestors(Type sessionType, string? sessionName)
    {
        var currentContext = _context;

        while (currentContext.ParentContext is not null)
        {
            currentContext = currentContext.ParentContext;

            foreach (var session in currentContext.Sessions)
                if (session.IsShareable && session.Name == sessionName && session.GetType() == sessionType)
                    return session;
        }

        return null;
    }

    public async ValueTask<TSession> TakeFromPoolAsync<TSession>(string? sessionName = null, CancellationToken cancellationToken = default)
        where TSession : AtataSession
        =>
        (TSession)await TakeFromPoolAsync(typeof(TSession), sessionName, cancellationToken);

    public async ValueTask<AtataSession> TakeFromPoolAsync(Type sessionType, string? sessionName = null, CancellationToken cancellationToken = default)
    {
        if (!TryFindPool(sessionType, sessionName, out AtataSessionPool? pool))
            throw new AtataSessionPoolNotFoundException(
                $"Failed to find {AtataSession.BuildTypedName(sessionType, sessionName)} pool to take session for {_context}.");

        var sessionTask = pool.GetAsync(cancellationToken);

        if (!sessionTask.IsCompleted)
            _context.Log.Trace($"Waiting for {AtataSession.BuildTypedName(sessionType, sessionName)} to take from session pool");

        AtataSession session = await sessionTask.ConfigureAwait(false);

        session.GiveFromPoolToContext(_context);

        return session;
    }

    internal AtataSessionPool GetPool(Type sessionType, string sessionName)
    {
        if (_poolContainer is null || !_poolContainer.TryGetPool(sessionType, sessionName, out AtataSessionPool? pool))
            throw new AtataSessionPoolNotFoundException(
                $"Failed to find {AtataSession.BuildTypedName(sessionType, sessionName)} pool in {_context}.");

        return pool;
    }

    private bool TryFindPool(
        Type sessionType,
        string? sessionName,
        [NotNullWhen(true)] out AtataSessionPool? pool)
    {
        AtataContext currentContext = _context;

        do
        {
            if (currentContext.Sessions._poolContainer?.TryGetPool(sessionType, sessionName, out pool) ?? false)
                return true;

            currentContext = currentContext.ParentContext;
        }
        while (currentContext is not null);

        pool = null;
        return false;
    }

    internal void Add(AtataSession session)
    {
        _sessionLinkedListOderedByCurrentUsageLock.EnterWriteLock();

        try
        {
            _sessionListOrderedByAdding.Add(session);
            _sessionLinkedListOderedByCurrentUsage.AddLast(session);
        }
        finally
        {
            _sessionLinkedListOderedByCurrentUsageLock.ExitWriteLock();
        }
    }

    internal void Remove(AtataSession session)
    {
        _sessionLinkedListOderedByCurrentUsageLock.EnterWriteLock();

        try
        {
            _sessionListOrderedByAdding.Remove(session);
            _sessionLinkedListOderedByCurrentUsage.Remove(session);
        }
        finally
        {
            _sessionLinkedListOderedByCurrentUsageLock.ExitWriteLock();
        }
    }

    internal void SetCurrent(AtataSession session)
    {
        _sessionLinkedListOderedByCurrentUsageLock.EnterWriteLock();

        try
        {
            if (_sessionLinkedListOderedByCurrentUsage.First.Value != session)
            {
                var node = _sessionLinkedListOderedByCurrentUsage.Find(session);
                _sessionLinkedListOderedByCurrentUsage.Remove(node);
                _sessionLinkedListOderedByCurrentUsage.AddFirst(node);
            }
        }
        finally
        {
            _sessionLinkedListOderedByCurrentUsageLock.ExitWriteLock();
        }
    }

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public IEnumerator<AtataSession> GetEnumerator() =>
        _sessionListOrderedByAdding.GetEnumerator();

    public void Dispose()
    {
        _sessionLinkedListOderedByCurrentUsageLock.Dispose();

        _sessionBuilders.Clear();
        _sessionListOrderedByAdding.Clear();
        _sessionLinkedListOderedByCurrentUsage.Clear();
        _poolContainer?.Clear();
    }
}
