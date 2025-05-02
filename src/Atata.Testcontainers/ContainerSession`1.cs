namespace Atata.Testcontainers;

/// <summary>
/// <para>
/// Represents a session that manages <typeparamref name="TContainer" /> instance
/// and provides a set of functionality to manipulate the container.
/// </para>
/// <para>
/// The session has additional variables in <see cref="AtataSession.Variables" />:
/// <c>{container-image-fullname}</c>, <c>{container-image-repository}</c>, <c>{container-image-registry}</c>,
/// <c>{container-image-tag}</c>, <c>{container-image-digest}</c>.
/// </para>
/// </summary>
/// <typeparam name="TContainer">The type of the container.</typeparam>
public class ContainerSession<TContainer> : AtataSession
    where TContainer : IContainer
{
    /// <summary>
    /// Gets the current <see cref="ContainerSession{TContainer}"/> instance in scope of <see cref="AtataContext.Current"/>.
    /// Returns <see langword="null"/> if there is no such session or <see cref="AtataContext.Current"/> is <see langword="null"/>.
    /// </summary>
    public static ContainerSession<TContainer>? Current =>
        AtataContext.Current?.Sessions.GetOrNull<ContainerSession<TContainer>>();

    /// <summary>
    /// Gets the container.
    /// </summary>
    public TContainer Container { get; internal set; } = default!;

    internal ContainerLogsSaveConfiguration LogsSaveConfiguration { get; set; } = null!;

    protected override async Task StartAsync(CancellationToken cancellationToken = default) =>
        await Log.ExecuteSectionAsync(
            new LogSection("Start container", LogLevel.Trace),
            async () => await Container.StartAsync(cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);

    /// <summary>
    /// Extracts the file from container to Artifacts directory.
    /// </summary>
    /// <param name="containerFilePath">The container file path.</param>
    /// <param name="artifactType">Type of the artifact.</param>
    /// <param name="artifactTitle">The artifact title.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task{TResult}"/> of <see cref="FileSubject"/> object.</returns>
    public async Task<FileSubject> ExtractFileToArtifactsAsync(
        string containerFilePath,
        string? artifactType = null,
        string? artifactTitle = null,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhitespace(containerFilePath);

        string destinationFileName = Path.GetFileName(containerFilePath);

        return await Log.ExecuteSectionAsync(
            new LogSection($"Extract \"{containerFilePath}\" file to Artifacts", LogLevel.Trace),
            async () =>
            {
                byte[] fileBytes = await Container.ReadFileAsync(containerFilePath, cancellationToken).ConfigureAwait(false);
                return Context.AddArtifact(destinationFileName, fileBytes, artifactType, artifactTitle);
            })
            .ConfigureAwait(false);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        if (Container is not null)
        {
            try
            {
                await Log.ExecuteSectionAsync(
                    new LogSection("Stop container", LogLevel.Trace),
                    async () => await Container.StopAsync().ConfigureAwait(false))
                    .ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Container stopping failed.");
            }

            if (LogsSaveConfiguration.IsAnyLogIncluded)
            {
                try
                {
                    await SaveLogsAsync().ConfigureAwait(false);
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "Container logs saving failed.");
                }
            }

            try
            {
                await Log.ExecuteSectionAsync(
                    new LogSection("Dispose container", LogLevel.Trace),
                    async () => await Container.DisposeAsync().ConfigureAwait(false))
                    .ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Container disposing failed.");
            }
        }

        await base.DisposeAsyncCore().ConfigureAwait(false);
    }

    private async Task SaveLogsAsync() =>
        await Log.ExecuteSectionAsync(
            new LogSection("Save container logs", LogLevel.Trace),
            DoSaveLogsAsync)
            .ConfigureAwait(false);

    private async Task DoSaveLogsAsync()
    {
        var (stdout, stderr) = await Container.GetLogsAsync(timestampsEnabled: LogsSaveConfiguration.TimestampsIncluded)
            .ConfigureAwait(false);

        if (LogsSaveConfiguration.StdoutFileIncluded && stdout?.Length > 0)
            SaveLogToArtifacts(LogsSaveConfiguration.StdoutFileNameTemplate, stdout);

        if (LogsSaveConfiguration.StderrFileIncluded && stderr?.Length > 0)
            SaveLogToArtifacts(LogsSaveConfiguration.StderrFileNameTemplate, stderr);
    }

    private void SaveLogToArtifacts(string fileNameTemplate, string content)
    {
        string fileName = Variables.FillPathTemplateString(fileNameTemplate);
        Context.AddArtifact(fileName, content, ArtifactTypes.Log);
    }
}
