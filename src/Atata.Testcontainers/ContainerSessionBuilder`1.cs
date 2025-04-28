namespace Atata.Testcontainers;

/// <summary>
/// Represents a builder for creating and configuring a container session for a specific container type.
/// </summary>
/// <typeparam name="TContainer">The type of the container.</typeparam>
public class ContainerSessionBuilder<TContainer> : ContainerSessionBuilder<TContainer, ContainerSession<TContainer>, ContainerSessionBuilder<TContainer>>
    where TContainer : IContainer
{
}
