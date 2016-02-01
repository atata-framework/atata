using Humanizer;
using System;

namespace Atata
{
    public class Select<TEnum, TOwner> : SelectBase<TEnum, TOwner>
        where TEnum : struct, IComparable, IFormattable
        where TOwner : PageObject<TOwner>
    {
        protected override TEnum GetValue()
        {
            return GetSelectedOptionValue().DehumanizeTo<TEnum>();
        }

        protected override void SetValue(TEnum value)
        {
            string stringValue = ConvertValueToString(value);
            SetSelectedOptionValue(stringValue);
        }

        protected internal override string ConvertValueToString(TEnum value)
        {
            return ((Enum)(object)value).ToTitleString();
        }
    }
}
