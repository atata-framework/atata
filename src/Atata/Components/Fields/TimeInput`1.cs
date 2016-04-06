using System;
using System.Globalization;

namespace Atata
{
    [ControlDefinition("input[@type='text' or @type='time' or not(@type)]")]
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
            string stringValue = ConvertValueToString(value);
            Scope.FillInWith(stringValue);
        }

        protected internal override string ConvertValueToString(TimeSpan? value)
        {
            return value != null ? value.Value.ToString(format, cultureInfo) : string.Empty;
        }

        protected internal override void ApplyMetadata(UIComponentMetadata metadata)
        {
            cultureInfo = metadata.GetCulture();
            format = metadata.GetFormat(GetType());
        }
    }
}
