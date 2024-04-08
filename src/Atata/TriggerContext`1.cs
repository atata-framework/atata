namespace Atata;

public class TriggerContext<TOwner>
    where TOwner : PageObject<TOwner>, IPageObject<TOwner>
{
    public TriggerEvents Event { get; internal set; }

    // TODO: Review properties.
    public IWebDriver Driver { get; internal set; }

    public ILogManager Log { get; internal set; }

    public IUIComponent<TOwner> Component { get; internal set; }
}
