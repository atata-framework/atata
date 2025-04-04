namespace Atata;

public class GoToUrlLogSection : LogSection
{
    public GoToUrlLogSection(Uri url)
    {
        Url = url;
        Message = $"Navigate to URL {url.AbsoluteUri}";
    }

    public Uri Url { get; }
}
