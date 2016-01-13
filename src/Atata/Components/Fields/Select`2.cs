using Humanizer;
using System;

namespace Atata
{
    [UIComponent("select")]
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
            // TODO: Perform custom humanization.
            SetSelectedOptionValue(((Enum)(object)value).ToTitleString());
        }
    }
}
