namespace Atata
{
    /// <summary>
    /// Represents the select control (<c>&lt;select&gt;</c>).
    /// Default search is performed by the label.
    /// Option selection is configured via <see cref="SelectOptionBehaviorAttribute"/>.
    /// Possible selection behavior attributes are:
    /// <see cref="SelectsOptionByTextAttribute"/>, <see cref="SelectsOptionByValueAttribute"/>,
    /// <see cref="SelectsOptionByLabelAttributeAttribute"/> and <see cref="SelectsOptionByAttributeAttribute"/>.
    /// Default option selection is performed by text using <see cref="SelectsOptionByTextAttribute"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the control's value.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("select", IgnoreNameEndings = "Select", ComponentTypeName = "select")]
    [FindByLabel]
    [SelectsOptionByText]
    public class Select<TValue, TOwner> : EditableField<TValue, TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the options' <see cref="ControlList{TItem, TOwner}"/> instance.
        /// </summary>
        [TraceLog]
        public ControlList<Option<TValue, TOwner>, TOwner> Options { get; private set; }

        /// <summary>
        /// Gets the selected option.
        /// </summary>
        public Option<TValue, TOwner> SelectedOption => Options[x => x.IsSelected];

        /// <summary>
        /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the index of the selected option.
        /// </summary>
        public ValueProvider<int, TOwner> SelectedIndex =>
            Options.IndexOf(x => x.IsSelected);

        /// <summary>
        /// Gets the <see cref="SelectOptionBehaviorAttribute"/> instance.
        /// By default uses <see cref="SelectsOptionByTextAttribute"/>.
        /// </summary>
        protected internal SelectOptionBehaviorAttribute SelectOptionBehavior =>
            Metadata.Get<SelectOptionBehaviorAttribute>();

        protected override TValue GetValue() =>
            SelectedOption.Value;

        protected override void SetValue(TValue value)
        {
            var option = GetOption(value);
            option.Select();
        }

        protected override TermOptions GetValueTermOptions() =>
            base.GetValueTermOptions().MergeWith(SelectOptionBehavior);

        /// <summary>
        /// Gets the option by the associated value.
        /// </summary>
        /// <param name="value">The value associated with the option.</param>
        /// <returns>The <see cref="Option{TValue, TOwner}"/> instance.</returns>
        public Option<TValue, TOwner> GetOption(TValue value)
        {
            string valueAsString = ConvertValueToString(value);
            string xPathCondition = SelectOptionBehavior.FormatOptionXPathCondition(valueAsString);

            return Options.GetByXPathCondition(valueAsString, xPathCondition);
        }
    }
}
