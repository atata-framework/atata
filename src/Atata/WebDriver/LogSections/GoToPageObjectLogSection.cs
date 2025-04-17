namespace Atata;

public class GoToPageObjectLogSection : UIComponentLogSection
{
    public GoToPageObjectLogSection(UIComponent pageObject, string? url = null, string? navigationTarget = null)
        : base(pageObject)
    {
        Url = url;

        var messageBuilder = new StringBuilder("Go to ")
            .Append(pageObject.ComponentFullName);

        if (navigationTarget?.Length > 0)
            messageBuilder.Append(' ').Append(navigationTarget);

        if (url?.Length > 0)
            messageBuilder.Append(" by URL ").Append(url);

        Message = messageBuilder.ToString();
    }

    public string? Url { get; }
}
