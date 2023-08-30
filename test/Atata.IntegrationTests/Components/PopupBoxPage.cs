namespace Atata.IntegrationTests;

using _ = PopupBoxPage;

[Url("popupbox")]
[VerifyTitle]
public class PopupBoxPage : Page<_>
{
    public Button<_> NoneButton { get; private set; }

    public Button<_> AlertButton { get; private set; }

    public Button<_> AlertWithDelayButton { get; private set; }

    public Link<GoTo1Page, _> ConfirmButton { get; private set; }

    public Button<_> PromptButton { get; private set; }

    [FindById]
    public Text<_> PromptEnteredValue { get; private set; }
}
