namespace Atata.ExtentReports;

public sealed class AddScreenshotToExtentLogEventHandler : IEventHandler<ArtifactAddedEvent>
{
    public void Handle(ArtifactAddedEvent eventData, AtataContext context)
    {
        if (eventData.ArtifactType == ArtifactTypes.Screenshot)
        {
            string relativeFilePath = eventData.AbsoluteFilePath.TrimStart(ExtentContext.WorkingDirectoryPath);

            ExtentContext.ResolveFor(context).Test.Log(
                Status.Info,
                CreateScreenCaptureMedia(relativeFilePath, eventData.ArtifactTitle));
        }
    }

    private static Media CreateScreenCaptureMedia(string relativeFilePath, string? title) =>
        MediaEntityBuilder.CreateScreenCaptureFromPath(relativeFilePath, title).Build();
}
