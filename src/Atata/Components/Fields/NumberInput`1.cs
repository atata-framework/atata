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
            Scope.FillInWith(value != null ? value.Value.ToString(format, cultureInfo) : string.Empty);
        }

        protected override decimal? Generate()
        {
            return generatableAttribute != null ? (decimal?)ValueGenerator.GenerateDecimal(generatableAttribute.Min, generatableAttribute.Max, generatableAttribute.Precision) : null;
        }

        protected internal override void ApplyMetadata(UIComponentMetadata metadata)
        {
            generatableAttribute = metadata.GetFirstOrDefaultPropertyAttribute<GeneratableNumberAttribute>();
            cultureInfo = metadata.GetCulture();
            format = metadata.GetFormat(GetType());
        }
    }
}
