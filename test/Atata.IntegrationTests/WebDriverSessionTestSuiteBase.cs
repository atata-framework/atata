namespace Atata.IntegrationTests;

[TestFixture]
public abstract class WebDriverSessionTestSuiteBase : TestSuiteBase
{
    public const int TestAppPort = 50549;

    /// <summary>
    /// Usage of 2046 on Azure DevOps pipeline port often leads to failure during WebDriver instance creation.
    /// </summary>
    private readonly int[] _portsToIgnore = [2046];

    public static string BaseUrl { get; } = $"http://localhost:{TestAppPort}";

    protected static IEnumerable<string> ChromeArguments { get; } =
    [
        "window-size=1200,800",
        "headless=new",
        "disable-search-engine-choice-screen"
    ];

    protected AtataContext BuildAtataContextWithWebDriverSession(
        Action<WebDriverSessionBuilder> configureWebDriverSession = null) =>
        ConfigureAtataContextWithWebDriverSession(configureWebDriverSession)
            .Build();

    protected AtataContextBuilder ConfigureAtataContextWithWebDriverSession(
        Action<WebDriverSessionBuilder> configureWebDriverSession = null)
    {
        AtataContextBuilder atataContextBuilder = ConfigureSessionlessAtataContext();

        atataContextBuilder.Sessions.AddWebDriver(session =>
        {
            session.UseBaseUrl(BaseUrl)
                .UseChrome()
                .WithArguments(ChromeArguments)
                .WithPortsToIgnore(_portsToIgnore)
                .WithInitialHealthCheck();

#warning Temporarily uses NUnit features.
            session.EventSubscriptions.TakeScreenshotOnNUnitError();
            session.EventSubscriptions.TakePageSnapshotOnNUnitError();

            configureWebDriverSession?.Invoke(session);
        });

#warning Temporarily uses NUnit features.
        atataContextBuilder.UseNUnitTestName();
        atataContextBuilder.UseNUnitTestSuiteName();
        atataContextBuilder.UseNUnitTestSuiteType();
        atataContextBuilder.LogConsumers.AddNUnitTestContext();
        atataContextBuilder.EventSubscriptions.LogNUnitError();
        atataContextBuilder.EventSubscriptions.AddArtifactsToNUnitTestContext();

        return atataContextBuilder;
    }

    protected static void SetAndVerifyValues<T, TPage>(EditableField<T, TPage> control, params T[] values)
        where TPage : PageObject<TPage>
    {
        control.Should.BePresent();

        for (int i = 0; i < values.Length; i++)
        {
            control.Set(values[i]);
            control.Should.Equal(values[i]);
            Assert.That(control.Value, Is.EqualTo(values[i]));

            if (i > 0 && !Equals(values[i], values[i - 1]))
                control.Should.Not.Equal(values[i - 1]);
        }
    }

    protected static void SetAndVerifyValue<T, TPage>(EditableField<T, TPage> control, T value)
        where TPage : PageObject<TPage>
    {
        control.Set(value);
        VerifyEquals(control, value);
    }

    protected static AssertionException AssertThrowsAssertionExceptionWithUnableToLocateMessage(TestDelegate code)
    {
        AssertionException exception = AssertThrowsWithoutInnerException<AssertionException>(code);

        Assert.That(exception.Message, Does.Contain("Actual: unable to locate"));

        return exception;
    }

    protected static void AssertThatPopupBoxIsOpen() =>
        Assert.DoesNotThrow(() =>
            WebDriverSession.Current.Driver.SwitchTo().Alert());

    protected static void AssertThatPopupBoxIsNotOpen() =>
        Assert.Throws<NoAlertPresentException>(() =>
            WebDriverSession.Current.Driver.SwitchTo().Alert());

    protected void AssertThatLastLogSectionIsVerificationAndEmpty()
    {
        var entries = GetLastLogEntries(2);
        entries[0].SectionStart.Should().BeOfType<VerificationLogSection>();
        entries[1].SectionEnd.Should().Be(entries[0].SectionStart);
    }

    protected void AssertThatLastLogSectionIsVerificationWithExecuteBehavior()
    {
        var entries = GetLastLogEntries(4);
        entries[0].SectionStart.Should().BeOfType<VerificationLogSection>();
        entries[1].SectionStart.Should().BeOfType<ExecuteBehaviorLogSection>();
        entries[2].SectionEnd.Should().Be(entries[1].SectionStart);
        entries[3].SectionEnd.Should().Be(entries[0].SectionStart);
    }

    protected void AssertThatLastLogSectionIsVerificationWith2ElementFindSections()
    {
        var entries = GetLastLogEntries(6);
        entries[0].SectionStart.Should().BeOfType<VerificationLogSection>();
        entries[1].SectionStart.Should().BeOfType<ElementFindLogSection>();
        entries[2].SectionEnd.Should().Be(entries[1].SectionStart);
        entries[3].SectionStart.Should().BeOfType<ElementFindLogSection>();
        entries[4].SectionEnd.Should().Be(entries[3].SectionStart);
        entries[5].SectionEnd.Should().Be(entries[0].SectionStart);
    }

    protected void AssertThatLastLogSectionIsVerificationWithExecuteBehaviorAnd3ElementFindSections()
    {
        var entries = GetLastLogEntries(10);
        entries[0].SectionStart.Should().BeOfType<VerificationLogSection>();
        entries[1].SectionStart.Should().BeOfType<ExecuteBehaviorLogSection>();
        entries[2].SectionStart.Should().BeOfType<ElementFindLogSection>();
        entries[3].SectionEnd.Should().Be(entries[2].SectionStart);
        entries[4].SectionStart.Should().BeOfType<ElementFindLogSection>();
        entries[5].SectionEnd.Should().Be(entries[4].SectionStart);
        entries[6].SectionStart.Should().BeOfType<ElementFindLogSection>();
        entries[7].SectionEnd.Should().Be(entries[6].SectionStart);
        entries[8].SectionEnd.Should().Be(entries[1].SectionStart);
        entries[9].SectionEnd.Should().Be(entries[0].SectionStart);
    }
}
