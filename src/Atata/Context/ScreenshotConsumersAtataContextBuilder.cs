namespace Atata
{
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
        /// By default uses <c>"Logs\{build-start}\{test-name-sanitized}"</c> as folder path format,
        /// <c>"{screenshot-number:D2} - {screenshot-pageobjectname} {screenshot-pageobjecttypename}{screenshot-title: - *}"</c> as file name format
        /// and <see cref="OpenQA.Selenium.ScreenshotImageFormat.Png"/> as image format.
        /// Example of screenshot file path using default settings: <c>"Logs\2018-03-03 14_34_04\SampleTest\01 - Home page - Screenshot title.png"</c>.
        /// Available path variables are:
        /// <c>{build-start}</c>,
        /// <c>{test-name}</c>, <c>{test-name-sanitized}</c>,
        /// <c>{test-suite-name}</c>, <c>{test-suite-name-sanitized}</c>,
        /// <c>{test-start}</c>, <c>{driver-alias}</c>, <c>{screenshot-number}</c>,
        /// <c>{screenshot-title}</c>, <c>{screenshot-pageobjectname}</c>,
        /// <c>{screenshot-pageobjecttypename}</c>, <c>{screenshot-pageobjectfullname}</c>.
        /// Path variables support the formatting.
        /// </summary>
        /// <returns>The <see cref="ScreenshotConsumerAtataContextBuilder{TScreenshotConsumer}"/> instance.</returns>
        public ScreenshotConsumerAtataContextBuilder<FileScreenshotConsumer> AddFile() =>
            Add(new FileScreenshotConsumer());
    }
}
