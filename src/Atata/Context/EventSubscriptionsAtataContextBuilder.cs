namespace Atata;

/// <summary>
/// Represents the builder of event subscriptions.
/// </summary>
public class EventSubscriptionsAtataContextBuilder : AtataContextBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventSubscriptionsAtataContextBuilder"/> class.
    /// </summary>
    /// <param name="buildingContext">The building context.</param>
    public EventSubscriptionsAtataContextBuilder(AtataBuildingContext buildingContext)
        : base(buildingContext)
    {
    }

    /// <summary>
    /// Adds the specified event handler as a subscription to the <typeparamref name="TEvent"/>.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="eventHandler">The event handler.</param>
    /// <returns>The same <see cref="EventSubscriptionsAtataContextBuilder"/> instance.</returns>
    public EventSubscriptionsAtataContextBuilder Add<TEvent>(Action eventHandler) =>
        Add(typeof(TEvent), new ActionEventHandler<TEvent>(eventHandler));

    /// <inheritdoc cref="Add{TEvent}(Action)"/>
    public EventSubscriptionsAtataContextBuilder Add<TEvent>(Action<TEvent> eventHandler) =>
        Add(typeof(TEvent), new ActionEventHandler<TEvent>(eventHandler));

    /// <inheritdoc cref="Add{TEvent}(Action)"/>
    public EventSubscriptionsAtataContextBuilder Add<TEvent>(Action<TEvent, AtataContext> eventHandler) =>
        Add(typeof(TEvent), new ActionEventHandler<TEvent>(eventHandler));

    /// <summary>
    /// Adds the created instance of <typeparamref name="TEventHandler"/> as a subscription to the <typeparamref name="TEvent"/>.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <typeparam name="TEventHandler">The type of the event handler.</typeparam>
    /// <returns>The same <see cref="EventSubscriptionsAtataContextBuilder"/> instance.</returns>
    public EventSubscriptionsAtataContextBuilder Add<TEvent, TEventHandler>()
        where TEventHandler : class, IEventHandler<TEvent>, new()
        =>
        Add(typeof(TEvent), new TEventHandler());

    /// <inheritdoc cref="Add{TEvent}(Action)"/>
    public EventSubscriptionsAtataContextBuilder Add<TEvent>(IEventHandler<TEvent> eventHandler) =>
        Add(typeof(TEvent), eventHandler);

    /// <summary>
    /// Adds the created instance of <paramref name="eventHandlerType"/> as a subscription to the event type
    /// that is read from <see cref="IEventHandler{TEvent}"/> generic argument that <paramref name="eventHandlerType"/> should implement.
    /// </summary>
    /// <param name="eventHandlerType">Type of the event handler.</param>
    /// <returns>The same <see cref="EventSubscriptionsAtataContextBuilder"/> instance.</returns>
    public EventSubscriptionsAtataContextBuilder Add(Type eventHandlerType)
    {
        eventHandlerType.CheckNotNull(nameof(eventHandlerType));

        Type expectedInterfaceType = typeof(IEventHandler<>);

        Type eventHanderType = eventHandlerType.GetGenericInterfaceType(expectedInterfaceType)
            ?? throw new ArgumentException($"'{nameof(eventHandlerType)}' of {eventHandlerType.FullName} type doesn't implement {expectedInterfaceType.FullName}.", nameof(eventHandlerType));

        Type eventType = eventHanderType.GetGenericArguments()[0];

        var eventHandler = ActivatorEx.CreateInstance(eventHandlerType);
        return Add(eventType, eventHandler);
    }

    /// <summary>
    /// Adds the created instance of <paramref name="eventHandlerType"/> as a subscription to the <paramref name="eventType"/>.
    /// </summary>
    /// <param name="eventType">Type of the event.</param>
    /// <param name="eventHandlerType">Type of the event handler.</param>
    /// <returns>The same <see cref="EventSubscriptionsAtataContextBuilder"/> instance.</returns>
    public EventSubscriptionsAtataContextBuilder Add(Type eventType, Type eventHandlerType)
    {
        eventType.CheckNotNull(nameof(eventType));
        eventHandlerType.CheckNotNull(nameof(eventHandlerType));

        Type expectedType = typeof(IEventHandler<>).MakeGenericType(eventType);

        if (!expectedType.IsAssignableFrom(eventHandlerType))
            throw new ArgumentException($"'{nameof(eventHandlerType)}' of {eventHandlerType.FullName} type doesn't implement {expectedType.FullName}.", nameof(eventHandlerType));

        var eventHandler = ActivatorEx.CreateInstance(eventHandlerType);
        return Add(eventType, eventHandler);
    }

    private EventSubscriptionsAtataContextBuilder Add(Type eventType, object eventHandler)
    {
        BuildingContext.EventSubscriptions.Add(new EventSubscriptionItem(eventType, eventHandler));
        return this;
    }

    /// <summary>
    /// Defines that an error occurred during the NUnit test execution
    /// should be added to the log during <see cref="AtataContext"/> deinitialization.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public new AtataContextBuilder LogNUnitError() =>
        Add(new LogNUnitErrorEventHandler());

    /// <summary>
    /// Defines that an error occurred during the NUnit test execution
    /// should be captured by a screenshot during <see cref="AtataContext"/> deinitialization.
    /// </summary>
    /// <param name="title">The screenshot title.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public new AtataContextBuilder TakeScreenshotOnNUnitError(string title = "Failed") =>
        Add(new TakeScreenshotOnNUnitErrorEventHandler(title));

    /// <inheritdoc cref="TakeScreenshotOnNUnitError(string)"/>
    /// <param name="kind">The kind of a screenshot.</param>
    /// <param name="title">The screenshot title.</param>
    public new AtataContextBuilder TakeScreenshotOnNUnitError(ScreenshotKind kind, string title = "Failed") =>
        Add(new TakeScreenshotOnNUnitErrorEventHandler(kind, title));

    /// <summary>
    /// Defines that an error occurred during the NUnit test execution
    /// should be captured by a page snapshot during <see cref="AtataContext"/> deinitialization.
    /// </summary>
    /// <param name="title">The snapshot title.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public new AtataContextBuilder TakePageSnapshotOnNUnitError(string title = "Failed") =>
        Add(new TakePageSnapshotOnNUnitErrorEventHandler(title));

    /// <summary>
    /// Defines that after <see cref="AtataContext"/> deinitialization the files stored in Artifacts directory
    /// should be added to NUnit <c>TestContext</c>.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder AddArtifactsToNUnitTestContext() =>
        Add(new AddArtifactsToNUnitTestContextEventHandler());

    /// <summary>
    /// Defines that after <see cref="AtataContext"/> deinitialization the files stored in the directory
    /// specified by <paramref name="directoryPath"/> should be added to NUnit <c>TestContext</c>.
    /// Directory path supports template variables.
    /// </summary>
    /// <param name="directoryPath">The directory path.</param>
    /// <returns>The <see cref="AtataContextBuilder" /> instance.</returns>
    public AtataContextBuilder AddDirectoryFilesToNUnitTestContext(string directoryPath)
    {
        directoryPath.CheckNotNull(nameof(directoryPath));
        return AddDirectoryFilesToNUnitTestContext(_ => directoryPath);
    }

    /// <inheritdoc cref="AddDirectoryFilesToNUnitTestContext(Func{AtataContext, string})"/>
    public AtataContextBuilder AddDirectoryFilesToNUnitTestContext(Func<string> directoryPathBuilder)
    {
        directoryPathBuilder.CheckNotNull(nameof(directoryPathBuilder));
        return AddDirectoryFilesToNUnitTestContext(_ => directoryPathBuilder.Invoke());
    }

    /// <summary>
    /// Defines that after <see cref="AtataContext"/> deinitialization the files stored in the directory
    /// specified by <paramref name="directoryPathBuilder"/> should be added to NUnit <c>TestContext</c>.
    /// Directory path supports template variables.
    /// </summary>
    /// <param name="directoryPathBuilder">The directory path builder.</param>
    /// <returns>The <see cref="AtataContextBuilder" /> instance.</returns>
    public AtataContextBuilder AddDirectoryFilesToNUnitTestContext(Func<AtataContext, string> directoryPathBuilder)
    {
        directoryPathBuilder.CheckNotNull(nameof(directoryPathBuilder));
        return Add(new AddDirectoryFilesToNUnitTestContextEventHandler(directoryPathBuilder));
    }
}
