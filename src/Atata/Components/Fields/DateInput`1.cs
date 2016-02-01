using System;
using System.Globalization;

namespace Atata
{
    [UIComponent("input[@type='text' or @type='date' or not(@type)]")]
    public class DateInput<TOwner> : EditableField<DateTime?, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private const string DefaultFormat = "d";

        private CultureInfo cultureInfo;
        private string format;

        protected override DateTime? GetValue()
        {
            DateTime value;
            return DateTime.TryParse(Scope.GetValue(), cultureInfo, DateTimeStyles.None, out value)
                ? (DateTime?)value
                : null;
        }

        protected override void SetValue(DateTime? value)
        {
            string stringValue = ConvertValueToString(value);
            Scope.FillInWith(stringValue);
        }

        protected internal override string ConvertValueToString(DateTime? value)
        {
            return value != null ? value.Value.ToString(format, cultureInfo) : string.Empty;
        }

        protected internal override void ApplyMetadata(UIComponentMetadata metadata)
        {
            cultureInfo = metadata.GetCulture();
            format = metadata.GetFormat(GetType()) ?? DefaultFormat;
        }
    }
}
