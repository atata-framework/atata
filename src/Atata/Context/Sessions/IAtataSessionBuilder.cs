namespace Atata;

public interface IAtataSessionBuilder : ICloneable
{
    string Name { get; }

    AtataSessionStartScopes StartScopes { get; set; }

    AtataSession Build(AtataContext context);
}
