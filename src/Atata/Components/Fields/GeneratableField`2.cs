namespace Atata
{
    public abstract class GeneratableField<T, TOwner> : EditableField<T, TOwner>, IGeneratableField<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected GeneratableField()
        {
        }

        public TOwner SetGenerated()
        {
            T value = Generate();
            return Set(value);
        }

        public TOwner SetGenerated(out T value)
        {
            value = Generate();
            return Set(value);
        }

        protected abstract T Generate();
    }
}
