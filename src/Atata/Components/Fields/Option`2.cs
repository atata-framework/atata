#nullable enable

namespace Atata;

/// <summary>
/// Represents the option control (<c>&lt;option&gt;</c>).
/// Default search finds the first occurring <c>&lt;option&gt;</c> element.
/// </summary>
/// <typeparam name="TValue">The type of the data value.</typeparam>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition("option", IgnoreNameEndings = "Option", ComponentTypeName = "option")]
[FindFirst]
public class Option<TValue, TOwner> : Field<TValue, TOwner>
    where TOwner : PageObject<TOwner>
{
    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the value indicating whether the component is selected.
    /// </summary>
    public ValueProvider<bool, TOwner> IsSelected =>
        CreateValueProvider("selected state", GetIsSelected);

    /// <summary>
    /// Gets the <see cref="SelectOptionBehaviorAttribute"/> instance.
    /// By default uses <see cref="SelectsOptionByTextAttribute"/>.
    /// </summary>
    protected SelectOptionBehaviorAttribute SelectOptionBehavior =>
        Metadata.Get<SelectOptionBehaviorAttribute>()
            ?? (Parent as Select<TValue, TOwner>)?.SelectOptionBehavior
            ?? new SelectsOptionByTextAttribute();

    protected virtual bool GetIsSelected() =>
        Scope.Selected;

    protected override TValue GetValue()
    {
        string valueAsString = SelectOptionBehavior.GetOptionRawValue(Scope);
        return ConvertStringToValue(valueAsString)!;
    }

    /// <summary>
    /// Selects the option.
    /// Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
    /// </summary>
    /// <returns>The instance of the owner page object.</returns>
    public TOwner Select() =>
        Click();

    protected override TermOptions GetValueTermOptions() =>
        new TermOptions
        {
            Culture = Metadata.GetCulture()
                ?? Parent?.Metadata.GetCulture()
                ?? Session.Context.Culture,
            Format = Metadata.GetFormat()
                ?? Parent?.Metadata.GetFormat()
        }
        .MergeWith(SelectOptionBehavior);
}
