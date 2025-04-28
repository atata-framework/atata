using Atata.Testcontainers;

namespace Atata;

public static class ContainerSessionAtataSessionCollectionExtensions
{
    /// <summary>
    /// Creates a new <see cref="ContainerSessionBuilder"/> and adds it to the collection.
    /// </summary>
    /// <param name="collection">The session collection.</param>
    /// <param name="configure">An action delegate to configure the <see cref="ContainerSessionBuilder"/>.</param>
    /// <returns>The created <see cref="ContainerSessionBuilder"/> instance.</returns>
    public static ContainerSessionBuilder AddContainer(
        this AtataSessionCollection collection,
        Action<ContainerSessionBuilder>? configure = null)
        =>
        collection.Add(configure);

    /// <summary>
    /// Creates a new <see cref="ContainerSessionBuilder{TContainer}"/> and adds it to the collection.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    /// <param name="collection">The session collection.</param>
    /// <param name="configure">An action delegate to configure the <see cref="ContainerSessionBuilder{TContainer}"/>.</param>
    /// <returns>The created <see cref="ContainerSessionBuilder{TContainer}"/> instance.</returns>
    public static ContainerSessionBuilder<TContainer> AddContainer<TContainer>(
        this AtataSessionCollection collection,
        Action<ContainerSessionBuilder<TContainer>>? configure = null)
        where TContainer : IContainer
        =>
        collection.Add(configure);
}
