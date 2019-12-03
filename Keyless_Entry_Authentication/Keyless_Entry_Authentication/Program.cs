using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Keyless_Entry_Authentication.Services;

namespace Keyless_Entry_Authentication
{
    public class Program
    {

        private static int _attempts;
        private static bool _canAuthenticate = true;
        private static readonly int _allowedAttempts = 2;
        private static DateTime _now;
        private static DateTime _end;

        public static void Main(string[] args)
        {
            // TODO: Commenting out for now, in future tasks we will need to update
            //       functionality such that the transmission service logic is in the
            //       Keyless_Entry_Transmission solution and the message sent contains
            //       the keyId and the transmission

            var _authenticationService = new KeylessEntryAuthentication();

            var _serverService = new ServerService();
            TcpListener server = null;

            try
            {
                server = _serverService.CreateServer(IPAddress.Parse("192.168.1.144"), 13000);

                server.Start();

                // Buffer for reading data
                var bytes = new byte[256];
                string data = null;

                
                _end = DateTime.Now.AddSeconds(10);

                // Enter the listening loop.
                while (true)
                {
                    _now = DateTime.Now;
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    var client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    var stream = client.GetStream();

                    int i;

                    if (_canAuthenticate)
                    {
                        if (_attempts < _allowedAttempts)
                        {
                            _attempts++;

                            // Loop to receive all the data sent by the client.
                            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                            {
                                // Translate data bytes to a ASCII string.
                                data = Encoding.ASCII.GetString(bytes, 0, i);
                                break;
                            }

                            var transmissions = data.Split(" ");

                            var keyId = int.Parse(transmissions[0]);
                            var keyTransmission = Encoding.ASCII.GetBytes(transmissions[1]);
                            var res = _authenticationService.TwoFactorAuthenticate(keyId, keyTransmission);

                            if (res)
                            {
                                Console.WriteLine("Authentication successful - car unlocked.");
                                break;
                            }

                            Console.WriteLine("Authentication failed.");
                        }
                        else
                        {
                            _attempts++;
                            _canAuthenticate = false;
                        }

                        CheckTimer(_now, _end);
                    }
                    else
                    {
                        Console.WriteLine("The allowed number of authentication attempts has been exceeded.");

                        _attempts++;
                        CheckTimer(_now, _end);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("SocketException: {0}", ex);
            }
            finally
            {
                server.Stop();
            }
        }

        private static void CheckTimer(DateTime now, DateTime end)
        {
            if (now > end)
            {
                _attempts = 0;
                _canAuthenticate = true;
                _end = DateTime.Now.AddSeconds(10);
            }
        }
    }
}
