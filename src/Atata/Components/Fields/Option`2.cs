namespace Atata
{
    /// <summary>
    /// Represents the option control (<c>&lt;option&gt;</c>).
    /// Default search finds the first occurring <c>&lt;option&gt;</c> element.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("option", IgnoreNameEndings = "Option", ComponentTypeName = "option")]
    [FindFirst]
    public class Option<T, TOwner> : Field<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the value indicating whether the component is selected.
        /// </summary>
        public DataProvider<bool, TOwner> IsSelected => GetOrCreateDataProvider("selected state", GetIsSelected);

        /// <summary>
        /// Gets the <see cref="SelectOptionBehaviorAttribute"/> instance.
        /// By default uses <see cref="SelectsOptionByTextAttribute"/>.
        /// </summary>
        protected SelectOptionBehaviorAttribute SelectOptionBehavior =>
            Metadata.Get<SelectOptionBehaviorAttribute>()
                ?? (Parent as Select<T, TOwner>)?.SelectOptionBehavior
                ?? new SelectsOptionByTextAttribute();

        protected virtual bool GetIsSelected()
        {
            return Scope.Selected;
        }

        protected override T GetValue()
        {
            string valueAsString = SelectOptionBehavior.GetOptionRawValue(Scope);
            return ConvertStringToValue(valueAsString);
        }

        /// <summary>
        /// Selects the option.
        /// Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
        /// </summary>
        /// <returns>The instance of the owner page object.</returns>
        public TOwner Select()
        {
            return Click();
        }

        protected override TermOptions GetValueTermOptions() =>
            new TermOptions
            {
                Culture = Metadata.Contains<CultureAttribute>()
                    ? Metadata.GetCulture()
                    : Parent.Metadata.GetCulture(),
                Format = Metadata.GetFormat() ?? Parent.Metadata.GetFormat()
            }
            .MergeWith(SelectOptionBehavior);
    }
}
