namespace Atata;

public interface IAtataSessionBuilder
{
    string Name { get; }

    AtataSessionStartScopes StartScopes { get; set; }

    Task<AtataSession> BuildAsync(AtataContext context);

    /// <summary>
    /// Creates a copy of the current builder.
    /// </summary>
    /// <returns>The copied builder instance.</returns>
    IAtataSessionBuilder Clone();
}
