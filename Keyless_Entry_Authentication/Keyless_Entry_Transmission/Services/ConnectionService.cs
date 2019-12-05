﻿using System;
using System.Net.Sockets;

namespace Keyless_Entry_Transmission.Services
{
    public class ConnectionService
    {
        public void Connect(string server, string message)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                var port = 13000;
                var client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                var data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                var stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent by Key: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new byte[256];

                // String to store the response ASCII representation.
                var responseData = string.Empty;

                // Read the first batch of the TcpServer response bytes.
                var bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received by Key: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }
    }
}