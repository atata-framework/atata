#nullable enable

namespace Atata;

/// <summary>
/// Represents the behavior for control value set by executing <see cref="ValueClearBehaviorAttribute"/> behavior first;
/// then, if value to set is not <see langword="null"/> or empty,
/// executes <see cref="TextTypeBehaviorAttribute"/> behavior.
/// </summary>
public class SetsValueUsingClearAndTypeBehaviorsAttribute : ValueSetBehaviorAttribute
{
    private readonly bool _useUIComponentScopeCache;

    /// <summary>
    /// Initializes a new instance of the <see cref="SetsValueUsingClearAndTypeBehaviorsAttribute"/> class.
    /// </summary>
    public SetsValueUsingClearAndTypeBehaviorsAttribute()
        : this(true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SetsValueUsingClearAndTypeBehaviorsAttribute"/> class.
    /// </summary>
    /// <param name="useUIComponentScopeCache">If set to <see langword="true"/>, uses UI component scope cache.</param>
    public SetsValueUsingClearAndTypeBehaviorsAttribute(bool useUIComponentScopeCache) =>
        _useUIComponentScopeCache = useUIComponentScopeCache;

    /// <inheritdoc/>
    public override void Execute<TOwner>(IUIComponent<TOwner> component, string value)
    {
        void DoExecute()
        {
            component.ExecuteBehavior<ValueClearBehaviorAttribute>(x => x.Execute(component));

            if (value?.Length > 0)
                component.ExecuteBehavior<TextTypeBehaviorAttribute>(x => x.Execute(component, value));
        }

        if (_useUIComponentScopeCache)
            component.Session.UIComponentAccessChainScopeCache.ExecuteWithin(DoExecute);
        else
            DoExecute();
    }
}
