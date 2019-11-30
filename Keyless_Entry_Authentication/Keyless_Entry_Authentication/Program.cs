using System;
using System.Net;
using System.Net.Sockets;
using Keyless_Entry_Authentication.Services;

namespace Keyless_Entry_Authentication
{
    public class Program
    {

        public static void Main(string[] args)
        {
            // TODO: Commenting out for now, in future tasks we will need to update
            //       functionality such that the transmission service logic is in the
            //       Keyless_Entry_Transmission solution and the message sent contains
            //       the keyId and the transmission

            //var _transmissionService = new TransmissionService();
            //_transmissionService.CreateTransmissions();

            var _serverService = new ServerService();
            TcpListener server = null;

            try
            {
                server = _serverService.CreateServer(IPAddress.Parse("192.168.1.144"), 13000);

                server.Start();

                // Buffer for reading data
                var bytes = new byte[256];
                string data = null;

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    var client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    var stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0) 
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received by Car: {0}", data);

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        var msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent by Car: {0}", data);
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
    }
}
