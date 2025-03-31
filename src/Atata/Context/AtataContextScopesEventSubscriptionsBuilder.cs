#nullable enable

namespace Atata;

internal sealed class AtataContextScopesEventSubscriptionsBuilder : EventSubscriptionsBuilder<AtataContextBuilder>
{
    private readonly Action<Type, object> _addItemAction;

    private readonly Action<Predicate<EventSubscriptionItem>> _removeItemAction;

    internal AtataContextScopesEventSubscriptionsBuilder(
        AtataContextBuilder rootBuilder,
        Action<Type, object> addItemAction,
        Action<Predicate<EventSubscriptionItem>> removeItemAction)
        : base(rootBuilder)
    {
        _addItemAction = addItemAction;
        _removeItemAction = removeItemAction;
    }

    protected override void DoAdd(Type eventType, object eventHandler) =>
        _addItemAction.Invoke(eventType, eventHandler);

    protected override void DoRemoveAll(Predicate<EventSubscriptionItem> match) =>
        _removeItemAction.Invoke(match);
}
