namespace Atata;

public abstract class AtataSessionBuilder<TBuilder> : IAtataSessionBuilder
    where TBuilder : AtataSessionBuilder<TBuilder>
{
    public string Name { get; internal set; }

    public AtataSessionStart Start { get; internal set; }

    public abstract AtataSession Build(AtataContext context);

    object ICloneable.Clone()
    {
        var copy = (TBuilder)MemberwiseClone();

        OnClone(copy);

        return copy;
    }

    protected virtual void OnClone(TBuilder copy)
    {
    }
}
