namespace Atata.IntegrationTests.Controls;

public class CustomControlTests : WebDriverSessionTestSuite
{
    [Test]
    public void CustomDatePicker_WithFormat()
    {
        var sut = Go.To<InputPage>().DatePicker;

        sut.Should.BeNull();

        DateTime value = new(2018, 7, 11);
        sut.Set(value);
        sut.Should.Equal(value);

        sut.Set(null);
        sut.Should.BeNull();
    }
}
