#nullable enable

using System.Net;
using System.Net.Sockets;

namespace Atata;

internal static class PortUtils
{
    internal static int FindFreePort()
    {
        using Socket portSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint socketEndPoint = new IPEndPoint(IPAddress.Any, 0);
        portSocket.Bind(socketEndPoint);
        return ((IPEndPoint)portSocket.LocalEndPoint).Port;
    }

    internal static int FindFreePortExcept(IReadOnlyList<int> portsToIgnore)
    {
        for (int i = 0; i < 500; i++)
        {
            int port = FindFreePort();

            if (!portsToIgnore.Contains(port))
                return port;
        }

        throw new InvalidOperationException("Failed to find free port.");
    }
}
