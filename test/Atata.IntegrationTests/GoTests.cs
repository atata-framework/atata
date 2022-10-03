namespace Atata.IntegrationTests;

public class GoTests : UITestFixture
{
    protected override bool ReuseDriver => false;

    [Test]
    public void To_UsingDirectNavigation()
    {
        var page1 = Go.To<GoTo1Page>();
        AssertCurrentPageObject(page1);

        var page2 = Go.To<GoTo2Page>();
        AssertCurrentPageObject(page2);
    }

    [Test]
    public void To_UsingLinkNavigation()
    {
        GoTo1Page page1 = Go.To<GoTo1Page>();

        AssertNoTemporarilyPreservedPageObjects();

        GoTo1Page page1Returned = page1
            .GoTo2.ClickAndGo()
                .GoTo1();

        AssertNoTemporarilyPreservedPageObjects();
        Assert.That(page1Returned, Is.Not.EqualTo(page1));
    }

    [Test]
    public void To_UsingAbsoluteUrlNavigation()
    {
        Go.To<GoTo1Page>();
        string url = BaseUrl + "/goto2?somearg=1";
        Go.To<GoTo2Page>(url: url);

        AssertNoTemporarilyPreservedPageObjects();
        Assert.That(AtataContext.Current.Driver.Url, Is.EqualTo(url));
    }

    [Test]
    public void To_UsingRelativeUrlNavigation()
    {
        Go.To<GoTo1Page>();
        string url = "goto2?somearg=1";
        Go.To<GoTo2Page>(url: url);

        AssertNoTemporarilyPreservedPageObjects();
        Assert.That(AtataContext.Current.Driver.Url, Does.EndWith(url));
    }

    [Test]
    public void To_UsingRelativeUrlWithLeadingSlashNavigation()
    {
        Go.To<GoTo1Page>();
        string url = "/goto2?somearg=1";
        Go.To<GoTo2Page>(url: url);

        AssertNoTemporarilyPreservedPageObjects();
        Assert.That(AtataContext.Current.Driver.Url, Does.EndWith(url));
    }

    [Test]
    public void To_WithTemporarilyTrue()
    {
        var page1 = Go.To<GoTo1Page>();
        Go.To<GoTo2Page>(temporarily: true);

        AssertTemporarilyPreservedPageObjects(page1);

        var page1Returned = Go.To<GoTo1Page>();

        AssertNoTemporarilyPreservedPageObjects();
        Assert.That(page1Returned, Is.EqualTo(page1));
    }

    [Test]
    public void To_TemporarilyByLink()
    {
        var page1 = Go.To<GoTo1Page>();
        var page2 = page1.GoTo2Temporarily();

        AssertTemporarilyPreservedPageObjects(page1);

        var page1Returned = page2.GoTo1();

        AssertNoTemporarilyPreservedPageObjects();
        Assert.That(page1Returned, Is.EqualTo(page1));

        page1Returned.GoTo2();
    }

    [Test]
    public void To_TemporarilyByLink_Twice()
    {
        var page1 = Go.To<GoTo1Page>();
        var page2 = page1.GoTo2Temporarily();

        AssertTemporarilyPreservedPageObjects(page1);

        var page1Returned = page2.GoTo1Temporarily();

        AssertNoTemporarilyPreservedPageObjects();
        Assert.That(page1Returned, Is.EqualTo(page1));
    }

    [Test]
    public void To_TemporarilyByLink_Complex()
    {
        var page1 = Go.To<GoTo1Page>();
        var page2 = page1.GoTo2Temporarily();

        AssertTemporarilyPreservedPageObjects(page1);

        var page3 = page2.GoTo3Temporarily();

        AssertTemporarilyPreservedPageObjects(page1, page2);

        var page1Returned = page3.GoTo1();

        AssertNoTemporarilyPreservedPageObjects();
        Assert.That(page1Returned, Is.EqualTo(page1));
        AssertCurrentPageObject(page1);
    }

    [Test]
    public void ToNextWindow()
    {
        var page1 = Go.To<GoTo1Page>();

        page1.GoTo2Blank();

        Go.ToNextWindow<GoTo2Page>()
            .CloseWindow();

        AssertNoTemporarilyPreservedPageObjects();

        Go.To<GoTo1Page>(navigate: false);
    }

    [Test]
    public void ToNextWindow_WithTemporarilyTrue()
    {
        var page1 = Go.To<GoTo1Page>()
            .GoTo2Blank();

        Go.ToNextWindow<GoTo2Page>(temporarily: true)
            .CloseWindow();

        AssertTemporarilyPreservedPageObjects(page1);

        Go.To<GoTo1Page>(navigate: false);

        AssertCurrentPageObject(page1);
    }

    [Test]
    public void ToNextWindow_WithTemporarilyTrue_Complex()
    {
        var page1 = Go.To<GoTo1Page>()
            .GoTo2Blank();

        var page2 = Go.ToNextWindow<GoTo2Page>(temporarily: true);

        page2
            .GoTo3Temporarily()
            .CloseWindow();

        AssertTemporarilyPreservedPageObjects(page1, page2);

        var page1Returned = Go.To<GoTo1Page>(navigate: false);

        Assert.That(page1Returned, Is.EqualTo(page1));
        AssertCurrentPageObject(page1);
    }

    [Test]
    public void ToNewWindowAsTab()
    {
        Go.To<GoTo1Page>();

        Go.ToNewWindowAsTab<GoTo2Page>()
            .PageUri.AbsolutePath.Should.EndWith("/goto2");

        AssertWindowHandlesCount(2);
        AssertNoTemporarilyPreservedPageObjects();

        Go.ToPreviousWindow<GoTo1Page>()
            .PageUri.AbsolutePath.Should.EndWith("/goto1");
    }

    [Test]
    public void ToNewWindow()
    {
        Go.To<GoTo1Page>();

        Go.ToNewWindow<GoTo2Page>()
            .PageUri.AbsolutePath.Should.EndWith("/goto2");

        AssertWindowHandlesCount(2);
        AssertNoTemporarilyPreservedPageObjects();

        Go.ToPreviousWindow<GoTo1Page>()
            .PageUri.AbsolutePath.Should.EndWith("/goto1");
    }

    [Test]
    public void ToNewWindow_WithUrl()
    {
        Go.To<GoTo1Page>();

        var page2 = Go.ToNewWindow<ScrollablePage>(url: "/goto2")
            .PageUri.AbsolutePath.Should.EndWith("/goto2");

        AssertWindowHandlesCount(2);
        AssertNoTemporarilyPreservedPageObjects();

        page2.CloseWindow();

        Go.To<GoTo1Page>(navigate: false)
            .PageUri.AbsolutePath.Should.EndWith("/goto1");
    }

    [Test]
    public void ToNewWindow_WithTemporarilyTrue()
    {
        var page1 = Go.To<GoTo1Page>();

        var page2 = Go.ToNewWindow<GoTo2Page>(temporarily: true)
            .PageUri.AbsolutePath.Should.EndWith("/goto2");

        AssertWindowHandlesCount(2);
        AssertTemporarilyPreservedPageObjects(page1);

        page2.CloseWindow();
        AssertCurrentPageObject(page2);

        Go.To<GoTo1Page>(navigate: false)
            .PageUri.AbsolutePath.Should.EndWith("/goto1");

        AssertCurrentPageObject(page1);
    }

    private static void AssertWindowHandlesCount(int expected) =>
        Assert.That(AtataContext.Current.Driver.WindowHandles.Count, Is.EqualTo(expected));

    private static void AssertCurrentPageObject(UIComponent pageObject) =>
        Assert.That(AtataContext.Current.PageObject, Is.EqualTo(pageObject));

    private static void AssertNoTemporarilyPreservedPageObjects() =>
        Assert.That(AtataContext.Current.TemporarilyPreservedPageObjects, Is.Empty);

    private static void AssertTemporarilyPreservedPageObjects(params UIComponent[] pageObjects)
    {
        Assert.That(AtataContext.Current.TemporarilyPreservedPageObjects.Count, Is.EqualTo(pageObjects.Length));
        Assert.That(AtataContext.Current.TemporarilyPreservedPageObjects, Is.EquivalentTo(pageObjects));
    }
}
