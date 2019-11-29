using System;
using System.Net;
using System.Net.Sockets;
using Keyless_Entry_Authentication.Interfaces;

namespace Keyless_Entry_Authentication.Services
{
    public class ServerService : IServerService
    {
        public TcpListener CreateServer(IPAddress localAddr, int port)
        {
            return new TcpListener(localAddr, port);
        }
    }
}
