namespace Atata.IntegrationTests.Controls.Inputs;

public class FileInputTests : WebDriverSessionTestSuite
{
    private InputPage _page;

    protected override void OnSetUp() =>
        _page = Go.To<InputPage>();

    [Test]
    public void Interact()
    {
        var sut = _page.FileInput;

        sut.Should.BePresent();
        sut.Should.BeVisible();

        Test(sut);
    }

    [Test]
    public void Interact_WhenHidden()
    {
        var sut = _page.HiddenFileInput;

        sut.Should.BePresent();
        sut.Should.BeHidden();

        Test(sut);
    }

    [Test]
    public void Interact_WhenTransparent()
    {
        var sut = _page.TransparentFileInput;

        sut.Should.BePresent();
        sut.Should.BeHidden();

        Test(sut);
    }

    private void Test(FileInput<InputPage> sut)
    {
        VerifyEquals(sut, string.Empty);

        string file1Name = $"{GetType().Assembly.GetName().Name}.dll";
        sut.Set(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file1Name));
        sut.Should.EndWith(file1Name);

        string file2Name = $"{typeof(OrdinaryPage).Assembly.GetName().Name}.dll";
        sut.Set(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file2Name));
        sut.Should.EndWith(file2Name);

        sut.Clear();
        sut.Should.BeEmpty();
    }
}
