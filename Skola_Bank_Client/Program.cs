using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;

namespace Skola_Bank_Client
{
    internal class Program
    {
        static MenuManager mainMenu = new MenuManager(new string[] { "Create account", "Login", "Exit application" });
        static void Main(string[] args)
        {
            string address = "127.0.0.1";
            int port = 8001;
            Console.WriteLine("Connecting to server...");
            /*TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient = AttemptToConnectToServer(tcpClient, address, port);
            }
            catch
            {
                Console.WriteLine("Press enter to exit the application");
                Console.ReadLine();
                Environment.Exit(0);
            }*/
            mainMenu.MainMenu();


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
            finally
            {
                throw new ServerUnavalibleException();
            }
        }
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
                }
            }
            throw new ServerUnavalibleException();
        }
    }
}
