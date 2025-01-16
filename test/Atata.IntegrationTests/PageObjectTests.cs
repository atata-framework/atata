namespace Atata.IntegrationTests;

public class PageObjectTests : WebDriverSessionTestSuite
{
    protected override bool ReuseDriver => false;

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
                .Should.BePresent();

    [Test]
    public void GoBack_WithInappropriateTOther()
    {
        var page2 = Go.To<GoTo1Page>()
            .GoTo2();

        page2.GoBack<GoTo3Page>()
            .PageUri.Relative.Should.Be(GoTo1Page.DefaultUrl);
    }

    [Test]
    public void GoForward() =>
        Go.To<GoTo1Page>()
            .GoTo2()
                .GoBack<GoTo1Page>()
            .GoForward<GoTo2Page>()
                .Should.BePresent();

    [Test]
    public void GoForward_WithInappropriateTOther()
    {
        var page1 = Go.To<GoTo1Page>()
            .GoTo2()
                .GoBack<GoTo1Page>();

        page1.GoForward<GoTo3Page>()
            .PageUri.Relative.Should.Be(GoTo2Page.DefaultUrl);
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
            .ActiveControl.DomProperties.Value.Should.Equal("123")
            .IntTextInput.Focus()
            .ActiveControl.DomProperties.Id.Should.Equal("int-text-input");

    [Test]
    public void AggregateAssert()
    {
        var exception = Assert.Throws<AggregateAssertionException>(() =>
            Go.To<StubPage>()
                .AggregateAssert(x => x
                    .IsTrue.Should.AtOnce.BeFalse()
                    .IsTrue.Should.AtOnce.BeTrue()
                    .IsTrue.Should.AtOnce.BeFalse()));

        Assert.That(exception.Results, Has.Count.EqualTo(2));
    }

    [Test]
    public void AggregateAssert_WithComponentSelector()
    {
        var exception = Assert.Throws<AggregateAssertionException>(() =>
           Go.To<StubPage>()
               .AggregateAssert(_ => _.IsTrue, x =>
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
    [Platform(Exclude = Platforms.MacOS)]
    public void ScrollDown() =>
        Go.To<ScrollablePage>()
            .BottomText.WaitTo.Not.BeVisibleInViewport()
            .ScrollDown()
            .BottomText.Should.BeVisibleInViewport();

    [Test]
    [Platform(Exclude = Platforms.MacOS)]
    public void ScrollUp() =>
        Go.To<ScrollablePage>()
            .BottomText.WaitTo.Not.BeVisibleInViewport()
            .ScrollDown()
            .BottomText.WaitTo.BeVisibleInViewport()
            .ScrollUp()
            .TopText.Should.BeVisibleInViewport();

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

    [Test]
    public void SwitchToAlertBox()
    {
        var sut = Go.To<PopupBoxPage>()
            .AlertButton.Click();
        AssertThatPopupBoxIsOpen();

        sut.SwitchToAlertBox()
            .Text.Should.Be("Alert!!!")
            .Accept();

        AssertThatPopupBoxIsNotOpen();
    }

    [Test]
    public void SwitchToAlertBox_WithDelayedOpen()
    {
        var sut = Go.To<PopupBoxPage>()
            .AlertWithDelayButton.Click();

        sut.SwitchToAlertBox()
            .Accept();

        AssertThatPopupBoxIsNotOpen();
    }

    [Test]
    public void SwitchToConfirmBox_AndAccept()
    {
        var sut = Go.To<PopupBoxPage>()
            .ConfirmButton.Click();
        AssertThatPopupBoxIsOpen();

        sut.SwitchToConfirmBox()
            .Text.Should.Be("Are you sure?")
            .Accept();

        AssertThatPopupBoxIsNotOpen();
    }

    [Test]
    public void SwitchToConfirmBox_AndCancel()
    {
        var sut = Go.To<PopupBoxPage>()
            .ConfirmButton.Click();

        sut.SwitchToConfirmBox()
            .Cancel();

        AssertThatPopupBoxIsNotOpen();
    }

    [Test]
    public void SwitchToPromptBox_AndAccept()
    {
        var sut = Go.To<PopupBoxPage>()
            .PromptButton.Click();
        AssertThatPopupBoxIsOpen();

        sut.SwitchToPromptBox()
            .Text.Should.Be("What is your name?")
            .Type("John")
            .Accept();

        AssertThatPopupBoxIsNotOpen();
        sut.PromptEnteredValue.Should.Be("John");
    }

    [Test]
    public void SwitchToPromptBox_AndCancel()
    {
        var sut = Go.To<PopupBoxPage>()
            .PromptButton.Click();

        sut.SwitchToPromptBox()
            .Type("John")
            .Cancel();

        AssertThatPopupBoxIsNotOpen();
        sut.PromptEnteredValue.Should.BeEmpty();
    }
}
