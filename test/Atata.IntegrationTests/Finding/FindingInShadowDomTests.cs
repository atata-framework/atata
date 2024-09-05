namespace Atata.IntegrationTests.Finding;

public class FindingInShadowDomTests : WebDriverSessionTestSuite
{
    private ShadowDomPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<ShadowDomPage>();

    [Test]
    public void First_AnyVisibility()
    {
        var sut = _page.Shadow1_0;

        sut.Should.BePresent();
        sut.Content.Should.BeEmpty();
        sut.Scope.TagName.Should().Be("style");
    }

    [Test]
    public void First_Visible()
    {
        var sut = _page.Shadow1_1;

        sut.Should.BePresent();
        sut.Content.Should.Equal("Shadow 1.1");
    }

    [Test]
    public void ByIndex()
    {
        var sut = _page.Shadow1_2;

        sut.Should.BePresent();
        sut.Content.Should.Equal("Shadow 1.2");
    }

    [Test]
    public void ControlList() =>
        _page.AggregateAssert(x => x.Shadow1Paragraphs, controls =>
        {
            controls[0].Should.Equal("Shadow 1.1");
            controls[1].Should.Equal("Shadow 1.2");
            controls.Count.Should.Equal(2);
            controls.Should.EqualSequence("Shadow 1.1", "Shadow 1.2");
            controls.Contents.Should.EqualSequence("Shadow 1.1", "Shadow 1.2");
        });

    [Test]
    public void UsingPropertyNameAsTerm() =>
        _page.ShadowContainer1.Should.EqualSequence("Shadow 1.1", "Shadow 1.2");

    [Test]
    public void UsingTerm() =>
        _page.ShadowContainer1UsingTerm.Should.EqualSequence("Shadow 1.1", "Shadow 1.2");

    [Test]
    public void RadioButtonList()
    {
        var sut = _page.YesNoRadios;

        sut.Should.BePresent();
        sut.Should.BeNull();
        sut.Set("No");
        sut.Should.Equal("No");
        sut.Set("Yes");
        sut.Should.Equal("Yes");
    }

    [Test]
    public void TwoLayers()
    {
        var sut = _page.Shadow2_1_1;

        sut.Should.BePresent();
        sut.Content.Should.Equal("2.1.1");
    }

    [Test]
    public void ThreeLayers()
    {
        var sut = _page.Shadow2_1_1_1;

        sut.Should.BePresent();
        sut.Content.Should.Equal("Shadow 2.1.1.1");
    }

    [Test]
    public void ThreeLayers_AtDifferentLevels()
    {
        var sut = _page.Shadow2_1_1_1_AtDifferentLevels;

        sut.Should.BePresent();
        sut.Content.Should.Equal("Shadow 2.1.1.1");
    }

    [Test]
    public void ThreeLayers_AtDifferentLevels_WithSetLayers()
    {
        var sut = _page.Shadow2_1_1_1_AtDifferentLevelsWithSetLayers;

        sut.Should.BePresent();
        sut.Content.Should.Equal("Shadow 2.1.1.1");
    }

    [Test]
    public void MixedLayers_AtDifferentLevels_WithSetLayers()
    {
        var sut = _page.Shadow2_1_1_1_MixedAtDifferentLevelsWithSetLayers;

        sut.Should.BePresent();
        sut.Content.Should.Equal("2.1.1.1");
    }

    [Test]
    public void AsShadowHostPage() =>
        Go.To<ShadowHostPage>().AggregateAssert(x => x
            .Paragraphs.Should.EqualSequence("Shadow 1.1", "Shadow 1.2")
            .Paragraph2.Should.Equal("Shadow 1.2"));

    [Test]
    public void AsShadowHostPopupWindow() =>
        Go.To<ShadowHostPopupWindow>().AggregateAssert(x => x
            .Paragraphs.Should.EqualSequence("Shadow 1.1", "Shadow 1.2")
            .Paragraph2.Should.Equal("Shadow 1.2"));

    [Test]
    public void InvalidShadowHostLocator()
    {
        var sut = _page.InvalidShadowRoot;

        AssertThrowsWithInnerException<AssertionException, WebDriverException>(() =>
            sut.Should.BePresent())
            .InnerException.Message.Should().Contain("Element doesn't have shadowRoot value");
    }
}
