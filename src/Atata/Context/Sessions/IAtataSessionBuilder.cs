namespace Atata;

public interface IAtataSessionBuilder : ICloneable
{
    string Name { get; }

    AtataSessionStart Start { get; }

    AtataSession Build(AtataContext context);
}
