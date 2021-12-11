namespace Atata
{
    public class TakeScreenshotOnNUnitErrorOnCleanUpEventHandler : IConditionalEventHandler<AtataContextCleanUpEvent>
    {
        private readonly string _screenshotTitle;

        public TakeScreenshotOnNUnitErrorOnCleanUpEventHandler(string screenshotTitle = "Failed")
        {
            _screenshotTitle = screenshotTitle;
        }

        public bool CanHandle(AtataContextCleanUpEvent eventData, AtataContext context) =>
            NUnitAdapter.IsCurrentTestFailed() && context.HasDriver;

        public void Handle(AtataContextCleanUpEvent eventData, AtataContext context) =>
            context.Log.Screenshot(_screenshotTitle);
    }
}
