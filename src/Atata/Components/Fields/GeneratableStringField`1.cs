namespace Atata
{
    public abstract class GeneratableStringField<TOwner> : GeneratableField<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private GeneratableStringAttribute generatableAttribute;

        protected GeneratableStringField()
        {
        }

        protected override string Generate()
        {
            string format = NormalizeFormat(generatableAttribute.Format);
            return ValueGenerator.GenerateString(format, generatableAttribute.NumberOfCharacters);
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
