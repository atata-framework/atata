namespace Atata;

public sealed class AtataSessionsBuilder
{
    private readonly AtataContextBuilder _atataContextBuilder;

    private readonly List<IAtataSessionBuilder> _sessionBuilders;

    private readonly List<AtataSessionRequestBuilder> _sessionBorrowRequests;

    private readonly List<AtataSessionRequestBuilder> _sessionPoolRequests;

    private readonly AtataSessionStartScopes? _defaultStartScopes;

    internal AtataSessionsBuilder(
        AtataContextBuilder atataContextBuilder,
        List<IAtataSessionBuilder> sessionBuilders,
        List<AtataSessionRequestBuilder> sessionBorrowClaims,
        List<AtataSessionRequestBuilder> sessionPoolClaims,
        AtataSessionStartScopes? defaultStartScopes)
    {
        _atataContextBuilder = atataContextBuilder;
        _sessionBuilders = sessionBuilders;
        _sessionBorrowRequests = sessionBorrowClaims;
        _sessionPoolRequests = sessionPoolClaims;
        _defaultStartScopes = defaultStartScopes;
    }

    public IReadOnlyList<IAtataSessionBuilder> Builders =>
        _sessionBuilders;

    public IReadOnlyList<AtataSessionRequestBuilder> BorrowRequests =>
        _sessionBorrowRequests;

    public IReadOnlyList<AtataSessionRequestBuilder> PoolRequests =>
        _sessionPoolRequests;

    public AtataContextBuilder Add<TSessionBuilder>(Action<TSessionBuilder> configure = null)
        where TSessionBuilder : IAtataSessionBuilder, new()
    {
        var sessionBuilder = new TSessionBuilder
        {
            StartScopes = _defaultStartScopes
        };
        configure?.Invoke(sessionBuilder);

        return Add(sessionBuilder);
    }

    internal AtataContextBuilder Add(IAtataSessionBuilder sessionBuilder)
    {
        _sessionBuilders.Add(sessionBuilder);
        return _atataContextBuilder;
    }

    public AtataContextBuilder Configure<TSessionBuilder>(Action<TSessionBuilder> configure = null)
        where TSessionBuilder : IAtataSessionBuilder
    {
        var sessionBuilder = _sessionBuilders.OfType<TSessionBuilder>().LastOrDefault();

        bool isExisiting = sessionBuilder is not null;

        if (!isExisiting)
            sessionBuilder = ActivatorEx.CreateInstance<TSessionBuilder>();

        configure?.Invoke(sessionBuilder);

        if (!isExisiting)
            _sessionBuilders.Add(sessionBuilder);

        return _atataContextBuilder;
    }

    public AtataContextBuilder Borrow<TSession>(string sessionName)
        where TSession : AtataSession
        =>
        Borrow<TSession>(x => x.Name = sessionName);

    public AtataContextBuilder Borrow<TSession>(Action<AtataSessionRequestBuilder> configure = null)
        where TSession : AtataSession
        =>
        Borrow(typeof(TSession), configure);

    public AtataContextBuilder Borrow(Type sessionType, Action<AtataSessionRequestBuilder> configure = null)
    {
        EnsureAtataSessionType(sessionType);

        AtataSessionRequestBuilder sessionRequestBuilder = new(sessionType)
        {
            StartScopes = _defaultStartScopes
        };
        configure?.Invoke(sessionRequestBuilder);

        _sessionBorrowRequests.Add(sessionRequestBuilder);
        return _atataContextBuilder;
    }

    public AtataContextBuilder TakeFromPool<TSession>(string sessionName)
        where TSession : AtataSession
        =>
        TakeFromPool<TSession>(x => x.Name = sessionName);

    public AtataContextBuilder TakeFromPool<TSession>(Action<AtataSessionRequestBuilder> configure = null)
        where TSession : AtataSession
        =>
        TakeFromPool(typeof(TSession), configure);

    public AtataContextBuilder TakeFromPool(Type sessionType, Action<AtataSessionRequestBuilder> configure = null)
    {
        EnsureAtataSessionType(sessionType);

        AtataSessionRequestBuilder sessionRequestBuilder = new(sessionType)
        {
            StartScopes = _defaultStartScopes
        };
        configure?.Invoke(sessionRequestBuilder);

        _sessionPoolRequests.Add(sessionRequestBuilder);
        return _atataContextBuilder;
    }

    public AtataContextBuilder RemoveAll<TSessionBuilder>()
        where TSessionBuilder : IAtataSessionBuilder
    {
        _sessionBuilders.RemoveAll(x => x is TSessionBuilder);
        return _atataContextBuilder;
    }

    public AtataContextBuilder Clear()
    {
        _sessionBuilders.Clear();
        _sessionBorrowRequests.Clear();
        _sessionPoolRequests.Clear();

        return _atataContextBuilder;
    }

    private static void EnsureAtataSessionType(Type type)
    {
        if (!typeof(AtataSession).IsAssignableFrom(type))
            throw new ArgumentException($"{type.FullName} is not inherited from {nameof(AtataSession)}.", nameof(type));
    }
}
