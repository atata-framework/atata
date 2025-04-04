#nullable enable

namespace Atata;

/// <summary>
/// Represents the checkbox list control (a set of <c>&lt;input type="checkbox"&gt;</c>).
/// Default search is performed by the name.
/// Specific checkbox items can be found by label or value.
/// By default items are searched by label using <see cref="FindItemByLabelAttribute"/>.
/// Use <see cref="FindItemByValueAttribute"/> to find items by value.
/// Currently as a data type supports only enum (with <c>[Flags]</c>) types.
/// </summary>
/// <typeparam name="TValue">
/// The type of the control's value.
/// Supports only enum (with <c>[Flags]</c>) types.
/// </typeparam>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition("input[@type='checkbox']", ComponentTypeName = "checkbox list", IgnoreNameEndings = "CheckBoxes,CheckBoxList,CheckBoxGroup,Options,OptionGroup")]
[FindByName]
[FindItemByLabel]
public class CheckBoxList<TValue, TOwner> : OptionList<TValue, TOwner>
    where TOwner : PageObject<TOwner>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CheckBoxList{TValue, TOwner}"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">generic <typeparamref name="TValue"/> parameter is not an Enum type.</exception>
    public CheckBoxList()
    {
        if (!typeof(TValue).IsEnum)
            throw new InvalidOperationException($"Incorrect generic parameter '{typeof(TValue).FullName}' type. {nameof(CheckBoxList<TValue, TOwner>)} control supports only Enum types.");
    }

    protected delegate bool ClickItemPredicate(bool isInValue, bool isSelected);

    /// <inheritdoc cref="UIComponent{TOwner}.Should"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public new FieldVerificationProvider<TValue, CheckBoxList<TValue, TOwner>, TOwner> Should =>
        new(this);

    /// <inheritdoc cref="UIComponent{TOwner}.ExpectTo"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public new FieldVerificationProvider<TValue, CheckBoxList<TValue, TOwner>, TOwner> ExpectTo =>
        Should.Using(ExpectationVerificationStrategy.Instance);

    /// <inheritdoc cref="UIComponent{TOwner}.WaitTo"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public new FieldVerificationProvider<TValue, CheckBoxList<TValue, TOwner>, TOwner> WaitTo =>
        Should.Using(WaitingVerificationStrategy.Instance);

    protected override TValue GetValue()
    {
        TValue[] selectedValues = GetItemElements()
            .Where(x => x.Selected)
            .Select(x => ItemElementFindStrategy.GetParameter<TValue>(x, GetValueTermOptions()))
            .ToArray();

        return selectedValues.Length > 0
            ? JoinValues(selectedValues)
            : default!;
    }

    private static TValue JoinValues(TValue[] values) =>
        (TValue)(object)JoinEnumValues(values.Cast<Enum>());

    private static Enum JoinEnumValues(IEnumerable<Enum> values) =>
        values.Aggregate(EnumExtensions.AddFlag);

    protected override void SetValue(TValue value) =>
        ClickItems(value, (isInValue, isSelected) => isInValue != isSelected);

    protected void ClickItems(TValue value, ClickItemPredicate predicate)
    {
        List<TValue> individualValues = [.. GetIndividualValues(value)];

        IWebElement[] elements = GetItemElements();
        foreach (IWebElement element in elements)
        {
            TValue elementValue = GetElementValue(element);
            bool isInValue = individualValues.Contains(elementValue);

            if (isInValue)
                individualValues.Remove(elementValue);

            if (predicate(isInValue, element.Selected))
                element.ClickWithLogging(Log);
        }

        if (individualValues.Count > 0)
        {
            throw ElementExceptionFactory.CreateForNotFound(
                new SearchFailureData
                {
                    ElementName = $"{ConvertIndividualValuesToString(individualValues, true)} checkbox element{(individualValues.Count > 1 ? "s" : null)} of {ComponentFullName}"
                });
        }
    }

    /// <summary>
    /// Checks the checkbox by specified value.
    /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
    /// </summary>
    /// <param name="value">The value of the checkbox.</param>
    /// <returns>The owner page object.</returns>
    public TOwner Check(TValue value)
    {
        if (!Equals(value, null))
        {
            ExecuteTriggers(TriggerEvents.BeforeSet);

            Log.ExecuteSection(
                new ValueChangeLogSection(this, nameof(Check), ConvertValueToString(value)),
                () => ClickItems(value, (isInValue, isSelected) => isInValue && !isSelected));

            ExecuteTriggers(TriggerEvents.AfterSet);
        }

        return Owner;
    }

    /// <summary>
    /// Unchecks the checkbox by specified value.
    /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
    /// </summary>
    /// <param name="value">The value of the checkbox.</param>
    /// <returns>The owner page object.</returns>
    public TOwner Uncheck(TValue value)
    {
        if (!Equals(value, null))
        {
            ExecuteTriggers(TriggerEvents.BeforeSet);

            Log.ExecuteSection(
                new ValueChangeLogSection(this, nameof(Uncheck), ConvertValueToString(value)),
                () => ClickItems(value, (isInValue, isSelected) => isInValue && isSelected));

            ExecuteTriggers(TriggerEvents.AfterSet);
        }

        return Owner;
    }

    protected internal IEnumerable<TValue> GetIndividualValues(TValue value) =>
        ((Enum)(object)value!).GetIndividualFlags().Cast<TValue>();

    protected internal override string ConvertValueToString(TValue value)
    {
        var individualValues = GetIndividualValues(value);
        return ConvertIndividualValuesToString(individualValues, false);
    }

    protected internal string ConvertIndividualValuesToString(IEnumerable<TValue> values, bool wrapWithDoubleQuotes)
    {
        string[]? stringValues = values?.Select(x => TermResolver.ToString(x, GetValueTermOptions())!)?.ToArray();

        if (stringValues is null or [])
            return "<none>";
        else if (wrapWithDoubleQuotes)
            return stringValues.ToDoubleQuotedValuesListOfString();
        else
            return string.Join(", ", stringValues);
    }
}
