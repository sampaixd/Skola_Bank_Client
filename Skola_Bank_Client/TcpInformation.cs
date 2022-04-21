using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;

namespace Skola_Bank_Client
{
    internal class TcpInformation
    {
        static TcpClient tcpClient;
        static NetworkStream tcpStream;
        static TcpInformation()
        {
            string address = "127.0.0.1";
            int port = 8001;
            Console.WriteLine("Connecting to server...");
            
            tcpClient = new TcpClient();
            try
            {
                tcpClient = AttemptToConnectToServer(tcpClient, address, port);
            }
            catch(Exception e)
            {
                Console.WriteLine("Press enter to exit the application");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.ReadLine();
                Environment.Exit(0);
            }
            tcpStream = tcpClient.GetStream();
        }
        static TcpClient AttemptToConnectToServer(TcpClient tcpClient, string address, int port)
        {
            try
            {
                return ConnectToServer(tcpClient, address, port);
            }
            catch (ServerUnavalibleException)
            {
                Console.WriteLine("Server is currently unavalible, please try again at a later date");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            throw new ServerUnavalibleException();

        }
        // tries 10 times to connect with a 10 second intervall, throws a exception if this fails
        static TcpClient ConnectToServer(TcpClient tcpClient, string address, int port)
        {
            int attempts = 0;
            while (attempts < 10)
            {
                try
                {
                    tcpClient.Connect(address, port);
                    return tcpClient;
                }
                catch (Exception)
                {
                    Console.WriteLine("Could not connect to server, trying again in 10 seconds...");
                    Thread.Sleep(10000);
                    attempts++;
                }
            }
            throw new ServerUnavalibleException();
        }
        static public void Init()  // since we want to connect to the server as soon as we start the application, this method is called to initalize and start that process
        { }

        static public TcpClient GetTcpClient { get { return tcpClient; } }

        static public NetworkStream GetTcpStream { get { return tcpStream; } }
    }
}
