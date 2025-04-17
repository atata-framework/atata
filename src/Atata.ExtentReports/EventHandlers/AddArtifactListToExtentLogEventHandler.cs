namespace Atata.ExtentReports;

public sealed class AddArtifactListToExtentLogEventHandler : IEventHandler<AtataContextDeInitCompletedEvent>
{
    public void Handle(AtataContextDeInitCompletedEvent eventData, AtataContext context)
    {
        DirectoryInfo directory = context.Artifacts.Object;

        if (directory.Exists)
        {
            bool isTestContext = context.Test.Name?.Length > 0;

            SearchOption directorySearchOption = isTestContext
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;
            string label = isTestContext ? "Artifacts" : "Test suite artifacts";

            var relativeFilePaths = directory.EnumerateFiles("*", directorySearchOption)
                .OrderBy(x => x.CreationTimeUtc)
                .Select(x => x.FullName.TrimStart(ExtentContext.WorkingDirectoryPath));

            IMarkup markup = new ArtifactsListMarkup(label, relativeFilePaths);

            ExtentContext.ResolveFor(context).Test.Log(Status.Info, markup);
        }
    }

    private sealed class ArtifactsListMarkup : IMarkup
    {
        private readonly string _label;

        private readonly IEnumerable<string> _relativeFilePaths;

        public ArtifactsListMarkup(string label, IEnumerable<string> relativeFilePaths)
        {
            _label = label;
            _relativeFilePaths = relativeFilePaths;
        }

        public string GetMarkup()
        {
            StringBuilder builder = new StringBuilder($"{_label}:<ul class=\"artifacts\">");

            foreach (string relativeFilePath in _relativeFilePaths)
            {
                string extraAnchorAttribute = relativeFilePath.EndsWith(".png", StringComparison.Ordinal)
                    ? "data-featherlight=\"image\""
                    : "target=\"_blank\"";

                builder.Append($"<li><a href=\"{relativeFilePath}\" {extraAnchorAttribute}>{Path.GetFileName(relativeFilePath)}</a></li>");
            }

            builder.Append("</ul>");
            return builder.ToString();
        }
    }
}
