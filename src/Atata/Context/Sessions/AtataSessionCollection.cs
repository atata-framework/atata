namespace Atata;

/// <summary>
/// A collection of <see cref="AtataSession"/> items associated with a certain <see cref="AtataContext"/>.
/// </summary>
public sealed class AtataSessionCollection : IReadOnlyList<AtataSession>, IDisposable
{
    private readonly AtataContext _context;

    private readonly List<IAtataSessionBuilder> _sessionBuilders = [];

    private readonly List<AtataSession> _sessionListOrderedByAdding = [];

    private readonly LinkedList<AtataSession> _sessionLinkedListOderedByCurrentUsage = [];

    private readonly ReaderWriterLockSlim _sessionLinkedListOderedByCurrentUsageLock = new();

    private AtataSessionPoolContainer? _poolContainer;

    private bool _isDisposed;

    internal AtataSessionCollection(AtataContext context) =>
        _context = context;

    /// <summary>
    /// Gets the number of sessions in this collection.
    /// </summary>
    public int Count =>
        _sessionListOrderedByAdding.Count;

    /// <summary>
    /// Gets the collection of <see cref="IAtataSessionBuilder"/>.
    /// </summary>
    public IReadOnlyList<IAtataSessionBuilder> Builders =>
        _sessionBuilders;

    /// <summary>
    /// Gets the <see cref="AtataSession"/> at the specified index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>A session.</returns>
    public AtataSession this[int index]
    {
        get
        {
            Guard.ThrowIfIndexIsNegativeOrGreaterOrEqualTo(index, _sessionListOrderedByAdding.Count);

            return _sessionListOrderedByAdding[index];
        }
    }

    /// <summary>
    /// Gets a session of <typeparamref name="TSession"/> type despite the name in this collection.
    /// In case of multiple sessions of the <typeparamref name="TSession"/> type,
    /// returns the one that was used/added last.
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    /// <returns>A session.</returns>
    public TSession Get<TSession>()
        where TSession : AtataSession
        =>
        GetOrNull<TSession>()
            ?? throw AtataSessionNotFoundException.For<TSession>(_context);

    /// <summary>
    /// Gets a session of <typeparamref name="TSession"/> type with the specified index
    /// among sessions in this collection of the <typeparamref name="TSession"/> type ordered by adding.
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    /// <param name="index">The index.</param>
    /// <returns>A session.</returns>
    public TSession Get<TSession>(int index)
       where TSession : AtataSession
    {
        Guard.ThrowIfIndexIsNegative(index);

        return _sessionListOrderedByAdding.OfType<TSession>().ElementAtOrDefault(index)
            ?? throw AtataSessionNotFoundException.ByIndex<TSession>(
                index,
                _sessionListOrderedByAdding.OfType<TSession>().Count(),
                _context);
    }

    /// <summary>
    /// Gets a session of <typeparamref name="TSession"/> type with the specified name in this collection.
    /// In case of multiple sessions of the <typeparamref name="TSession"/> type and the name,
    /// returns the one that was used/added last.
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    /// <param name="name">The name.</param>
    /// <returns>A session.</returns>
    public TSession Get<TSession>(string? name)
        where TSession : AtataSession
        =>
        GetOrNull<TSession>(name)
            ?? throw AtataSessionNotFoundException.ByName<TSession>(
                name,
                _sessionListOrderedByAdding.OfType<TSession>().Count(),
                _context);

    /// <summary>
    /// Gets a session of <typeparamref name="TSession"/> type despite the name in this and ancestral context collections.
    /// In case of multiple sessions of the <typeparamref name="TSession"/> type,
    /// returns the one that was used/added last.
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    /// <returns>A session.</returns>
    public TSession GetRecursively<TSession>()
        where TSession : AtataSession
    {
        EnsureNotDisposed();

        return GetOrNullRecursively<TSession>()
            ?? throw AtataSessionNotFoundException.For<TSession>(_context, recursively: true);
    }

    /// <summary>
    /// Gets a session of <typeparamref name="TSession"/> type with the specified name in this and ancestral context collections.
    /// In case of multiple sessions of the <typeparamref name="TSession"/> type and the name,
    /// returns the one that was used/added last.
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    /// <param name="name">The name.</param>
    /// <returns>A session.</returns>
    public TSession GetRecursively<TSession>(string? name)
        where TSession : AtataSession
    {
        EnsureNotDisposed();

        return GetOrNullRecursively<TSession>(name)
            ?? throw AtataSessionNotFoundException.ByName<TSession>(
            name,
            CountRecursively<TSession>(),
            _context,
            recursively: true);
    }

    /// <summary>
    /// Tries to get a session of <typeparamref name="TSession"/> type despite the name in this collection.
    /// In case of multiple sessions of the <typeparamref name="TSession"/> type,
    /// returns the one that was used/added last.
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    /// <param name="session">The session.</param>
    /// <returns><see langword="true"/> if session is found; otherwise, <see langword="false"/>.</returns>
    public bool TryGet<TSession>([NotNullWhen(true)] out TSession? session)
        where TSession : AtataSession
    {
        session = GetOrNull<TSession>();
        return session is not null;
    }

    /// <summary>
    /// Tries to get a session of <typeparamref name="TSession"/> type with the specified name in this collection.
    /// In case of multiple sessions of the <typeparamref name="TSession"/> type and the name,
    /// returns the one that was used/added last.
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    /// <param name="name">The name.</param>
    /// <param name="session">The session.</param>
    /// <returns><see langword="true"/> if session is found; otherwise, <see langword="false"/>.</returns>
    public bool TryGet<TSession>(string? name, [NotNullWhen(true)] out TSession? session)
        where TSession : AtataSession
    {
        session = GetOrNull<TSession>(name);
        return session is not null;
    }

    /// <summary>
    /// Tries to get a session of <typeparamref name="TSession"/> type despite the name in this and ancestral context collections.
    /// In case of multiple sessions of the <typeparamref name="TSession"/> type,
    /// returns the one that was used/added last.
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    /// <param name="session">The session.</param>
    /// <returns><see langword="true"/> if session is found; otherwise, <see langword="false"/>.</returns>
    public bool TryGetRecursively<TSession>([NotNullWhen(true)] out TSession? session)
        where TSession : AtataSession
    {
        session = GetOrNullRecursively<TSession>();
        return session is not null;
    }

    /// <summary>
    /// Tries to get a session of <typeparamref name="TSession"/> type with the specified name in this and ancestral context collections.
    /// In case of multiple sessions of the <typeparamref name="TSession"/> type and the name,
    /// returns the one that was used/added last.
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    /// <param name="name">The name.</param>
    /// <param name="session">The session.</param>
    /// <returns><see langword="true"/> if session is found; otherwise, <see langword="false"/>.</returns>
    public bool TryGetRecursively<TSession>(string? name, [NotNullWhen(true)] out TSession? session)
        where TSession : AtataSession
    {
        session = GetOrNullRecursively<TSession>(name);
        return session is not null;
    }

    /// <summary>
    /// Determines whether a session of <typeparamref name="TSession"/> type despite the name is in this collection.
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    /// <returns><see langword="true"/> if session is found; otherwise, <see langword="false"/>.</returns>
    public bool Contains<TSession>()
        where TSession : AtataSession
        =>
        GetOrNull<TSession>() is not null;

    /// <summary>
    /// Determines whether a session of <typeparamref name="TSession"/> type with the specified name is in this collection.
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    /// <param name="name">The name.</param>
    /// <returns><see langword="true"/> if session is found; otherwise, <see langword="false"/>.</returns>
    public bool Contains<TSession>(string? name)
        where TSession : AtataSession
        =>
        GetOrNull<TSession>(name) is not null;

    /// <summary>
    /// Determines whether a session of <typeparamref name="TSession"/> type despite the name is in this and ancestral context collections.
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    /// <returns><see langword="true"/> if session is found; otherwise, <see langword="false"/>.</returns>
    public bool ContainsRecursively<TSession>()
        where TSession : AtataSession
        =>
        GetOrNullRecursively<TSession>() is not null;

    /// <summary>
    /// Determines whether a session of <typeparamref name="TSession"/> type with the specified name is in this and ancestral context collections.
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    /// <param name="name">The name.</param>
    /// <returns><see langword="true"/> if session is found; otherwise, <see langword="false"/>.</returns>
    public bool ContainsRecursively<TSession>(string? name)
        where TSession : AtataSession
        =>
        GetOrNullRecursively<TSession>(name) is not null;

    internal void AddBuilders(IEnumerable<IAtataSessionBuilder> sessionBuilders) =>
        _sessionBuilders.AddRange(sessionBuilders);

    /// <summary>
    /// Creates a new session builder of the specified <typeparamref name="TSessionBuilder"/> type
    /// and adds it to the collection.
    /// </summary>
    /// <typeparam name="TSessionBuilder">The type of the session builder to add.</typeparam>
    /// <param name="configure">An action delegate to configure the <typeparamref name="TSessionBuilder"/>.</param>
    /// <returns>The created <typeparamref name="TSessionBuilder"/> instance.</returns>
    public TSessionBuilder Add<TSessionBuilder>(Action<TSessionBuilder>? configure = null)
        where TSessionBuilder : IAtataSessionBuilder, new()
    {
        EnsureNotDisposed();

        TSessionBuilder sessionBuilder = new()
        {
            TargetContext = _context
        };

        configure?.Invoke(sessionBuilder);

        _sessionBuilders.Add(sessionBuilder);

        return sessionBuilder;
    }

    /// <summary>
    /// Configures existing <typeparamref name="TSessionBuilder"/> session builders.
    /// </summary>
    /// <typeparam name="TSessionBuilder">The type of the session builder to configure.</typeparam>
    /// <param name="configure">An action delegate to configure the <typeparamref name="TSessionBuilder"/>.</param>
    public void ConfigureAllBuilders<TSessionBuilder>(Action<TSessionBuilder> configure)
        where TSessionBuilder : IAtataSessionBuilder
    {
        EnsureNotDisposed();

        Guard.ThrowIfNull(configure);

        foreach (var sessionBuilder in _sessionBuilders.OfType<TSessionBuilder>())
            configure.Invoke(sessionBuilder);
    }

    /// <summary>
    /// Configures existing nameless <typeparamref name="TSessionBuilder"/> session builder.
    /// In case of multiple session builders matching, configures the first one.
    /// In case of missing session builder, throws <see cref="AtataSessionBuilderNotFoundException"/>.
    /// </summary>
    /// <typeparam name="TSessionBuilder">The type of the session builder to configure.</typeparam>
    /// <param name="configure">An action delegate to configure the <typeparamref name="TSessionBuilder"/>.</param>
    /// <returns>The existing <typeparamref name="TSessionBuilder"/> instance.</returns>
    public TSessionBuilder ConfigureBuilder<TSessionBuilder>(Action<TSessionBuilder>? configure = null)
        where TSessionBuilder : IAtataSessionBuilder
        =>
        ConfigureBuilder(null, configure);

    /// <summary>
    /// Configures existing <typeparamref name="TSessionBuilder"/> session builder that has the specified <paramref name="sessionName"/>.
    /// In case of multiple session builders matching, configures the first one.
    /// In case of missing session builder, throws <see cref="AtataSessionBuilderNotFoundException"/>.
    /// </summary>
    /// <typeparam name="TSessionBuilder">The type of the session builder to configure.</typeparam>
    /// <param name="sessionName">The name of the session.</param>
    /// <param name="configure">An action delegate to configure the <typeparamref name="TSessionBuilder"/>.</param>
    /// <returns>The existing <typeparamref name="TSessionBuilder"/> instance.</returns>
    public TSessionBuilder ConfigureBuilder<TSessionBuilder>(string? sessionName, Action<TSessionBuilder>? configure = null)
        where TSessionBuilder : IAtataSessionBuilder
    {
        EnsureNotDisposed();

        var sessionBuilder = _sessionBuilders.OfType<TSessionBuilder>().FirstOrDefault(x => x.Name == sessionName)
           ?? throw AtataSessionBuilderNotFoundException.ByBuilderType(typeof(TSessionBuilder), sessionName, _context);

        configure?.Invoke(sessionBuilder);

        return sessionBuilder;
    }

    /// <summary>
    /// Creates a copy for configuration of existing nameless <typeparamref name="TSessionBuilder"/> session builder.
    /// In case of multiple session builders matching, takes the first one.
    /// In case of missing session builder, throws <see cref="AtataSessionBuilderNotFoundException"/>.
    /// </summary>
    /// <typeparam name="TSessionBuilder">The type of the session builder to configure.</typeparam>
    /// <param name="configure">An action delegate to configure the <typeparamref name="TSessionBuilder"/>.</param>
    /// <returns>The existing <typeparamref name="TSessionBuilder"/> instance.</returns>
    public TSessionBuilder ConfigureBuilderCopy<TSessionBuilder>(Action<TSessionBuilder>? configure = null)
        where TSessionBuilder : IAtataSessionBuilder
        =>
        ConfigureBuilderCopy(null, configure);

    /// <summary>
    /// Creates a copy for configuration of existing <typeparamref name="TSessionBuilder"/> session builder that has the specified <paramref name="sessionName"/>.
    /// In case of multiple session builders matching, takes the first one.
    /// In case of missing session builder, throws <see cref="AtataSessionBuilderNotFoundException"/>.
    /// </summary>
    /// <typeparam name="TSessionBuilder">The type of the session builder to configure.</typeparam>
    /// <param name="sessionName">The name of the session.</param>
    /// <param name="configure">An action delegate to configure the <typeparamref name="TSessionBuilder"/>.</param>
    /// <returns>The existing <typeparamref name="TSessionBuilder"/> instance.</returns>
    public TSessionBuilder ConfigureBuilderCopy<TSessionBuilder>(string? sessionName, Action<TSessionBuilder>? configure = null)
        where TSessionBuilder : IAtataSessionBuilder
    {
        EnsureNotDisposed();

        var existingSessionBuilder = _sessionBuilders.OfType<TSessionBuilder>().FirstOrDefault(x => x.Name == sessionName)
           ?? throw AtataSessionBuilderNotFoundException.ByBuilderType(typeof(TSessionBuilder), sessionName, _context);

        var newSessionBuilder = (TSessionBuilder)existingSessionBuilder.Clone();
        newSessionBuilder.TargetContext = _context;

        configure?.Invoke(newSessionBuilder);

        _sessionBuilders.Add(newSessionBuilder);

        return newSessionBuilder;
    }

    /// <summary>
    /// Starts a pool of sessions of the specified <typeparamref name="TSessionBuilder"/> type.
    /// </summary>
    /// <typeparam name="TSessionBuilder">The type of the session.</typeparam>
    /// <param name="configure">An action delegate to configure the <typeparamref name="TSessionBuilder"/>.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="ValueTask"/> object.</returns>
    public async ValueTask StartPoolAsync<TSessionBuilder>(Action<TSessionBuilder>? configure = null, CancellationToken cancellationToken = default)
        where TSessionBuilder : IAtataSessionBuilder, new()
    {
        _context.SetToDefaultCancellationTokenWhenDefault(ref cancellationToken);

        TSessionBuilder sessionBuilder = Add(configure);

        await StartPoolAsync(sessionBuilder, cancellationToken)
            .ConfigureAwait(false);
    }

    internal async ValueTask StartPoolAsync(IAtataSessionBuilder sessionBuilder, CancellationToken cancellationToken)
    {
        EnsureNotDisposed();

        ValidatePoolCapacitySettings(sessionBuilder.PoolInitialCapacity, sessionBuilder.PoolMaxCapacity);

        _poolContainer ??= new();
        AtataSessionPool pool = _poolContainer.AddPool(sessionBuilder);

        string poolAsString = BuildSessionPoolName(sessionBuilder);

        if (sessionBuilder.PoolInitialCapacity > 0)
        {
            await _context.Log.ExecuteSectionAsync(
                new LogSection($"Initialize {poolAsString}", LogLevel.Trace),
                async () => await pool.FillAsync(sessionBuilder.PoolInitialCapacity, sessionBuilder.StartMultipleInParallel, cancellationToken)
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
        StringBuilder stringBuilder = new("pool");

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

        stringBuilder.Append(" of ")
            .Append(AtataSessionTypeMap.ResolveSessionTypedName(sessionBuilder));

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Builds a session of the specified <typeparamref name="TSession"/> type and <paramref name="sessionName"/>.
    /// </summary>
    /// <typeparam name="TSession">The type of the session.</typeparam>
    /// <param name="sessionName">The name of the session.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="ValueTask{TResult}"/> of <typeparamref name="TSession"/> object.</returns>
    public async Task<TSession> BuildAsync<TSession>(string? sessionName = null, CancellationToken cancellationToken = default)
        where TSession : AtataSession
        =>
        (TSession)await BuildAsync(typeof(TSession), sessionName, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Builds a session of the specified <paramref name="sessionName"/>.
    /// </summary>
    /// <param name="sessionName">The name of the session.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="ValueTask{TResult}"/> of <see cref="AtataSession"/> object.</returns>
    public async Task<AtataSession> BuildAsync(string sessionName, CancellationToken cancellationToken = default)
    {
        EnsureNotDisposed();

        var builder = _sessionBuilders.Find(x => x.Name == sessionName)
            ?? throw AtataSessionBuilderNotFoundException.BySessionTypeAndName(null, sessionName, _context);

        return await builder.BuildAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Builds a session of the specified <paramref name="sessionType"/> and <paramref name="sessionName"/>.
    /// </summary>
    /// <param name="sessionType">The type of the session.</param>
    /// <param name="sessionName">The name of the session.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="ValueTask{TResult}"/> of <see cref="AtataSession"/> object.</returns>
    public async Task<AtataSession> BuildAsync(Type sessionType, string? sessionName = null, CancellationToken cancellationToken = default)
    {
        EnsureNotDisposed();

        var builder = _sessionBuilders.Find(x => x.Name == sessionName && sessionType.IsAssignableFrom(AtataSessionTypeMap.ResolveSessionTypeByBuilderType(x.GetType())))
            ?? throw AtataSessionBuilderNotFoundException.BySessionTypeAndName(sessionType, sessionName, _context);

        return await builder.BuildAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Borrows a session of the specified <typeparamref name="TSession"/> type with the specified <paramref name="sessionName"/>.
    /// </summary>
    /// <typeparam name="TSession">The type of the session to borrow.</typeparam>
    /// <param name="sessionName">The name of the session.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="ValueTask{TResult}"/> of <typeparamref name="TSession"/> object.</returns>
    public async ValueTask<TSession> BorrowAsync<TSession>(string? sessionName = null, CancellationToken cancellationToken = default)
        where TSession : AtataSession
        =>
        (TSession)await BorrowAsync(typeof(TSession), sessionName, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Borrows a session of the specified <paramref name="sessionType"/> with the specified <paramref name="sessionName"/>.
    /// </summary>
    /// <param name="sessionType">The type of the session to borrow.</param>
    /// <param name="sessionName">The name of the session.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="ValueTask{TResult}"/> of <see cref="AtataSession"/> object.</returns>
    public async ValueTask<AtataSession> BorrowAsync(Type sessionType, string? sessionName = null, CancellationToken cancellationToken = default)
    {
        EnsureNotDisposed();

        _context.SetToDefaultCancellationTokenWhenDefault(ref cancellationToken);

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
                if (session.IsShareable && session.Name == sessionName && sessionType.IsInstanceOfType(session))
                    return session;
        }

        return null;
    }

    /// <summary>
    /// Takes a session from the pool of the specified <typeparamref name="TSession"/> type with the specified <paramref name="sessionName"/>.
    /// </summary>
    /// <typeparam name="TSession">The type of the session to take from the pool.</typeparam>
    /// <param name="sessionName">The name of the session.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="ValueTask{TResult}"/> of <typeparamref name="TSession"/> object.</returns>
    public async ValueTask<TSession> TakeFromPoolAsync<TSession>(string? sessionName = null, CancellationToken cancellationToken = default)
        where TSession : AtataSession
        =>
        (TSession)await TakeFromPoolAsync(typeof(TSession), sessionName, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Takes a session from the pool of the specified <paramref name="sessionType"/> with the specified <paramref name="sessionName"/>.
    /// </summary>
    /// <param name="sessionType">The type of the session to take from the pool.</param>
    /// <param name="sessionName">The name of the session.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="ValueTask{TResult}"/> of <see cref="AtataSession"/> object.</returns>
    public async ValueTask<AtataSession> TakeFromPoolAsync(Type sessionType, string? sessionName = null, CancellationToken cancellationToken = default)
    {
        EnsureNotDisposed();

        _context.SetToDefaultCancellationTokenWhenDefault(ref cancellationToken);

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

    internal AtataSessionPool GetPool(Type sessionType, string? sessionName)
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
        AtataContext? currentContext = _context;

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
        EnsureNotDisposed();
        _sessionLinkedListOderedByCurrentUsageLock.EnterWriteLock();

        try
        {
            _sessionListOrderedByAdding.Add(session);
            _sessionLinkedListOderedByCurrentUsage.AddFirst(session);
        }
        finally
        {
            _sessionLinkedListOderedByCurrentUsageLock.ExitWriteLock();
        }
    }

    internal void Remove(AtataSession session)
    {
        EnsureNotDisposed();
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
        EnsureNotDisposed();
        _sessionLinkedListOderedByCurrentUsageLock.EnterWriteLock();

        try
        {
            LinkedListNode<AtataSession>? firstNode = _sessionLinkedListOderedByCurrentUsage.First;

            if (firstNode is not null && firstNode.Value != session)
            {
                LinkedListNode<AtataSession>? targetNode = _sessionLinkedListOderedByCurrentUsage.Find(session);

                if (targetNode is not null)
                {
                    _sessionLinkedListOderedByCurrentUsage.Remove(targetNode);
                    _sessionLinkedListOderedByCurrentUsage.AddFirst(targetNode);
                }
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

        _isDisposed = true;
    }

    internal IEnumerable<AtataSession> GetAllIncludingPooled()
    {
        foreach (AtataSession session in _sessionListOrderedByAdding)
            yield return session;

        if (_poolContainer is not null)
        {
            foreach (AtataSessionPool pool in _poolContainer)
                foreach (AtataSession session in pool)
                    if (!_sessionListOrderedByAdding.Contains(session))
                        yield return session;
        }
    }

    /// <inheritdoc cref="Get{TSession}()"/>
    /// <returns>A session or <see langword="null"/>, if such session is not found.</returns>
    public TSession? GetOrNull<TSession>()
        where TSession : AtataSession
    {
        _sessionLinkedListOderedByCurrentUsageLock.EnterReadLock();

        try
        {
            return _sessionLinkedListOderedByCurrentUsage.OfType<TSession>().FirstOrDefault();
        }
        finally
        {
            _sessionLinkedListOderedByCurrentUsageLock.ExitReadLock();
        }
    }

    /// <inheritdoc cref="Get{TSession}(string?)"/>
    /// <returns>A session or <see langword="null"/>, if such session is not found.</returns>
    public TSession? GetOrNull<TSession>(string? name)
        where TSession : AtataSession
    {
        _sessionLinkedListOderedByCurrentUsageLock.EnterReadLock();

        try
        {
            return _sessionLinkedListOderedByCurrentUsage.OfType<TSession>().FirstOrDefault(x => x.Name == name);
        }
        finally
        {
            _sessionLinkedListOderedByCurrentUsageLock.ExitReadLock();
        }
    }

    /// <inheritdoc cref="GetRecursively{TSession}()"/>
    /// <returns>A session or <see langword="null"/>, if such session is not found.</returns>
    public TSession? GetOrNullRecursively<TSession>()
        where TSession : AtataSession
    {
        for (AtataContext? currentContext = _context;
            currentContext is not null;
            currentContext = currentContext.ParentContext)
        {
            TSession? session = currentContext.Sessions.GetOrNull<TSession>();

            if (session is not null)
                return session;
        }

        return null;
    }

    /// <inheritdoc cref="GetRecursively{TSession}(string?)"/>
    /// <returns>A session or <see langword="null"/>, if such session is not found.</returns>
    public TSession? GetOrNullRecursively<TSession>(string? name)
        where TSession : AtataSession
    {
        for (AtataContext? currentContext = _context;
            currentContext is not null;
            currentContext = currentContext.ParentContext)
        {
            TSession? session = currentContext.Sessions.GetOrNull<TSession>(name);

            if (session is not null)
                return session;
        }

        return null;
    }

    private int CountRecursively<TSession>(Func<TSession, bool>? predicate = null)
    {
        int totalCount = 0;

        for (AtataContext? currentContext = _context;
            currentContext is not null;
            currentContext = currentContext.ParentContext)
        {
            var enumerableTypedSessions = currentContext.Sessions._sessionListOrderedByAdding.OfType<TSession>();

            int count = predicate is null
                ? enumerableTypedSessions.Count()
                : enumerableTypedSessions.Count(predicate);

            totalCount += count;
        }

        return totalCount;
    }

    private void EnsureNotDisposed() =>
        Guard.ThrowIfDisposed(_isDisposed, this);
}
