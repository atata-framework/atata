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
            return ValueGenerator.GenerateString(
                generatableAttribute.Prefix,
                generatableAttribute.NumberOfCharacters,
                generatableAttribute.Separator);
        }

        protected internal override void ApplyMetadata(UIComponentMetadata metadata)
        {
            generatableAttribute = metadata.GetFirstOrDefaultDeclaringAttribute<GeneratableStringAttribute>() ?? new GeneratableStringAttribute();
        }
    }
}
