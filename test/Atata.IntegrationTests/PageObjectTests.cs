namespace Atata.IntegrationTests;

public class PageObjectTests : UITestFixture
{
    [Test]
    public void RefreshPage() =>
        Go.To<BasicControlsPage>()
            .ById.FirstName.Set("test")
            .ById.FirstName.Should.Equal("test")
            .RefreshPage()
            .ById.FirstName.Should.BeEmpty();

    [Test]
    public void RefreshPageUntil() =>
        Go.To<WaitingPage>()
            .CurrentTime.Get(out TimeSpan? time)
            .RefreshPageUntil(x => x.CurrentTime.Value > time.Value.Add(TimeSpan.FromSeconds(15)), timeout: 20, retryInterval: 2);

    [Test]
    public void RefreshPageUntil_WithTimeout()
    {
        var page = Go.To<WaitingPage>()
            .CurrentTime.Get(out TimeSpan? time);

        using (StopwatchAsserter.WithinSeconds(1))
            Assert.Throws<TimeoutException>(() =>
                page.RefreshPageUntil(x => x.CurrentTime.Value > time.Value.Add(TimeSpan.FromSeconds(15)), timeout: 1));
    }

    [Test]
    public void WhenIgnoreInitAttributeOnProperty() =>
        Assert.That(Go.To<BasicControlsPage>().IgnoredInput, Is.Null);

    [Test]
    public void GoBack() =>
        Go.To<GoTo1Page>()
            .GoTo2()
                .GoBack<GoTo1Page>()
                .Should.Exist();

    [Test]
    public void GoBack_WithInappropriateTOther()
    {
        var page2 = Go.To<GoTo1Page>()
            .GoTo2();

        Assert.Throws<AssertionException>(
            () => page2.GoBack<GoTo3Page>());
    }

    [Test]
    public void GoForward() =>
        Go.To<GoTo1Page>()
            .GoTo2()
                .GoBack<GoTo1Page>()
            .GoForward<GoTo2Page>()
                .Should.Exist();

    [Test]
    public void GoForward_WithInappropriateTOther()
    {
        var page1 = Go.To<GoTo1Page>()
            .GoTo2()
                .GoBack<GoTo1Page>();

        Assert.Throws<AssertionException>(
            () => page1.GoForward<GoTo3Page>());
    }

    [Test]
    public void Press() =>
        Go.To<InputPage>()
            .TextInput.Focus()
            .Press("abc")
            .Press("d")
            .Press(Keys.Tab)
            .Press("e")
            .TextInput.Should.Equal("abcd");

    [Test]
    public void ActiveControl() =>
        Go.To<InputPage>()
            .TelInput.Set("123")
            .ActiveControl.Attributes.Value.Should.Equal("123")
            .IntTextInput.Focus()
            .ActiveControl.Attributes.Id.Should.Equal("int-text-input");

    [Test]
    public void AggregateAssert()
    {
        var exception = Assert.Throws<AggregateAssertionException>(() =>
            Go.To<StubPage>().
                AggregateAssert(x => x
                    .IsTrue.Should.AtOnce.BeFalse()
                    .IsTrue.Should.AtOnce.BeTrue()
                    .IsTrue.Should.AtOnce.BeFalse()));

        Assert.That(exception.Results, Has.Count.EqualTo(2));
    }

    [Test]
    public void AggregateAssert_WithComponentSelector()
    {
        var exception = Assert.Throws<AggregateAssertionException>(() =>
           Go.To<StubPage>().
               AggregateAssert(_ => _.IsTrue, x =>
               {
                   x.Should.AtOnce.BeFalse();
                   x.Should.AtOnce.BeTrue();
                   x.Should.AtOnce.BeFalse();
               }));

        Assert.That(exception.Results, Has.Count.EqualTo(2));
    }

    [Test]
    public void PageSource() =>
        Go.To<BasicControlsPage>()
            .PageSource.Should.ContainAll("<head>", "</body>", "<input", "<button");

    [Test]
    public void ScrollDown() =>
        Go.To<ScrollablePage>()
            .BottomText.Should.Not.BeVisibleInViewPort()
            .ScrollDown()
            .BottomText.Should.BeVisibleInViewPort();

    [Test]
    public void ScrollUp() =>
        Go.To<ScrollablePage>()
            .Press(Keys.End)
            .TopText.Should.Not.BeVisibleInViewPort()
            .ScrollUp()
            .TopText.Should.BeVisibleInViewPort();

    [Test]
    public void SwitchToFrame() =>
        Go.To<FramePage>()
           .SwitchToFrame1()
               .TextInput.Set("abc")
               .SwitchToRoot<FramePage>()
           .Header.Should.Equal("Frame")
           .SwitchToFrame2()
               .Select.Set(4)
               .SwitchBack()

           .Header.Should.Equal("Frame")
           .SwitchToFrame1()
               .Header.Should.Equal("Frame Inner 1")
               .TextInput.Should.Equal("abc")
               .SwitchToRoot<FramePage>()
           .SwitchToFrame2()
               .Header.Should.Equal("Frame Inner 2")
               .Select.Should.Equal(4);

    [Test]
    public void FrameSwitchingViaGo()
    {
        Go.To<FramePage>()
            .Header.Should.Equal("Frame");

        Go.To<FrameInner1SelfSwitchingPage>()
            .Header.Should.Equal("Frame Inner 1")
            .TextInput.Set("abc");

        Go.To<FramePage>(navigate: false)
            .Header.Should.Equal("Frame");

        Go.To<FrameInner1SelfSwitchingPage>()
            .Header.Should.Equal("Frame Inner 1")
            .TextInput.Should.Equal("abc");
    }
}
