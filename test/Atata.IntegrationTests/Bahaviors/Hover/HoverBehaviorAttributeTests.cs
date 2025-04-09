using System.Drawing;

namespace Atata.IntegrationTests.Bahaviors;

public class HoverBehaviorAttributeTests : WebDriverSessionTestSuite
{
    private static TestCaseData[] Source =>
    [
        new TestCaseData(new HoversUsingActionsAttribute())
    ];

    protected override bool ReuseDriver => false;

    [TestCaseSource(nameof(Source))]
    public void Execute(HoverBehaviorAttribute behavior)
    {
        static object GetPageYOffset() =>
            CurrentSession.Driver.AsScriptExecutor().ExecuteScript("return window.pageYOffset;")!;

        CurrentSession.Driver.Manage().Window.Size = new Size(400, 400);

        ScrollablePage page = Go.To<ScrollablePage>();
        Assume.That(GetPageYOffset(), Is.EqualTo(0));

        var sut = page.BottomText;
        sut.Metadata.Push(behavior);

        sut.Hover();

        Assert.That(GetPageYOffset(), Is.GreaterThan(200));
    }
}
