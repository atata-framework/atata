namespace Atata.IntegrationTests;

internal static class FakeWebDriver
{
    public static IWebDriver Create()
    {
        var driverMock = new Mock<IWebDriver>();
        var driverManageMock = new Mock<IOptions>();
        var driverTimeoutsMock = new Mock<ITimeouts>();

        driverMock.Setup(x => x.Manage()).Returns(driverManageMock.Object);
        driverManageMock.Setup(x => x.Timeouts()).Returns(driverTimeoutsMock.Object);

        return driverMock.Object;
    }
}
