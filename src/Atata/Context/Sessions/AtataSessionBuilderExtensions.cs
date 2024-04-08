namespace Atata;

public static class AtataSessionBuilderExtensions
{
    public static TBuilder WithName<TBuilder>(this TBuilder builder, string name)
        where TBuilder : AtataSessionBuilder
    {
        builder.Name = name;
        return builder;
    }

    public static TBuilder WithStart<TBuilder>(this TBuilder builder, AtataSessionStart sessionStart)
        where TBuilder : AtataSessionBuilder
    {
        builder.Start = sessionStart;
        return builder;
    }
}
