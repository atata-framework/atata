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

        public static TOwner SetRandom<TOwner>(this EditableField<decimal?, TOwner> field, out int value)
            where TOwner : PageObject<TOwner>
        {
            decimal? decimalValue;
            field.SetRandom(out decimalValue);

            value = (int)decimalValue;

            return field.Owner;
        }

        public static TOwner SetRandom<TData, TOwner>(this EditableField<TData?, TOwner> field, out TData value)
            where TData : struct
            where TOwner : PageObject<TOwner>
        {
            TData? nullableValue;
            field.SetRandom(out nullableValue);

            value = (TData)nullableValue;

            return field.Owner;
        }
    }
}
