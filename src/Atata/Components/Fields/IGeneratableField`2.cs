namespace Atata
{
    public interface IGeneratableField<T, TParent>
    {
        TParent SetGenerated();
        TParent SetGenerated(out T value);
    }
}
