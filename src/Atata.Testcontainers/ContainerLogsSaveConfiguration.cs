namespace Atata.Testcontainers;

/// <summary>
/// A configuration for saving container logs.
/// </summary>
public sealed class ContainerLogsSaveConfiguration
{
    /// <summary>
    /// Gets the default configuration instance.
    /// </summary>
    public static ContainerLogsSaveConfiguration Default { get; } = new();

    /// <summary>
    /// Gets or sets the template for the stdout log file name.
    /// The default value is "{container-image-fullname}-stdout.log".
    /// </summary>
    public string StdoutFileNameTemplate { get; set; } = "{container-image-fullname}-stdout.log";

    /// <summary>
    /// Gets or sets a value indicating whether to include the stdout log file.
    /// The default value is <c>true</c>.
    /// </summary>
    public bool StdoutFileIncluded { get; set; } = true;

    /// <summary>
    /// Gets or sets the template for the stderr log file name.
    /// The default value is "{container-image-fullname}-stderr.log".
    /// </summary>
    public string StderrFileNameTemplate { get; set; } = "{container-image-fullname}-stderr.log";

    /// <summary>
    /// Gets or sets a value indicating whether to include the stderr log file.
    /// The default value is <c>true</c>.
    /// </summary>
    public bool StderrFileIncluded { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to include timestamps in the logs.
    /// The default value is <c>true</c>.
    /// </summary>
    public bool TimestampsIncluded { get; set; } = true;

    internal bool IsAnyLogIncluded =>
        StdoutFileIncluded || StderrFileIncluded;

    /// <summary>
    /// Creates a new instance of <see cref="ContainerLogsSaveConfiguration"/> that is a copy of the current instance.
    /// </summary>
    /// <returns>A new <see cref="ContainerLogsSaveConfiguration"/> instance with the same property values.</returns>
    public ContainerLogsSaveConfiguration Clone() =>
        new()
        {
            StdoutFileNameTemplate = StdoutFileNameTemplate,
            StdoutFileIncluded = StdoutFileIncluded,
            StderrFileNameTemplate = StderrFileNameTemplate,
            StderrFileIncluded = StderrFileIncluded,
            TimestampsIncluded = TimestampsIncluded
        };
}
