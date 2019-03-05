using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the checkbox list control (a set of <c>&lt;input type="checkbox"&gt;</c>).
    /// Default search is performed by the name.
    /// Specific checkbox items can be found by label or value.
    /// By default items are searched by label using <see cref="FindItemByLabelAttribute"/>.
    /// Use <see cref="FindItemByValueAttribute"/> to find items by value.
    /// Currently as a data type supports only enum (with <c>[Flags]</c>) types.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the control's data.
    /// Supports only enum (with <c>[Flags]</c>) types.
    /// </typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='checkbox']", ComponentTypeName = "checkbox list", IgnoreNameEndings = "CheckBoxes,CheckBoxList,CheckBoxGroup,Options,OptionGroup")]
    [ControlFinding(FindTermBy.Name)]
    public class CheckBoxList<T, TOwner> : OptionList<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBoxList{T, TOwner}"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">generic <typeparamref name="T"/> parameter is not an Enum type.</exception>
        public CheckBoxList()
        {
            if (!typeof(T).IsEnum)
                throw new InvalidOperationException($"Incorrect generic parameter '{typeof(T).FullName}' type. {nameof(CheckBoxList<T, TOwner>)} control supports only Enum types.");
        }

        protected delegate bool ClickItemPredicate(bool isInValue, bool isSelected);

        /// <summary>
        /// Gets the verification provider that gives a set of verification extension methods.
        /// </summary>
        public new FieldVerificationProvider<T, CheckBoxList<T, TOwner>, TOwner> Should => new FieldVerificationProvider<T, CheckBoxList<T, TOwner>, TOwner>(this);

        protected override T GetValue()
        {
            T[] selectedValues = GetItemElements().
                Where(x => x.Selected).
                Select(x => ItemElementFindStrategy.GetParameter<T>(x, ValueTermOptions)).
                ToArray();

            return selectedValues.Any()
                ? JoinValues(selectedValues)
                : default(T);
        }

        private static T JoinValues(T[] values)
        {
            return (T)(object)JoinEnumValues(values.Cast<Enum>());
        }

        private static Enum JoinEnumValues(IEnumerable<Enum> values)
        {
            return values.Aggregate(EnumExtensions.AddFlag);
        }

        protected override void SetValue(T value)
        {
            ClickItems(value, (isInValue, isSelected) => isInValue != isSelected);
        }

        protected void ClickItems(T value, ClickItemPredicate predicate)
        {
            List<T> individualValues = GetIndividualValues(value).ToList();

            IWebElement[] elements = GetItemElements();
            foreach (IWebElement element in elements)
            {
                T elementValue = GetElementValue(element);
                bool isInValue = individualValues.Contains(elementValue);

                if (isInValue)
                    individualValues.Remove(elementValue);

                if (predicate(isInValue, element.Selected))
                    element.Click();
            }

            if (individualValues.Any())
            {
                throw ExceptionFactory.CreateForNoSuchElement(
                    "Unable to locate element{0}: {1}.".FormatWith(
                        individualValues.Count > 1 ? "s" : null,
                        ConvertIndividualValuesToString(individualValues, true)));
            }
        }

        /// <summary>
        /// Checks the checkbox by specified value.
        /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
        /// </summary>
        /// <param name="value">The value of the checkbox.</param>
        /// <returns>The owner page object.</returns>
        public TOwner Check(T value)
        {
            if (!Equals(value, null))
            {
                ExecuteTriggers(TriggerEvents.BeforeSet);
                Log.Start(new DataSettingLogSection(this, ConvertValueToString(value)) { ActionText = "Check" });

                ClickItems(value, (isInValue, isSelected) => isInValue && !isSelected);

                Log.EndSection();
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
        public TOwner Uncheck(T value)
        {
            if (!Equals(value, null))
            {
                ExecuteTriggers(TriggerEvents.BeforeSet);
                Log.Start(new DataSettingLogSection(this, ConvertValueToString(value)) { ActionText = "Uncheck" });

                ClickItems(value, (isInValue, isSelected) => isInValue && isSelected);

                Log.EndSection();
                ExecuteTriggers(TriggerEvents.AfterSet);
            }

            return Owner;
        }

        protected internal IEnumerable<T> GetIndividualValues(T value)
        {
            return ((Enum)(object)value).GetIndividualFlags().Cast<T>();
        }

        protected internal override string ConvertValueToString(T value)
        {
            var individualValues = GetIndividualValues(value);
            return ConvertIndividualValuesToString(individualValues, false);
        }

        protected internal string ConvertIndividualValuesToString(IEnumerable<T> values, bool wrapWithDoubleQuotes)
        {
            string[] stringValues = values.Select(x => TermResolver.ToString(x, ValueTermOptions)).ToArray();

            if (stringValues.Length == 0)
                return "<none>";
            else if (wrapWithDoubleQuotes)
                return stringValues.ToQuotedValuesListOfString(doubleQuotes: true);
            else
                return string.Join(", ", stringValues);
        }
    }
}
