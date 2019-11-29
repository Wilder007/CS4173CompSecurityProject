using System;
using System.Net;
using System.Net.Sockets;

namespace Keyless_Entry_Authentication.Interfaces
{
    public interface IServerService
    {
        TcpListener CreateServer(IPAddress localAddr, int port);
    }
}
