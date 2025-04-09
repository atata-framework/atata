﻿namespace Atata.IntegrationTests;

public class GoTests : WebDriverSessionTestSuite
{
    protected override bool ReuseDriver => false;

    [Test]
    public void ToUrl_Relative_WithoutPriorNavigation()
    {
        Go.ToUrl(GoTo1Page.DefaultUrl);

        CurrentSession.Driver.Url.Should().Be(BaseUrl + GoTo1Page.DefaultUrl);
    }

    [Test]
    public void On_WhenNoCurrent()
    {
        var result = Go.On<GoTo1Page>();

        result.PageUri.Relative.Should.Not.Be(GoTo1Page.DefaultUrl);
    }

    [Test]
    public void On_WhenItIsCurrent()
    {
        var initialPage = Go.To<GoTo1Page>();

        var result = Go.On<GoTo1Page>();

        result.Should().Be(initialPage);
    }

    [Test]
    public void On_WhenItIsNotCurrent()
    {
        Go.To<GoTo1Page>();

        var result = Go.On<GoTo2Page>();

        result.PageUri.Relative.Should.Be(GoTo1Page.DefaultUrl);
        AssertNoTemporarilyPreservedPageObjects();
    }

    [Test]
    public void OnRefreshed_WhenNoCurrent()
    {
        var result = Go.OnRefreshed<GoTo1Page>();

        result.PageUri.Relative.Should.Not.Be(GoTo1Page.DefaultUrl);
    }

    [Test]
    public void OnRefreshed_WhenItIsCurrent()
    {
        var initialPage = Go.To<InputPage>()
            .TextInput.Type("init");

        var result = Go.OnRefreshed<InputPage>();

        result.Should().NotBe(initialPage);
        result.TextInput.Should.BeEmpty();
    }

    [Test]
    public void OnRefreshed_WhenItIsNotCurrent()
    {
        Go.To<GoTo1Page>();

        var result = Go.OnRefreshed<GoTo2Page>();

        result.PageUri.Relative.Should.Be(GoTo1Page.DefaultUrl);
        AssertNoTemporarilyPreservedPageObjects();
    }

    [Test]
    public void OnOrTo_WhenNoCurrent()
    {
        var result = Go.OnOrTo<GoTo1Page>();

        result.PageUri.Relative.Should.Be(GoTo1Page.DefaultUrl);
    }

    [Test]
    public void OnOrTo_WhenItIsCurrent()
    {
        var initialPage = Go.To<GoTo1Page>();

        var result = Go.OnOrTo<GoTo1Page>();

        result.Should().Be(initialPage);
    }

    [Test]
    public void OnOrTo_WhenItIsNotCurrent()
    {
        Go.To<GoTo1Page>();

        var result = Go.OnOrTo<GoTo2Page>();

        result.PageUri.Relative.Should.Be(GoTo2Page.DefaultUrl);
        AssertNoTemporarilyPreservedPageObjects();
    }

    [Test]
    public void To_WithoutUrl()
    {
        var page1 = Go.To<OrdinaryPage>();
        AssertCurrentPageObject(page1);
        page1.PageUri.Should.Be(new Uri(CurrentSession.BaseUrl!));
    }

    [Test]
    public void To_UsingDirectNavigation()
    {
        var page1 = Go.To<GoTo1Page>();
        AssertCurrentPageObject(page1);

        var page2 = Go.To<GoTo2Page>();
        AssertCurrentPageObject(page2);
    }

    [Test]
    public void To_UsingDirectNavigation_WhenUrlIsTemplated()
    {
        CurrentContext.Variables["GoToNumber"] = 2;
        CurrentSession.Variables["GoToArg"] = 42;
        CurrentSession.Variables["GoToFragment"] = "fragment";

        Go.To<PageWithTemplatedUrl>()
            .PageUri.Relative.Should.Be("/goto2?arg=42#fragment");
    }

    [Test]
    public void To_UsingDirectNavigation_WhenUrlIsTemplated_AndNoVariable()
    {
        CurrentContext.Variables["GoToNumber"] = 2;

        var exception = Assert.Throws<FormatException>(
            () => Go.To<PageWithTemplatedUrl>())!;

        exception.Message.Should().Contain(@"{GoToArg}");
    }

    [Test]
    public void To_UsingRelativeUrlNavigation_WhenUrlIsTemplated()
    {
        CurrentContext.Variables["GoToArg"] = "?";

        Go.To<GoTo1Page>(url: "/goto1{GoToArg:dataescape:?q=*}")
            .PageUrl.Should.EndWith("/goto1?q=%3F");
    }

    [Test]
    public void To_UsingRelativeUrlNavigation_WhenUrlIsTemplated_AndVariableNull()
    {
        CurrentContext.Variables["GoToArg"] = null;

        Go.To<GoTo1Page>(url: "/goto1{GoToArg:dataescape:?q=*}")
            .PageUrl.Should.EndWith("/goto1");
    }

    [Test]
    public void To_UsingQueryUrlNavigation_WhenUrlIsTemplated()
    {
        CurrentContext.Variables["GoToArg"] = "/?";

        Go.To<GoTo1Page>(url: "?q={GoToArg:dataescape}")
            .PageUrl.Should.EndWith("/goto1?q=%2F%3F");
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
        Assert.That(CurrentSession.Driver.Url, Is.EqualTo(url));
    }

    [Test]
    public void To_UsingRelativeUrlNavigation()
    {
        Go.To<GoTo1Page>();
        string url = "goto2?somearg=1";
        Go.To<GoTo2Page>(url: url);

        AssertNoTemporarilyPreservedPageObjects();
        Assert.That(CurrentSession.Driver.Url, Does.EndWith(url));
    }

    [Test]
    public void To_UsingRelativeUrlWithLeadingSlashNavigation()
    {
        Go.To<GoTo1Page>();
        string url = "/goto2?somearg=1";
        Go.To<GoTo2Page>(url: url);

        AssertNoTemporarilyPreservedPageObjects();
        Assert.That(CurrentSession.Driver.Url, Does.EndWith(url));
    }

    [TestCase("?arg=1", ExpectedResult = "/?arg=1")]
    [TestCase("&arg=1", ExpectedResult = "/?arg=1")]
    [TestCase(";arg=1", ExpectedResult = "/?arg=1")]
    [TestCase("#frag1", ExpectedResult = "/#frag1")]
    public string To_UsingCombinedUrl_WhenNavigationUrlIsNull(string url) =>
        Go.To<OrdinaryPage>(url: url)
            .PageUri.Relative;

    [TestCase("?arg=1", ExpectedResult = "/goto1?arg=1")]
    [TestCase("&arg=1", ExpectedResult = "/goto1?arg=1")]
    [TestCase(";arg=1", ExpectedResult = "/goto1?arg=1")]
    [TestCase("#frag1", ExpectedResult = "/goto1#frag1")]
    public string To_UsingCombinedUrl_WhenNavigationUrlHasOnlyPath(string url) =>
        Go.To<GoTo1Page>(url: url)
            .PageUri.Relative;

    [TestCase("?argB=2&argC=3", ExpectedResult = "/goto1?argB=2&argC=3")]
    [TestCase("&argB=2", ExpectedResult = "/goto1?argA=1&argB=2")]
    [TestCase(";argB", ExpectedResult = "/goto1?argA=1;argB")]
    [TestCase("#frag2", ExpectedResult = "/goto1?argA=1#frag2")]
    [TestCase("?argB=2#frag2", ExpectedResult = "/goto1?argB=2#frag2")]
    [TestCase("&argB=2#frag2", ExpectedResult = "/goto1?argA=1&argB=2#frag2")]
    public string To_UsingCombinedUrl_WhenNavigationUrlIsComplex(string url) =>
        Go.To<PageWithComplexUrl>(url: url)
            .PageUri.Relative;

    [TestCase("?arg=1", ExpectedResult = "/?arg=1")]
    [TestCase("/goto2", ExpectedResult = "/goto2")]
    [TestCase("goto2?arg=1", ExpectedResult = "/goto2?arg=1")]
    public string To_AfterSetNavigationUrl(string url) =>
        Go.To(new GoTo1Page().SetNavigationUrl(url))
            .PageUri.Relative;

    [TestCase("?arg=1", ExpectedResult = "/goto1?arg=1")]
    [TestCase("/goto2", ExpectedResult = "/goto1/goto2")]
    [TestCase("goto2?arg=1", ExpectedResult = "/goto1goto2?arg=1")]
    public string To_AfterAppendNavigationUrl_WhenStaticUrlHasOnlyPath(string url) =>
        Go.To(new GoTo1Page().AppendNavigationUrl(url))
            .PageUri.Relative;

    [Test]
    public void To_AfterAppendNavigationUrl_AndSetNavigationUrlVariable()
    {
        int arg1 = 1;

        var page = new GoTo1Page()
            .AppendNavigationUrl("?arg={arg1}")
            .SetNavigationUrlVariable(arg1);

        Go.To(page).PageUri.Relative.Should.Be("/goto1?arg=1");
    }

    [Test]
    public void To_WithNavigateTrue()
    {
        Go.To<GoTo1Page>();

        Go.To(new OrdinaryPage().SetNavigationUrl("/goto2"))
            .PageUri.Relative.Should.Be("/goto2");
    }

    [Test]
    public void To_WithNavigateFalse()
    {
        Go.To<GoTo1Page>();

        Go.To(new OrdinaryPage().SetNavigationUrl("/goto2"), navigate: false)
            .PageUri.Relative.Should.Be("/goto1");
    }

    [Test]
    public void To_WithNavigateFalse_InReusedDriver()
    {
        Go.To<GoTo1Page>();

        var driver = CurrentSession.Driver;

        CurrentSession.DisposeDriver = false;
        CurrentContext.Dispose();

        BuildAtataContextWithWebDriverSession(
            x => x.UseDriver(driver));

        Go.To(new OrdinaryPage().SetNavigationUrl("/goto2"), navigate: false)
            .PageUri.Relative.Should.Be("/goto1");
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
            .PageUri.Relative.Should.Be("/goto2");

        AssertWindowHandlesCount(2);
        AssertNoTemporarilyPreservedPageObjects();

        Go.ToPreviousWindow<GoTo1Page>()
            .PageUri.Relative.Should.Be("/goto1");
    }

    [Test]
    public void ToNewWindow()
    {
        Go.To<GoTo1Page>();

        Go.ToNewWindow<GoTo2Page>()
            .PageUri.Relative.Should.Be("/goto2");

        AssertWindowHandlesCount(2);
        AssertNoTemporarilyPreservedPageObjects();

        Go.ToPreviousWindow<GoTo1Page>()
            .PageUri.Relative.Should.Be("/goto1");
    }

    [Test]
    public void ToNewWindow_WithUrl()
    {
        Go.To<GoTo1Page>();

        var page2 = Go.ToNewWindow<ScrollablePage>(url: "/goto2")
            .PageUri.Relative.Should.Be("/goto2");

        AssertWindowHandlesCount(2);
        AssertNoTemporarilyPreservedPageObjects();

        page2.CloseWindow();

        Go.To<GoTo1Page>(navigate: false)
            .PageUri.Relative.Should.Be("/goto1");
    }

    [Test]
    public void ToNewWindow_WithTemporarilyTrue()
    {
        var page1 = Go.To<GoTo1Page>();

        var page2 = Go.ToNewWindow<GoTo2Page>(temporarily: true)
            .PageUri.Relative.Should.Be("/goto2");

        AssertWindowHandlesCount(2);
        AssertTemporarilyPreservedPageObjects(page1);

        page2.CloseWindow();
        AssertCurrentPageObject(page2);

        Go.To<GoTo1Page>(navigate: false)
            .PageUri.Relative.Should.Be("/goto1");

        AssertCurrentPageObject(page1);
    }

    private static void AssertWindowHandlesCount(int expected) =>
        Assert.That(CurrentSession.Driver.WindowHandles.Count, Is.EqualTo(expected));

    private static void AssertCurrentPageObject(UIComponent pageObject) =>
        Assert.That(CurrentSession.PageObject, Is.EqualTo(pageObject));

    private static void AssertNoTemporarilyPreservedPageObjects() =>
        Assert.That(CurrentSession.TemporarilyPreservedPageObjects, Is.Empty);

    private static void AssertTemporarilyPreservedPageObjects(params UIComponent[] pageObjects)
    {
        Assert.That(CurrentSession.TemporarilyPreservedPageObjects.Count, Is.EqualTo(pageObjects.Length));
        Assert.That(CurrentSession.TemporarilyPreservedPageObjects, Is.EquivalentTo(pageObjects));
    }

    public class WithoutBaseUrl : WebDriverSessionTestSuiteBase
    {
        [SetUp]
        public void SetUp() =>
            BuildAtataContextWithWebDriverSession(
                x => x.UseBaseUrl(null));

        [Test]
        public void ToUrl_Relative_WithoutPriorNavigation() =>
            Assert.Throws<InvalidOperationException>(() =>
                Go.ToUrl(GoTo1Page.DefaultUrl));

        [Test]
        public void To_WithoutUrl_WhenNotNavigated()
        {
            var exception = Assert.Throws<InvalidOperationException>(() =>
                Go.To<OrdinaryPage>())!;

            exception.Message.Should().StartWith("Cannot navigate to empty or null URL.");
        }

        [Test]
        public void To_WithoutUrl_WhenNavigated()
        {
            Go.To<OrdinaryPage>(url: BaseUrl + "/goto1");

            Go.To<OrdinaryPage>()
                .PageUri.Should.Be(new Uri(BaseUrl + "/goto1"));
        }

        [Test]
        public void To_WithRelativeUrl_WhenNotNavigated()
        {
            var exception = Assert.Throws<InvalidOperationException>(() =>
                Go.To<OrdinaryPage>(url: "/goto1"))!;

            exception.Message.Should().StartWith("Cannot navigate to relative URL \"/goto1\".");
        }

        [Test]
        public void To_WithRelativeUrl_WhenNavigated()
        {
            Go.To<OrdinaryPage>(url: BaseUrl + "/goto1");

            Go.To<OrdinaryPage>(url: "/goto2")
                .PageUri.Should.Be(new Uri(BaseUrl + "/goto2"));
        }

        [Test]
        public void To_WithAbsoluteUrl_WhenNotNavigated() =>
            Go.To<OrdinaryPage>(url: BaseUrl + "/goto1")
                .PageUri.Should.Be(new Uri(BaseUrl + "/goto1"));

        [Test]
        public void To_WithAbsoluteUrl_WhenNavigated()
        {
            Go.To<OrdinaryPage>(url: BaseUrl + "/goto1");

            Go.To<OrdinaryPage>(url: BaseUrl + "/goto2")
                .PageUri.Should.Be(new Uri(BaseUrl + "/goto2"));
        }
    }

    [Url("/goto{GoToNumber}?arg={GoToArg}#{GoToFragment}")]
    public class PageWithTemplatedUrl : Page<PageWithTemplatedUrl>
    {
    }

    [Url("/goto1?argA=1#frag1")]
    public class PageWithComplexUrl : Page<PageWithComplexUrl>
    {
    }
}
