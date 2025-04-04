#nullable enable

namespace Atata;

public abstract class WaitUntilAttribute : TriggerAttribute
{
    protected WaitUntilAttribute(Until until, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority) =>
        Until = until;

    /// <summary>
    /// Gets the waiting approach.
    /// </summary>
    public Until Until { get; }

    /// <summary>
    /// Gets or sets a value indicating whether to throw the exception on the presence (exists or visible) failure.
    /// The default value is <see langword="true"/>.
    /// </summary>
    public bool ThrowOnPresenceFailure
    {
        get => WaitOptions.ThrowOnPresenceFailure;
        set => WaitOptions.ThrowOnPresenceFailure = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether to throw the exception on the absence (missing or hidden) failure.
    /// The default value is <see langword="true"/>.
    /// </summary>
    public bool ThrowOnAbsenceFailure
    {
        get => WaitOptions.ThrowOnAbsenceFailure;
        set => WaitOptions.ThrowOnAbsenceFailure = value;
    }

    /// <summary>
    /// Gets or sets the presence (exists or visible) timeout in seconds.
    /// The default value is taken from <c>AtataContext.Current.RetryTimeout.TotalSeconds</c>.
    /// </summary>
    public double PresenceTimeout
    {
        get => WaitOptions.PresenceTimeout;
        set => WaitOptions.PresenceTimeout = value;
    }

    /// <summary>
    /// Gets or sets the absence (missing or hidden) timeout in seconds.
    /// The default value is taken from <c>AtataContext.Current.RetryTimeout.TotalSeconds</c>.
    /// </summary>
    public double AbsenceTimeout
    {
        get => WaitOptions.AbsenceTimeout;
        set => WaitOptions.AbsenceTimeout = value;
    }

    /// <summary>
    /// Gets or sets the retry interval.
    /// The default value is taken from <c>AtataContext.Current.RetryInterval.TotalSeconds</c>.
    /// </summary>
    public double RetryInterval
    {
        get => WaitOptions.RetryInterval;
        set => WaitOptions.RetryInterval = value;
    }

    /// <summary>
    /// Gets the wait options.
    /// </summary>
    protected WaitOptions WaitOptions { get; } = new();
}
