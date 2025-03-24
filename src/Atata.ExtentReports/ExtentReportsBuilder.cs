namespace Atata.ExtentReports;

public sealed class ExtentReportsBuilder
{
    internal string ReportTitle { get; set; } = "Atata Tests";

    internal string ReportFileName { get; set; } = "Report.html";

    /// <summary>
    /// Sets the report title.
    /// The default value is <c>"Atata Tests"</c>.
    /// </summary>
    /// <param name="reportTitle">The report title.</param>
    /// <returns>The same <see cref="ExtentReportsBuilder"/> builder.</returns>
    public ExtentReportsBuilder WithReportTitle(string reportTitle)
    {
        ReportTitle = reportTitle;
        return this;
    }

    /// <summary>
    /// Sets the name of a report file.
    /// The default value is <c>"Report.html"</c>.
    /// </summary>
    /// <param name="reportFileName">The name of a report file.</param>
    /// <returns>The same <see cref="ExtentReportsBuilder"/> builder.</returns>
    public ExtentReportsBuilder WithReportFileName(string reportFileName)
    {
        ReportFileName = reportFileName;
        return this;
    }
}
