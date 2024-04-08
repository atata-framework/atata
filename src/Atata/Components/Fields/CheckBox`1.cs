namespace Atata;

/// <summary>
/// Represents the checkbox control (<c>&lt;input type="checkbox"&gt;</c>).
/// Default search is performed by the label.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition("input[@type='checkbox']", ComponentTypeName = "checkbox", IgnoreNameEndings = "Checkbox,CheckBox,Option")]
[FindByLabel]
public class CheckBox<TOwner> : EditableField<bool, TOwner>, ICheckable<TOwner>
    where TOwner : PageObject<TOwner>
{
    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}" /> of the checked state value.
    /// </summary>
    public ValueProvider<bool, TOwner> IsChecked =>
        CreateValueProvider("checked state", () => Value);

    /// <inheritdoc cref="UIComponent{TOwner}.Should"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public new FieldVerificationProvider<bool, CheckBox<TOwner>, TOwner> Should =>
        new(this);

    /// <inheritdoc cref="UIComponent{TOwner}.ExpectTo"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public new FieldVerificationProvider<bool, CheckBox<TOwner>, TOwner> ExpectTo =>
        Should.Using<ExpectationVerificationStrategy>();

    /// <inheritdoc cref="UIComponent{TOwner}.WaitTo"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public new FieldVerificationProvider<bool, CheckBox<TOwner>, TOwner> WaitTo =>
        Should.Using<WaitingVerificationStrategy>();

    protected override bool GetValue() =>
        Scope.Selected;

    protected override void SetValue(bool value) =>
        Session.UIComponentAccessChainScopeCache.ExecuteWithin(() =>
        {
            if (GetValue() != value)
                OnClick();
        });

    /// <summary>
    /// Checks the control.
    /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
    /// </summary>
    /// <returns>The owner page object.</returns>
    public TOwner Check() =>
        Set(true);

    /// <summary>
    /// Unchecks the control.
    /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
    /// </summary>
    /// <returns>The owner page object.</returns>
    public TOwner Uncheck() =>
        Set(false);
}
