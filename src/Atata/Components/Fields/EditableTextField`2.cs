namespace Atata;

/// <summary>
/// <para>
/// Represents the editable text field control.
/// </para>
/// <para>
/// To set a value, executes an associated with the component <see cref="ValueSetBehaviorAttribute"/>
/// that is <see cref="SetsValueUsingClearAndTypeBehaviorsAttribute"/> by default.
/// To get a value, executes an associated with the component <see cref="ValueGetBehaviorAttribute"/>
/// that is <see cref="GetsValueFromValueAttribute"/> by default.
/// </para>
/// </summary>
/// <typeparam name="TValue">The type of the control's value.</typeparam>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[GetsValueFromValue]
[SetsValueUsingClearAndTypeBehaviors]
[ClearsValueUsingClearMethod]
[TypesTextUsingSendKeys]
public class EditableTextField<TValue, TOwner> : EditableField<TValue, TOwner>, IClearable
    where TOwner : PageObject<TOwner>
{
    /// <summary>
    /// Gets the value by executing <see cref="ValueGetBehaviorAttribute"/>.
    /// </summary>
    /// <returns>The value.</returns>
    protected override TValue GetValue()
    {
        string valueAsString = ExecuteBehavior<ValueGetBehaviorAttribute, string>(x => x.Execute(this));

        return ConvertStringToValueUsingGetFormat(valueAsString);
    }

    /// <summary>
    /// Sets the value by executing <see cref="ValueSetBehaviorAttribute"/> behavior.
    /// </summary>
    /// <param name="value">The value.</param>
    protected override void SetValue(TValue value)
    {
        string valueAsString = ConvertValueToStringUsingSetFormat(value);

        ExecuteBehavior<ValueSetBehaviorAttribute>(x => x.Execute(this, valueAsString));
    }

    void IClearable.Clear() =>
        Clear();

    /// <summary>
    /// Clears the value.
    /// Executes an associated with the component <see cref="ValueClearBehaviorAttribute"/>
    /// that is <see cref="ClearsValueUsingClearMethodAttribute"/> by default.
    /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
    /// </summary>
    /// <returns>The owner page object.</returns>
    public TOwner Clear()
    {
        ExecuteTriggers(TriggerEvents.BeforeSet);

        Log.ExecuteSection(
            new ValueClearLogSection(this),
            OnClear);

        ExecuteTriggers(TriggerEvents.AfterSet);

        return Owner;
    }

    /// <summary>
    /// Clears the value by executing <see cref="ValueClearBehaviorAttribute"/> behavior.
    /// </summary>
    protected virtual void OnClear() =>
        ExecuteBehavior<ValueClearBehaviorAttribute>(x => x.Execute(this));

    /// <summary>
    /// Types (appends) the specified text value.
    /// Executes an associated with the component <see cref="TextTypeBehaviorAttribute"/>
    /// that is <see cref="TypesTextUsingSendKeysAttribute"/> by default.
    /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
    /// </summary>
    /// <param name="text">The text to type.</param>
    /// <returns>The owner page object.</returns>
    public TOwner Type(string text)
    {
        ExecuteTriggers(TriggerEvents.BeforeSet);

        Log.ExecuteSection(
            new ValueChangeLogSection(this, nameof(Type), text),
            () => OnType(text));

        ExecuteTriggers(TriggerEvents.AfterSet);

        return Owner;
    }

    /// <summary>
    /// Types the text value by executing <see cref="TextTypeBehaviorAttribute" /> behavior.
    /// </summary>
    /// <param name="text">The text to type.</param>
    protected virtual void OnType(string text) =>
        ExecuteBehavior<TextTypeBehaviorAttribute>(x => x.Execute(this, text));
}
