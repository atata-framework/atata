#nullable enable

namespace Atata;

internal sealed class AtataContextScopesEventSubscriptionsBuilder : EventSubscriptionsBuilder<AtataContextBuilder>
{
    private readonly Action<Type, object> _addItemAction;

    internal AtataContextScopesEventSubscriptionsBuilder(
        AtataContextBuilder rootBuilder,
        Action<Type, object> addItemAction)
        : base(rootBuilder)
        =>
        _addItemAction = addItemAction;

    protected override void DoAdd(Type eventType, object eventHandler) =>
        _addItemAction.Invoke(eventType, eventHandler);
}
