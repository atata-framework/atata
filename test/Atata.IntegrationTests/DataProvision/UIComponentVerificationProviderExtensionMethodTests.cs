namespace Atata.IntegrationTests.DataProvision;

public class UIComponentVerificationProviderExtensionMethodTests : WebDriverSessionTestSuite
{
    private FieldVerificationProvider<string, EditableField<string, FindingPage>, FindingPage> _visibleSutShould;

    private FieldVerificationProvider<string, EditableField<string, FindingPage>, FindingPage> _hiddenSutShould;

    private FieldVerificationProvider<string, EditableField<string, FindingPage>, FindingPage> _missingSutShould;

    protected override void OnSetUp()
    {
        var page = Go.To<FindingPage>();

        _visibleSutShould = page.VisibleInput.Should.AtOnce;
        _hiddenSutShould = page.HiddenInput.Should.AtOnce;
        _missingSutShould = page.MissingInput.Should.AtOnce;
    }

    [Test]
    public void BePresent_VisibleComponent() =>
        Assert.DoesNotThrow(() =>
            _visibleSutShould.BePresent());

    [Test]
    public void BePresent_HiddenComponent() =>
        Assert.DoesNotThrow(() =>
            _hiddenSutShould.BePresent());

    [Test]
    public void BePresent_MissingComponent() =>
        AssertThrowsWithoutInnerException<AssertionException>(() =>
            _missingSutShould.BePresent());

    [Test]
    public void Not_BePresent_VisibleComponent() =>
        AssertThrowsWithoutInnerException<AssertionException>(() =>
            _visibleSutShould.Not.BePresent());

    [Test]
    public void Not_BePresent_HiddenComponent() =>
        AssertThrowsWithoutInnerException<AssertionException>(() =>
            _hiddenSutShould.Not.BePresent());

    [Test]
    public void Not_BePresent_MissingComponent() =>
        Assert.DoesNotThrow(() =>
            _missingSutShould.Not.BePresent());

    [Test]
    public void BeVisible_VisibleComponent() =>
        Assert.DoesNotThrow(() =>
            _visibleSutShould.BeVisible());

    [Test]
    public void BeVisible_HiddenComponent() =>
        AssertThrowsWithoutInnerException<AssertionException>(() =>
            _hiddenSutShould.BeVisible());

    [Test]
    public void BeVisible_MissingComponent() =>
        AssertThrowsWithoutInnerException<AssertionException>(() =>
            _missingSutShould.BeVisible());

    [Test]
    public void Not_BeVisible_VisibleComponent() =>
        AssertThrowsWithoutInnerException<AssertionException>(() =>
            _visibleSutShould.Not.BeVisible());

    [Test]
    public void Not_BeVisible_HiddenComponent() =>
        Assert.DoesNotThrow(() =>
            _hiddenSutShould.Not.BeVisible());

    [Test]
    public void Not_BeVisible_MissingComponent() =>
        Assert.DoesNotThrow(() =>
            _missingSutShould.Not.BeVisible());

    [Test]
    public void BeHidden_VisibleComponent() =>
        AssertThrowsWithoutInnerException<AssertionException>(() =>
            _visibleSutShould.BeHidden());

    [Test]
    public void BeHidden_HiddenComponent() =>
        Assert.DoesNotThrow(() =>
            _hiddenSutShould.BeHidden());

    [Test]
    public void BeHidden_MissingComponent() =>
        AssertThrowsWithoutInnerException<AssertionException>(() =>
          _missingSutShould.BeHidden());

    [Test]
    public void Not_BeHidden_VisibleComponent() =>
        Assert.DoesNotThrow(() =>
            _visibleSutShould.Not.BeHidden());

    [Test]
    public void Not_BeHidden_HiddenComponent() =>
        AssertThrowsWithoutInnerException<AssertionException>(() =>
            _hiddenSutShould.Not.BeHidden());

    [Test]
    public void Not_BeHidden_MissingComponent() =>
        Assert.DoesNotThrow(() =>
            _missingSutShould.Not.BeHidden());

    [Test]
    public void BeEnabled_VisibleComponent() =>
        Assert.DoesNotThrow(() =>
            _visibleSutShould.BeEnabled());

    [Test]
    public void BeEnabled_HiddenComponent() =>
        Assert.DoesNotThrow(() =>
            _hiddenSutShould.BeEnabled());

    [Test]
    public void BeEnabled_MissingComponent() =>
        AssertThrowsWithInnerException<AssertionException, ElementNotFoundException>(() =>
            _missingSutShould.BeEnabled());

    [Test]
    public void Not_BeEnabled_VisibleComponent() =>
        AssertThrowsWithoutInnerException<AssertionException>(() =>
            _visibleSutShould.Not.BeEnabled());

    [Test]
    public void Not_BeEnabled_HiddenComponent() =>
        AssertThrowsWithoutInnerException<AssertionException>(() =>
            _hiddenSutShould.Not.BeEnabled());

    [Test]
    public void Not_BeEnabled_MissingComponent() =>
        AssertThrowsWithInnerException<AssertionException, ElementNotFoundException>(() =>
            _missingSutShould.Not.BeEnabled());
}
