namespace Atata
{
    public static class EditableFieldExtensions
    {
        public static TOwner SetRandom<TOwner>(this EditableField<decimal?, TOwner> field, out int? value)
            where TOwner : PageObject<TOwner>
        {
            decimal? decimalValue;
            field.SetRandom(out decimalValue);

            value = (int?)decimalValue;

            return field.Owner;
        }
    }
}
