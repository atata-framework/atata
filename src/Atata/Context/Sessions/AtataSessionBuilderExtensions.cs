namespace Atata;

public static class AtataSessionBuilderExtensions
{
    public static TBuilder WithName<TBuilder>(this TBuilder builder, string name)
        where TBuilder : AtataSessionBuilder<TBuilder>
    {
        builder.Name = name;
        return builder;
    }

    public static TBuilder WithStart<TBuilder>(this TBuilder builder, AtataSessionStart sessionStart)
        where TBuilder : AtataSessionBuilder<TBuilder>
    {
        builder.Start = sessionStart;
        return builder;
    }
}
