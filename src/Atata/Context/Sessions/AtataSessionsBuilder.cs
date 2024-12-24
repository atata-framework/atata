#nullable enable

namespace Atata;

public sealed class AtataSessionsBuilder
{
    private readonly AtataContextBuilder _atataContextBuilder;

    private readonly List<IAtataSessionProvider> _sessionProviders;

    private readonly AtataSessionStartScopes? _defaultStartScopes;

    internal AtataSessionsBuilder(
        AtataContextBuilder atataContextBuilder,
        List<IAtataSessionProvider> sessionProviders,
        AtataSessionStartScopes? defaultStartScopes)
    {
        _atataContextBuilder = atataContextBuilder;
        _sessionProviders = sessionProviders;
        _defaultStartScopes = defaultStartScopes;
    }

    /// <summary>
    /// Gets only session builders out of registered providers.
    /// </summary>
    public IEnumerable<IAtataSessionBuilder> Builders =>
        _sessionProviders.OfType<IAtataSessionBuilder>();

    /// <summary>
    /// Gets all the registered session providers: builders, borrow requests, pool requests.
    /// </summary>
    public IReadOnlyList<IAtataSessionProvider> Providers =>
        _sessionProviders;

    public AtataContextBuilder Add<TSessionBuilder>(Action<TSessionBuilder>? configure = null)
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
        _sessionProviders.Add(sessionBuilder);
        return _atataContextBuilder;
    }

    public AtataContextBuilder Configure<TSessionBuilder>(Action<TSessionBuilder>? configure = null)
        where TSessionBuilder : IAtataSessionBuilder
    {
        var sessionBuilder = _sessionProviders.OfType<TSessionBuilder>().LastOrDefault();

        bool isExisiting = sessionBuilder is not null;

        if (!isExisiting)
            sessionBuilder = ActivatorEx.CreateInstance<TSessionBuilder>();

        configure?.Invoke(sessionBuilder!);

        if (!isExisiting)
            _sessionProviders.Add(sessionBuilder!);

        return _atataContextBuilder;
    }

    public AtataContextBuilder Borrow<TSession>(string? sessionName)
        where TSession : AtataSession
        =>
        Borrow<TSession>(x => x.Name = sessionName);

    public AtataContextBuilder Borrow<TSession>(Action<AtataSessionRequestBuilder>? configure = null)
        where TSession : AtataSession
        =>
        Borrow(typeof(TSession), configure);

    public AtataContextBuilder Borrow(Type sessionType, Action<AtataSessionRequestBuilder>? configure = null)
    {
        EnsureAtataSessionType(sessionType);

        AtataSessionBorrowRequestBuilder sessionRequestBuilder = new(sessionType)
        {
            StartScopes = _defaultStartScopes
        };
        configure?.Invoke(sessionRequestBuilder);

        _sessionProviders.Add(sessionRequestBuilder);
        return _atataContextBuilder;
    }

    public AtataContextBuilder TakeFromPool<TSession>(string? sessionName)
        where TSession : AtataSession
        =>
        TakeFromPool<TSession>(x => x.Name = sessionName);

    public AtataContextBuilder TakeFromPool<TSession>(Action<AtataSessionRequestBuilder>? configure = null)
        where TSession : AtataSession
        =>
        TakeFromPool(typeof(TSession), configure);

    public AtataContextBuilder TakeFromPool(Type sessionType, Action<AtataSessionRequestBuilder>? configure = null)
    {
        EnsureAtataSessionType(sessionType);

        AtataSessionPoolRequestBuilder sessionRequestBuilder = new(sessionType)
        {
            StartScopes = _defaultStartScopes
        };
        configure?.Invoke(sessionRequestBuilder);

        _sessionProviders.Add(sessionRequestBuilder);
        return _atataContextBuilder;
    }

    public AtataContextBuilder Remove<TSessionBuilder>(string? name)
        where TSessionBuilder : IAtataSessionBuilder
    {
        _sessionProviders.RemoveAll(x => x is TSessionBuilder && x.Name == name);
        return _atataContextBuilder;
    }

    public AtataContextBuilder RemoveAll<TSessionBuilder>()
        where TSessionBuilder : IAtataSessionBuilder
    {
        _sessionProviders.RemoveAll(x => x is TSessionBuilder);
        return _atataContextBuilder;
    }

    public AtataContextBuilder Clear()
    {
        _sessionProviders.Clear();
        return _atataContextBuilder;
    }

    private static void EnsureAtataSessionType(Type type)
    {
        if (!typeof(AtataSession).IsAssignableFrom(type))
            throw new ArgumentException($"{type.FullName} is not inherited from {nameof(AtataSession)}.", nameof(type));
    }
}
