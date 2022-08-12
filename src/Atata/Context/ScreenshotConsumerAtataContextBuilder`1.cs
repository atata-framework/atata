namespace Atata
{
    public class ScreenshotConsumerAtataContextBuilder<TScreenshotConsumer> : ScreenshotConsumersAtataContextBuilder, IHasContext<TScreenshotConsumer>
        where TScreenshotConsumer : IScreenshotConsumer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenshotConsumerAtataContextBuilder{TScreenshotConsumer}"/> class.
        /// </summary>
        /// <param name="screenshotConsumer">The screenshot consumer.</param>
        /// <param name="buildingContext">The building context.</param>
        public ScreenshotConsumerAtataContextBuilder(TScreenshotConsumer screenshotConsumer, AtataBuildingContext buildingContext)
            : base(buildingContext) =>
            Context = screenshotConsumer;

        /// <inheritdoc/>
        public TScreenshotConsumer Context { get; }
    }
}
