namespace Atata;

public abstract class AtataSessionBuilder : IAtataSessionBuilder
{
    public string Name { get; internal set; }

    public AtataSessionStart Start { get; internal set; }

    public abstract AtataSession Build(AtataContext context);

    object ICloneable.Clone()
    {
        var copy = (AtataSessionBuilder)MemberwiseClone();

        OnClone(copy);

        return copy;
    }

    protected virtual void OnClone(AtataSessionBuilder copy)
    {
    }
}
