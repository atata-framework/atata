namespace Atata;

/// <summary>
/// Represents the builder of screenshot consumers.
/// </summary>
public class ScreenshotConsumersAtataContextBuilder : AtataContextBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ScreenshotConsumersAtataContextBuilder"/> class.
    /// </summary>
    /// <param name="buildingContext">The building context.</param>
    public ScreenshotConsumersAtataContextBuilder(AtataBuildingContext buildingContext)
        : base(buildingContext)
    {
    }

    /// <summary>
    /// Adds the screenshot consumer.
    /// </summary>
    /// <typeparam name="TScreenshotConsumer">The type of the screenshot consumer.</typeparam>
    /// <returns>The <see cref="ScreenshotConsumerAtataContextBuilder{TScreenshotConsumer}"/> instance.</returns>
    public ScreenshotConsumerAtataContextBuilder<TScreenshotConsumer> Add<TScreenshotConsumer>()
        where TScreenshotConsumer : IScreenshotConsumer, new() =>
        Add(new TScreenshotConsumer());

    /// <summary>
    /// Adds the screenshot consumer.
    /// </summary>
    /// <param name="typeNameOrAlias">The type name or alias of the log consumer.</param>
    /// <returns>The <see cref="ScreenshotConsumerAtataContextBuilder{TScreenshotConsumer}"/> instance.</returns>
    public ScreenshotConsumerAtataContextBuilder<IScreenshotConsumer> Add(string typeNameOrAlias)
    {
        IScreenshotConsumer consumer = ScreenshotConsumerAliases.Resolve(typeNameOrAlias);

        return Add(consumer);
    }

    /// <summary>
    /// Adds the screenshot consumer.
    /// </summary>
    /// <typeparam name="TScreenshotConsumer">The type of the screenshot consumer.</typeparam>
    /// <param name="consumer">The screenshot consumer.</param>
    /// <returns>The <see cref="ScreenshotConsumerAtataContextBuilder{TScreenshotConsumer}"/> instance.</returns>
    public ScreenshotConsumerAtataContextBuilder<TScreenshotConsumer> Add<TScreenshotConsumer>(TScreenshotConsumer consumer)
        where TScreenshotConsumer : IScreenshotConsumer
    {
        consumer.CheckNotNull(nameof(consumer));

        BuildingContext.ScreenshotConsumers.Add(consumer);
        return new ScreenshotConsumerAtataContextBuilder<TScreenshotConsumer>(consumer, BuildingContext);
    }

    /// <summary>
    /// Adds the <see cref="FileScreenshotConsumer"/> instance for the screenshot saving to file.
    /// By default uses <see cref="AtataContext.Artifacts"/> directory as a directory path format and
    /// <c>"{screenshot-number:D2}{screenshot-pageobjectname: - *}{screenshot-pageobjecttypename: *}{screenshot-title: - *}"</c> as a file name format.
    /// Example of a screenshot file path using default settings: <c>"artifacts\20220303T143404\SampleTest\01 - Home page - Screenshot title.png"</c>.
    /// Available predefined path variables are:
    /// <c>{build-start}</c>, <c>{build-start-utc}</c>
    /// <c>{test-name}</c>, <c>{test-name-sanitized}</c>,
    /// <c>{test-suite-name}</c>, <c>{test-suite-name-sanitized}</c>,
    /// <c>{test-start}</c>, <c>{test-start-utc}</c>,
    /// <c>{driver-alias}</c>, <c>{screenshot-number}</c>,
    /// <c>{screenshot-title}</c>, <c>{screenshot-pageobjectname}</c>,
    /// <c>{screenshot-pageobjecttypename}</c>, <c>{screenshot-pageobjectfullname}</c>.
    /// Path variables support formatting.
    /// </summary>
    /// <returns>The <see cref="ScreenshotConsumerAtataContextBuilder{TScreenshotConsumer}"/> instance.</returns>
    public ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> AddFile() =>
        Add(new FileScreenshotConsumer());
}
