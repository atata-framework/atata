using System.Net;
using System.Net.Sockets;

namespace Atata;

public static class PortUtils
{
    public static int FindAvailablePort()
    {
        using Socket portSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint socketEndPoint = new IPEndPoint(IPAddress.Any, 0);
        portSocket.Bind(socketEndPoint);
        return ((IPEndPoint)portSocket.LocalEndPoint!).Port;
    }

    public static int FindAvailablePortExcept(IReadOnlyList<int> portsToIgnore)
    {
        for (int i = 0; i < 500; i++)
        {
            int port = FindAvailablePort();

            if (!portsToIgnore.Contains(port))
                return port;
        }

        throw new InvalidOperationException("Failed to find free port.");
    }

    public static bool IsPortAvailable(int port)
    {
        try
        {
            TcpListener listener = new(IPAddress.Loopback, port);
            listener.Start();
            listener.Stop();
            return true;
        }
        catch (SocketException)
        {
            return false;
        }
    }
}
