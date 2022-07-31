namespace Atata.IntegrationTests;

using _ = ScrollablePage;

[Url("scrollable")]
public class ScrollablePage : Page<_>
{
    [FindById]
    public Text<_> BottomText { get; private set; }

    [FindById]
    public Text<_> TopText { get; private set; }
}
