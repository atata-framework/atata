namespace Atata;

public class GoToUrlLogSection : LogSection
{
    public GoToUrlLogSection(Uri url, bool useInfoLevel = true)
    {
        Url = url;

        Message = $"Navigate to URL {url.AbsoluteUri}";
        Level = useInfoLevel ? LogLevel.Info : LogLevel.Trace;
    }

    public Uri Url { get; }
}
