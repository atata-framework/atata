namespace Atata;

/// <summary>
/// Represents the radio button control (<c>&lt;input type="radio"&gt;</c>).
/// Default search is performed by the label.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition("input[@type='radio']", IgnoreNameEndings = "RadioButton,Radio,Button,Option", ComponentTypeName = "radio button")]
[FindByLabel]
public class RadioButton<TOwner> : Field<bool, TOwner>, ICheckable<TOwner>
    where TOwner : PageObject<TOwner>
{
    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}" /> of the value indicating whether the control is checked.
    /// </summary>
    public ValueProvider<bool, TOwner> IsChecked =>
        CreateValueProvider("checked state", () => Value);

    /// <inheritdoc cref="UIComponent{TOwner}.Should"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public new FieldVerificationProvider<bool, RadioButton<TOwner>, TOwner> Should =>
        new(this);

    /// <inheritdoc cref="UIComponent{TOwner}.ExpectTo"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public new FieldVerificationProvider<bool, RadioButton<TOwner>, TOwner> ExpectTo =>
        Should.Using(ExpectationVerificationStrategy.Instance);

    /// <inheritdoc cref="UIComponent{TOwner}.WaitTo"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public new FieldVerificationProvider<bool, RadioButton<TOwner>, TOwner> WaitTo =>
        Should.Using(WaitingVerificationStrategy.Instance);

    protected override bool GetValue() =>
        Scope.Selected;

    /// <summary>
    /// Checks the control.
    /// Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
    /// </summary>
    /// <returns>The owner page object.</returns>
    public TOwner Check()
    {
        OnCheck();

        return Owner;
    }

    protected virtual void OnCheck()
    {
        if (!IsChecked)
            Click();
    }
}
