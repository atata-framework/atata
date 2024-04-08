namespace Atata;

public abstract class AtataSession
{
    protected AtataSession(AtataContext context) =>
        OwnerContext = Context = context.CheckNotNull(nameof(context));

    public AtataContext OwnerContext { get; }

    public AtataContext Context { get; internal set; }

    public string Name { get; internal set; }

    /// <summary>
    /// Gets the instance of the log manager associated with the session.
    /// </summary>
    public ILogManager Log { get; internal set; }

    /// <summary>
    /// Gets the event bus, which can used to subscribe to and publish events.
    /// </summary>
    protected IEventBus EventBus => Context.EventBus;

    public void SetAsCurrent() =>
        Context.Sessions.SetCurrent(this);

    public void Start()
    {
    }
}
