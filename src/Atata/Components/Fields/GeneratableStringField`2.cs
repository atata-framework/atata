namespace Atata
{
    public abstract class GeneratableStringField<T, TOwner> : GeneratableField<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private GeneratableStringAttribute generatableAttribute;

        protected GeneratableStringField()
        {
        }

        protected override T Generate()
        {
            string format = NormalizeFormat(generatableAttribute.Format);
            string generatedString = ValueGenerator.GenerateString(format, generatableAttribute.NumberOfCharacters);
            return TermResolver.FromString<T>(generatedString, ValueTermOptions);
        }

        private string NormalizeFormat(string format)
        {
            if (string.IsNullOrEmpty(format))
                return "{0}";
            else if (!format.Contains("{0}"))
                return format + "{0}";
            else
                return format;
        }

        protected internal override void ApplyMetadata(UIComponentMetadata metadata)
        {
            generatableAttribute = metadata.GetFirstOrDefaultDeclaringAttribute<GeneratableStringAttribute>() ?? new GeneratableStringAttribute();
        }
    }
}
