using System.Net;
using System.Net.Sockets;

namespace Atata.UnitTests.Utils;

public sealed class PortUtilsTests
{
    public sealed class FindAvailablePort
    {
        [Test]
        public void WhenReturnsSuccessfully()
        {
            int result = PortUtils.FindAvailablePort();

            result.Should().BePositive();
        }
    }

    public sealed class IsPortAvailable
    {
        [Test]
        public void WhenIsAvailable()
        {
            bool result = PortUtils.IsPortAvailable(29876);

            result.Should().BeTrue();
        }

        [Test]
        public void WhenIsAvailable_CallTwice()
        {
            WhenIsAvailable();
            WhenIsAvailable();
        }

        [Test]
        public void WhenIsNotAvailable()
        {
            TcpListener listener = new(IPAddress.Loopback, 29876);
            listener.Start();

            bool result;

            try
            {
                result = PortUtils.IsPortAvailable(29876);
            }
            finally
            {
                listener.Stop();
            }

            result.Should().BeFalse();
        }
    }
}
