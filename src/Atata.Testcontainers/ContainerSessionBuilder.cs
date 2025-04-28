namespace Atata.Testcontainers;

/// <summary>
/// Represents a builder for creating and configuring a container session.
/// </summary>
public class ContainerSessionBuilder : ContainerSessionBuilder<IContainer, ContainerSession, ContainerSessionBuilder>
{
    /// <summary>
    /// Configures the builder to use a specific container builder.
    /// </summary>
    /// <param name="containerBuilder">The function to configure the container builder.</param>
    /// <returns>The same <see cref="ContainerSessionBuilder"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="containerBuilder"/> is <see langword="null"/>.</exception>
    public ContainerSessionBuilder Use(Func<ContainerBuilder, ContainerBuilder> containerBuilder)
    {
        Guard.ThrowIfNull(containerBuilder);

        return Use(() => containerBuilder.Invoke(new ContainerBuilder()));
    }
}
