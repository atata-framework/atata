namespace Atata;

/// <summary>
/// Represents the configuration of browser logs monitoring and handling.
/// </summary>
public sealed class BrowserLogsConfiguration : ICloneable
{
    /// <summary>
    /// Gets or sets a value indicating whether the browser log should be transferred to Atata logging system.
    /// The default value is <see langword="false"/>.
    /// </summary>
    public bool Log { get; set; }

    /// <summary>
    /// Gets or sets the minimum log level on which to raise warnings.
    /// The default value is <see langword="null"/>, meaning that warning is disabled.
    /// For example, setting the <see cref="LogLevel.Warn"/> value will mean to warn on
    /// browser log entries with <see cref="LogLevel.Warn"/> level and higher,
    /// which are <see cref="LogLevel.Error"/> and <see cref="LogLevel.Fatal"/>.
    /// </summary>
    public LogLevel? MinLevelOfWarning { get; set; }

    internal bool HasPropertiesToUse =>
        Log || MinLevelOfWarning is not null;

    /// <inheritdoc cref="Clone"/>
    object ICloneable.Clone() => Clone();

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new object that is a copy of this instance.
    /// </returns>
    internal BrowserLogsConfiguration Clone() =>
        (BrowserLogsConfiguration)MemberwiseClone();
}
