#nullable enable

namespace Atata;

/// <summary>
/// Represents a session event subscriptions builder.
/// </summary>
/// <typeparam name="TSessionBuilder">The type of the session builder.</typeparam>
public sealed class AtataSessionEventSubscriptionsBuilder<TSessionBuilder> : EventSubscriptionsBuilder<TSessionBuilder>
{
    private readonly List<EventSubscriptionItem> _items;

    internal AtataSessionEventSubscriptionsBuilder(TSessionBuilder rootBuilder)
        : this(rootBuilder, [])
    {
    }

    private AtataSessionEventSubscriptionsBuilder(
        TSessionBuilder rootBuilder,
        List<EventSubscriptionItem> items)
        : base(rootBuilder)
        =>
        _items = items;

    internal IReadOnlyList<EventSubscriptionItem> Items =>
        _items;

    protected override void DoAdd(Type eventType, object eventHandler) =>
        _items.Add(new(eventType, eventHandler));

    protected override void DoRemoveAll(Predicate<EventSubscriptionItem> match) =>
        _items.RemoveAll(match);

    internal AtataSessionEventSubscriptionsBuilder<TSessionBuilder> CloneFor(TSessionBuilder rootBuilder) =>
        new(rootBuilder, [.. _items]);
}
