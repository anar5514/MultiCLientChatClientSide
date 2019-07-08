using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MultiClientChatWithTcpClient
{
    class Program
    {
        static string IpAddress = "10.1.16.11";
        static int TcpPort = 1031;

        static void Main(string[] args)
        {
            Task.Run(() => StartTcpClient()).Wait();

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }

        static void StartTcpClient()
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(IpAddress), TcpPort);
                TcpClient client = new TcpClient();
                Console.WriteLine("Starting to connect server");

                client.Connect(endPoint);
                while (true)
                {
                    // Translate the passed message into ASCII and store it as a Byte array.
                    string message = Console.ReadLine();
                    if (message == "exit") break;

                    byte[] data = Encoding.ASCII.GetBytes(message);

                    // Get a client stream for reading and writing.
                    NetworkStream stream = client.GetStream();

                    // Send the message to the connected TcpServer. 
                    stream.Write(data, 0, data.Length);

                    Console.WriteLine("Sent: {0}", message);

                    // Buffer to store the response bytes.
                    data = new byte[256];

                    // String to store the response ASCII representation.
                    string responseData = string.Empty;

                    // Read the first batch of the TcpServer response bytes.
                    int bytes = stream.Read(data, 0, data.Length);
                    responseData = Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("Received: {0}", responseData);

                }

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
        }

    }
}
