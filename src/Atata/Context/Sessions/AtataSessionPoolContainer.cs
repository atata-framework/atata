namespace Atata;

internal sealed class AtataSessionPoolContainer : IEnumerable<AtataSessionPool>
{
    private AddOnlyList<PoolItem> _poolItems = [];

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public IEnumerator<AtataSessionPool> GetEnumerator()
    {
        foreach (PoolItem item in _poolItems)
            yield return item.Pool;
    }

    internal AtataSessionPool AddPool(IAtataSessionBuilder sessionBuilder)
    {
        Type sessionType = AtataSessionTypeMap.ResolveSessionTypeByBuilderType(sessionBuilder.GetType());
        string? sessionName = sessionBuilder.Name;

        if (TryGetPool(sessionType, sessionName, out _))
            throw new InvalidOperationException($"Cannot add a session pool. Such pool for {AtataSession.BuildTypedName(sessionType, sessionName)} already exists.");

        AtataSessionPool pool = new(sessionBuilder, sessionBuilder.PoolMaxCapacity);
        _poolItems.Add(new(pool, sessionType, sessionName));

        return pool;
    }

    internal bool TryGetPool(
        Type? sessionType,
        string? sessionName,
        [NotNullWhen(true)] out AtataSessionPool? pool)
    {
        for (int i = _poolItems.Count - 1; i >= 0; i--)
        {
            PoolItem item = _poolItems[i];

            if (item.SessionName == sessionName && (sessionType is null || sessionType.IsAssignableFrom(item.SessionType)))
            {
                pool = item.Pool;
                return true;
            }
        }

        pool = null;
        return false;
    }

    internal void Clear() =>
        _poolItems = [];

    private readonly record struct PoolItem(AtataSessionPool Pool, Type SessionType, string? SessionName);
}
