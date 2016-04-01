using System.Globalization;

namespace Atata
{
    [UIComponent("input[@type='text' or @type='number' or @type='tel' or not(@type)]")]
    public class NumberInput<TOwner> : GeneratableField<decimal?, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private GeneratableNumberAttribute generatableAttribute;
        private CultureInfo cultureInfo;
        private string format;

        protected override decimal? GetValue()
        {
            decimal value;
            return decimal.TryParse(Scope.GetValue(), NumberStyles.None, cultureInfo, out value) ? (decimal?)value : null;
        }

        protected override void SetValue(decimal? value)
        {
            string stringValue = ConvertValueToString(value);
            Scope.FillInWith(stringValue);
        }

        protected override decimal? Generate()
        {
            return generatableAttribute != null ? (decimal?)Randomizer.GetDecimal(generatableAttribute.Min, generatableAttribute.Max, generatableAttribute.Precision) : null;
        }

        protected internal override string ConvertValueToString(decimal? value)
        {
            return value != null ? value.Value.ToString(format, cultureInfo) : string.Empty;
        }

        protected internal override void ApplyMetadata(UIComponentMetadata metadata)
        {
            generatableAttribute = metadata.GetFirstOrDefaultDeclaringAttribute<GeneratableNumberAttribute>();
            cultureInfo = metadata.GetCulture();
            format = metadata.GetFormat(GetType());
        }
    }
}
