namespace Atata.Testcontainers;

/// <summary>
/// <para>
/// Represents a session that manages <see cref="IContainer"/> instance
/// and provides a set of functionality to manipulate the container.
/// </para>
/// <para>
/// The session has additional variables in <see cref="AtataSession.Variables" />:
/// <c>{container-image-fullname}</c>, <c>{container-image-repository}</c>, <c>{container-image-registry}</c>,
/// <c>{container-image-tag}</c>, <c>{container-image-digest}</c>.
/// </para>
/// </summary>
public class ContainerSession : ContainerSession<IContainer>
{
}
