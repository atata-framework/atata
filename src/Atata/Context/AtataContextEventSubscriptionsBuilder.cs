namespace Atata;

/// <summary>
/// Represents an <see cref="AtataContext"/> event subscriptions builder.
/// </summary>
public sealed class AtataContextEventSubscriptionsBuilder : EventSubscriptionsBuilder<AtataContextBuilder>
{
    private readonly List<ScopeLimitedEventSubscription> _items;

    internal AtataContextEventSubscriptionsBuilder(AtataContextBuilder rootBuilder)
        : this(rootBuilder, [])
    {
    }

    private AtataContextEventSubscriptionsBuilder(
        AtataContextBuilder rootBuilder,
        List<ScopeLimitedEventSubscription> items)
        : base(rootBuilder) =>
        _items = items;

    /// <summary>
    /// Gets the event subscriptions builder for the specified <paramref name="scopes"/>.
    /// </summary>
    /// <param name="scopes">The scopes.</param>
    /// <returns>The <see cref="AtataSessionEventSubscriptionsBuilder{TRootBuilder}"/> instance.</returns>
    public EventSubscriptionsBuilder<AtataContextBuilder> For(AtataContextScopes scopes) =>
        new AtataContextScopesEventSubscriptionsBuilder(
            RootBuilder,
            (e, h) => _items.Add(new(e, h, scopes)),
            (match) => RemoveAllItems(scopes, match));

    internal IEnumerable<EventSubscriptionItem> GetItemsForScope(AtataContextScope? scope)
    {
        foreach (var item in _items)
        {
            if (item.Scopes.Satisfies(scope))
                yield return item.Item;
        }
    }

    internal AtataContextEventSubscriptionsBuilder CloneFor(AtataContextBuilder rootBuilder) =>
        new(rootBuilder, [.. _items]);

    protected override void DoAdd(Type eventType, object eventHandler) =>
        _items.Add(new(eventType, eventHandler, AtataContextScopes.All));

    protected override void DoRemoveAll(Predicate<EventSubscriptionItem> match) =>
        RemoveAllItems(AtataContextScopes.All, match);

    private void RemoveAllItems(AtataContextScopes scopes, Predicate<EventSubscriptionItem> match) =>
        _items.RemoveAll(x => x.Scopes == scopes && match(x.Item));

    private sealed class ScopeLimitedEventSubscription
    {
        internal ScopeLimitedEventSubscription(Type eventType, object eventHandler, AtataContextScopes scopes)
        {
            Item = new(eventType, eventHandler);
            Scopes = scopes;
        }

        public AtataContextScopes Scopes { get; }

        public EventSubscriptionItem Item { get; }
    }
}
