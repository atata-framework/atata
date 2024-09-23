namespace Atata;

public sealed class AtataSessionCollection : IReadOnlyCollection<AtataSession>, IDisposable
{
    private readonly List<IAtataSessionBuilder> _sessionBuilders = [];

    private readonly List<AtataSession> _sessionListOrderedByAdding = [];

    private readonly LinkedList<AtataSession> _sessionLinkedListOderedByCurrentUsage = [];

    private readonly ReaderWriterLockSlim _sessionLinkedListOderedByCurrentUsageLock = new();

    public int Count =>
        _sessionListOrderedByAdding.Count;

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

#warning Finish later Start methods.
    ////public TSession Start<TSessionBuilder, TSession>(string name = null)
    ////{
    ////    //return
    ////}

    ////public TSession Start<TSessionBuilder, TSession>(Action<TSessionBuilder> sessionConfiguration)
    ////    where TSessionBuilder : IAtataSessionBuilder
    ////{
    ////    var sessionBuilder = ActivatorEx.CreateInstance<TSessionBuilder>();

    ////    sessionConfiguration?.Invoke(sessionBuilder);

    ////    return (TSession)sessionBuilder.Build();
    ////}

    internal void AddBuilders(IEnumerable<IAtataSessionBuilder> sessionBuilders) =>
        _sessionBuilders.AddRange(sessionBuilders);

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

        _sessionListOrderedByAdding.Clear();
        _sessionLinkedListOderedByCurrentUsage.Clear();
    }
}
