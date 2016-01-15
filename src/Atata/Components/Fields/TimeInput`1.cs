using System;
using System.Globalization;

namespace Atata
{
    [UIComponent("input[@type='text' or @type='time' or not(@type)]")]
    public class TimeInput<TOwner> : EditableField<TimeSpan?, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private CultureInfo cultureInfo;
        private string format;

        protected override TimeSpan? GetValue()
        {
            TimeSpan value;
            return TimeSpan.TryParse(Scope.GetValue(), cultureInfo, out value)
                ? (TimeSpan?)value
                : null;
        }

        protected override void SetValue(TimeSpan? value)
        {
            if (value.HasValue)
            {
                string stringValue = value.Value.ToString(format, cultureInfo);
                Scope.FillInWith(stringValue);
            }
            else
            {
                Scope.Clear();
            }
        }

        protected internal override void ApplyMetadata(UIComponentMetadata metadata)
        {
            cultureInfo = metadata.GetCulture();
            format = metadata.GetFormat(GetType());
        }
    }
}
