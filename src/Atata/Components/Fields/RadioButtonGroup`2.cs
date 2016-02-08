////using OpenQA.Selenium;
////using System;
////using System.Linq;

////namespace Atata
////{
////    public class RadioButtonGroup<TEnum, TOwner> : GroupField<TEnum, TOwner>
////        where TEnum : struct, IComparable, IFormattable
////        where TOwner : PageObject<TOwner>
////    {
////        protected override TEnum GetValue()
////        {
////            var enumValues = Enum.GetValues(typeof(TEnum));

////            return enumValues.Cast<TEnum>().FirstOrDefault(x => IsChecked(x));
////        }

////        protected override void SetValue(TEnum value)
////        {
////            IWebElement element = GetItem(value);
////            if (!element.Selected)
////                element.Click();
////        }
////    }
////}
