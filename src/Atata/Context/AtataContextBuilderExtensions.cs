namespace Atata;

/// <summary>
/// Provides a set of extension methods for <see cref="AtataContextBuilder"/>.
/// </summary>
public static class AtataContextBuilderExtensions
{
    /// <summary>
    /// Specifies the properties map for the context.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the builder.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="propertiesMap">The properties map.</param>
    /// <returns>The same builder instance.</returns>
    public static TBuilder WithProperties<TBuilder>(this TBuilder builder, Dictionary<string, object> propertiesMap)
        where TBuilder : AtataContextBuilder, IHasContext<object>
    {
        propertiesMap.CheckNotNull(nameof(propertiesMap));

        builder.CreateObjectMapper().Map(propertiesMap, builder.Context);

        return builder;
    }
}
