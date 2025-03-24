using ExtReports = AventStack.ExtentReports.ExtentReports;

namespace Atata.ExtentReports;

internal sealed class ExtentContext
{
    private static readonly Lazy<string> s_workingDirectoryPath =
        new(BuildWorkingDirectoryPath);

    private static readonly Lazy<ExtReports> s_lazyReports =
        new(CreateAndInitReportsInstance);

    private static readonly LockingConcurrentDictionary<string, ExtentContext> s_testSuiteExtentContextMap =
        new(StartExtentTestSuite);

    private static readonly LockingConcurrentDictionary<(string TestSuiteName, string TestName), ExtentContext> s_testExtentContextMap =
        new(StartExtentTest);

    public ExtentContext(ExtentTest test) =>
        Test = test;

    public static string WorkingDirectoryPath => s_workingDirectoryPath.Value;

    public static ExtentReportsBuilder Configuration { get; set; } = new();

    public static string ReportAdditionalCss { get; set; } =
@"
  tr.event-row > td { vertical-align: top; font-family: ""Cascadia Mono"", Consolas, ""Courier New""; color: #222; line-height: 1.5 !important; }
  tr.event-row > td:nth-child(2) { color: #666; }
  .table-sm > tbody > tr > td, .table-sm > thead > tr > th { padding: 0.2em; }
  .mb-3 { margin-bottom: 0 !important; }
  .mt-4 { margin-top: 0.5rem !important; }
  .detail-body img { padding: 0; border: 1px solid #ccc; }
  .artifacts { padding-left: 2em; }
";

    public static ExtReports Reports => s_lazyReports.Value;

    public ExtentTest Test { get; }

    public LogEventInfo? LastLogEvent { get; set; }

    public static ExtentContext ResolveFor(AtataContext context)
    {
        string testSuiteName = context.Test.SuiteName
            ?? throw new InvalidOperationException($"{nameof(AtataContext)}.{nameof(AtataContext.Test)}.{nameof(TestInfo.SuiteName)} is not set and cannot be used to create Extent test.");
        string testName = context.Test.Name;

        return testName is null
            ? ResolveForTestSuite(testSuiteName)
            : ResolveForTest(testSuiteName, testName);
    }

    private static ExtentContext ResolveForTestSuite(string testSuiteName) =>
        s_testSuiteExtentContextMap.GetOrAdd(testSuiteName);

    private static ExtentContext ResolveForTest(string testSuiteName, string testName) =>
        s_testExtentContextMap.GetOrAdd((testSuiteName, testName));

    private static ExtentContext StartExtentTestSuite(string testSuiteName)
    {
        ExtentTest extentTest = Reports.CreateTest(testSuiteName);

        return new(extentTest);
    }

    private static ExtentContext StartExtentTest((string TestSuiteName, string TestName) testInfo)
    {
        var testSuiteContext = ResolveForTestSuite(testInfo.TestSuiteName);

        ExtentTest extentTest = testSuiteContext.Test.CreateNode(testInfo.TestName);

        return new(extentTest);
    }

    private static ExtReports CreateAndInitReportsInstance()
    {
        ExtReports reports = new ExtReports();

        var reporters = CreateReporters(WorkingDirectoryPath);

        reports.AttachReporter([.. reporters]);

        return reports;
    }

    private static string BuildWorkingDirectoryPath() =>
        AtataContext.GlobalProperties.ArtifactsRootPath + Path.DirectorySeparatorChar;

    private static IEnumerable<IObserver<ReportEntity>> CreateReporters(string workingDirectoryPath)
    {
        ExtentSparkReporter sparkReporter = new(
            Path.Combine(workingDirectoryPath, Configuration.ReportFileName));

        sparkReporter.Config.DocumentTitle = $"{Configuration.ReportTitle} / {AtataContext.GlobalProperties.RunStart:yyyy-MM-dd HH:mm:ss}";
        sparkReporter.Config.TimeStampFormat = "HH:mm:ss.fff";
        sparkReporter.Config.CSS = ReportAdditionalCss;

        yield return sparkReporter;
    }

    private sealed class LockingConcurrentDictionary<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, Lazy<TValue>> _dictionary;

        private readonly Func<TKey, Lazy<TValue>> _valueFactory;

        public LockingConcurrentDictionary(Func<TKey, TValue> valueFactory)
        {
            _dictionary = new ConcurrentDictionary<TKey, Lazy<TValue>>();
            _valueFactory = key => new Lazy<TValue>(() => valueFactory(key));
        }

        public TValue GetOrAdd(TKey key) =>
            _dictionary.GetOrAdd(key, _valueFactory).Value;
    }
}
