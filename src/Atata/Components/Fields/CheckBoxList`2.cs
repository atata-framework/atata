using Humanizer;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    [UIComponent("input[@type='checkbox']", IgnoreNameEndings = "CheckBoxes,CheckBoxList,CheckBoxGroup,Options,OptionGroup")]
    public class CheckBoxList<T, TOwner> : OptionList<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        public CheckBoxList()
        {
            if (!typeof(T).IsEnum)
                throw new InvalidOperationException("Incorrect generic parameter type '{0}'. CheckBoxList control supports only Enum types.".FormatWith(typeof(T).FullName));
        }

        protected delegate bool ClickItemPredicate(bool isInValue, bool isSelected);

        protected override T GetValue()
        {
            T[] selectedValues = GetItemElements().
                Where(x => x.Selected).
                Select(x => ItemElementFindStrategy.GetParameter<T>(x, ValueTermOptions)).
                ToArray();

            if (selectedValues.Any())
                return JoinValues(selectedValues);
            else
                return default(T);
        }

        private T JoinValues(T[] values)
        {
            return (T)(object)JoinEnumValues(values.Cast<Enum>());
        }

        private Enum JoinEnumValues(IEnumerable<Enum> values)
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
                throw ExceptionFactory.CreateForNoSuchElement(
                    "Unable to locate element{0}: '{1}'.".FormatWith(
                        individualValues.Count > 1 ? "s" : null,
                        ConvertIndividualValuesToString(individualValues)));
        }

        public TOwner Check(T value)
        {
            if (!object.Equals(value, null))
            {
                RunTriggers(TriggerEvents.BeforeSet);
                Log.StartSection("Check '{0}' of {1}", ConvertValueToString(value), ComponentName);

                ClickItems(value, (isInValue, isSelected) => isInValue && !isSelected);

                Log.EndSection();
                RunTriggers(TriggerEvents.AfterSet);
            }
            return Owner;
        }

        public TOwner Uncheck(T value)
        {
            if (!object.Equals(value, null))
            {
                RunTriggers(TriggerEvents.BeforeSet);
                Log.StartSection("Uncheck '{0}' of {1}", ConvertValueToString(value), ComponentName);

                ClickItems(value, (isInValue, isSelected) => isInValue && isSelected);

                Log.EndSection();
                RunTriggers(TriggerEvents.AfterSet);
            }
            return Owner;
        }

        private IEnumerable<T> GetIndividualValues(T value)
        {
            return ((Enum)(object)value).GetIndividualFlags().Cast<T>();
        }

        protected internal override string ConvertValueToString(T value)
        {
            var individualValues = GetIndividualValues(value);
            return ConvertIndividualValuesToString(individualValues);
        }

        protected string ConvertIndividualValuesToString(IEnumerable<T> values)
        {
            string[] stringValues = values.Select(x => TermResolver.ToString(x, ValueTermOptions)).ToArray();

            if (stringValues.Length == 0)
                return "<none>";
            if (stringValues.Length == 1)
                return stringValues[0];
            else if (stringValues.Any(x => x.Contains(',')))
                return "\"{0}\"".FormatWith(string.Join("\", \"", stringValues));
            else
                return string.Join(", ", stringValues);
        }
    }
}
