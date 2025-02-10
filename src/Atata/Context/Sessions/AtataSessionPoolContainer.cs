#nullable enable

namespace Atata;

internal sealed class AtataSessionPoolContainer : IEnumerable<AtataSessionPool>
{
    private readonly Dictionary<PoolKey, AtataSessionPool> _poolsMap = [];

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public IEnumerator<AtataSessionPool> GetEnumerator()
    {
        foreach (AtataSessionPool pool in _poolsMap.Values)
            yield return pool;
    }

    internal AtataSessionPool AddPool(IAtataSessionBuilder sessionBuilder)
    {
        Type sessionType = AtataSessionTypeMap.ResolveSessionTypeByBuilderType(sessionBuilder.GetType());

        PoolKey key = new(sessionType, sessionBuilder.Name);
        AtataSessionPool pool = new(sessionBuilder, sessionBuilder.PoolMaxCapacity);
        _poolsMap.Add(key, pool);

        return pool;
    }

    internal bool TryGetPool(
        Type sessionType,
        string? sessionName,
        [NotNullWhen(true)] out AtataSessionPool? pool)
    {
        PoolKey key = new(sessionType, sessionName);

        return _poolsMap.TryGetValue(key, out pool);
    }

    internal void Clear() =>
        _poolsMap.Clear();

    private readonly record struct PoolKey(Type SessionType, string? SessionName);
}
