using OpenQA.Selenium;
using System;
using System.Linq;

namespace Atata
{
    [UIComponent("input[@type='checkbox']", IgnoreNameEndings = "CheckBoxes,CheckBoxList,CheckBoxGroup,Options,OptionGroup")]
    public class CheckBoxList<T, TOwner> : OptionList<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
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
            // TODO: Implement.
            return default(T);
        }

        protected override void SetValue(T value)
        {
            if (value == null)
                throw new ArgumentNullException("value", "Cannot set 'null' to CheckBoxList control.");

            IWebElement[] elements = GetItemElements();

            // TODO: Implement.
        }
    }
}
