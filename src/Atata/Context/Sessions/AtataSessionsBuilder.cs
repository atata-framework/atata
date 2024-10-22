namespace Atata;

#warning Probably update methods to return AtataSessionsBuilder instead of AtataContextBuilder.
public sealed class AtataSessionsBuilder
{
    private readonly AtataContextBuilder _atataContextBuilder;

    private readonly List<IAtataSessionBuilder> _sessionBuilders;

    private readonly AtataSessionStartScopes? _defaultStartScopes;

    internal AtataSessionsBuilder(
        AtataContextBuilder atataContextBuilder,
        List<IAtataSessionBuilder> sessionBuilders,
        AtataSessionStartScopes? defaultStartScopes)
    {
        _atataContextBuilder = atataContextBuilder;
        _sessionBuilders = sessionBuilders;
        _defaultStartScopes = defaultStartScopes;
    }

    public IReadOnlyList<IAtataSessionBuilder> Builders =>
        _sessionBuilders;

    public AtataContextBuilder Add<TSessionBuilder>(Action<TSessionBuilder> configure = null)
        where TSessionBuilder : IAtataSessionBuilder, new()
    {
        var sessionBuilder = new TSessionBuilder
        {
            StartScopes = _defaultStartScopes
        };
        configure?.Invoke(sessionBuilder);

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

    public AtataContextBuilder RemoveAll<TSessionBuilder>()
        where TSessionBuilder : IAtataSessionBuilder
    {
        _sessionBuilders.RemoveAll(x => x is TSessionBuilder);
        return _atataContextBuilder;
    }

    public AtataContextBuilder Clear()
    {
        _sessionBuilders.Clear();
        return _atataContextBuilder;
    }
}
