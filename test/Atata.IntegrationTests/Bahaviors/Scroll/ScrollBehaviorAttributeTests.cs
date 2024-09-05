using System.Drawing;

namespace Atata.IntegrationTests.Bahaviors;

public class ScrollBehaviorAttributeTests : WebDriverSessionTestSuite
{
    private static TestCaseData[] Source =>
    [
        new TestCaseData(new ScrollsUsingScrollToElementActionAttribute()),
        new TestCaseData(new ScrollsUsingMoveToElementActionAttribute()),
        new TestCaseData(new ScrollsUsingScriptAttribute())
    ];

    protected override bool ReuseDriver => false;

    [TestCaseSource(nameof(Source))]
    public void Execute(ScrollBehaviorAttribute behavior)
    {
        static object GetPageYOffset() =>
            WebDriverSession.Current.Driver.AsScriptExecutor().ExecuteScript("return window.pageYOffset;");

        WebDriverSession.Current.Driver.Manage().Window.Size = new Size(400, 400);

        ScrollablePage page = Go.To<ScrollablePage>();
        Assume.That(GetPageYOffset(), Is.EqualTo(0));

        var sut = page.BottomText;
        sut.Metadata.Push(behavior);

        sut.ScrollTo();

        Assert.That(GetPageYOffset(), Is.GreaterThan(200));
    }
}
