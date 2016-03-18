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
            if (value == null)
                throw new ArgumentNullException("value", "Cannot set 'null' to CheckBoxList control.");

            IWebElement[] elements = GetItemElements();
            foreach (IWebElement element in elements)
            {
                T elementValue = GetElementValue(element);
                if (HasValue(value, elementValue) != element.Selected)
                    element.Click();
            }
        }

        private bool HasValue(T joinedValue, T value)
        {
            return ((Enum)(object)joinedValue).HasFlag((Enum)(object)value);
        }
    }
}
