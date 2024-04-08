namespace Atata;

public sealed class AtataSessionsBuilder
{
    private readonly List<IAtataSessionBuilder> _sessionBuilders = [];

    internal AtataSessionsBuilder()
    {
    }

    public IReadOnlyList<IAtataSessionBuilder> Builders => _sessionBuilders;

    public TSessionBuilder Add<TSessionBuilder>()
        where TSessionBuilder : IAtataSessionBuilder, new() =>
        Add(new TSessionBuilder());

    public TSessionBuilder Add<TSessionBuilder>(TSessionBuilder builder)
        where TSessionBuilder : IAtataSessionBuilder
    {
        _sessionBuilders.Add(builder);
        return builder;
    }

    public TSessionBuilder Configure<TSessionBuilder>()
        where TSessionBuilder : IAtataSessionBuilder =>
        _sessionBuilders.OfType<TSessionBuilder>().LastOrDefault()
            ?? ActivatorEx.CreateInstance<TSessionBuilder>();

    public void RemoveAll<TSessionBuilder>()
        where TSessionBuilder : IAtataSessionBuilder =>
        _sessionBuilders.RemoveAll(x => x is TSessionBuilder);
}
