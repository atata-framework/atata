namespace Atata.IntegrationTests;

using _ = WaitingOnInitPage;

[Url(Url)]
[WaitForDocumentReadyState]
public class WaitingOnInitPage : Page<_>
{
    public const string Url = "waitingoninit";

    public enum WaitKind
    {
        None,
        WaitForElementVisible,
        WaitForVisible,
        VerifyExists,
        VerifyMissing
    }

    [FindByClass(Visibility = Visibility.Visible)]
    public Control<_> LoadingBlock { get; private set; }

    [FindByClass(Visibility = Visibility.Visible)]
    public Text<_> ContentBlock { get; private set; }

    public WaitKind OnInitWaitKind { get; set; }

    protected override void OnInit()
    {
        if (OnInitWaitKind == WaitKind.WaitForElementVisible)
            Metadata.Add(new WaitForElementAttribute(WaitBy.Class, "content-block", Until.Visible, TriggerEvents.Init));
        else if (OnInitWaitKind == WaitKind.WaitForVisible)
            ContentBlock.Metadata.Add(new WaitForAttribute(Until.Visible));
        else if (OnInitWaitKind == WaitKind.VerifyExists)
            ContentBlock.Metadata.Add(new VerifyExistsAttribute());
        else if (OnInitWaitKind == WaitKind.VerifyMissing)
            LoadingBlock.Metadata.Add(new VerifyMissingAttribute());
    }

    public _ VerifyContentBlockIsLoaded()
    {
        Assert.That(ContentBlock.GetScope(SearchOptions.UnsafelyAtOnce()).Text, Is.EqualTo("Loaded"));
        return this;
    }
}
