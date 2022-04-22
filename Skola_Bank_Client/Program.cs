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
        static MenuManager mainMenu = new MenuManager("Welcome to the lorem ipsum bank!", new string[] { "Create account", "Login", "Exit application" });
        static void Main(string[] args)
        {
            TcpInformation.Init();
            bool connected = true;
            while (connected)
            {
                int chosenOption = mainMenu.MainMenu();
                Console.Clear();
                NavigateToSelectedOption(chosenOption);
            }


        }

        
        static void NavigateToSelectedOption(int chosenOption)
        {
            switch(chosenOption)
            {
                case 0:
                    UserCreator.CreateUser();
                    break;

                case 1:
                    LoginManager.Login();
                    break;

                case 2:
                case -1:    // if esc was pressed in main menu
                    Environment.Exit(0);
                    break;

                default:
                    throw new InvalidArgumentException();

            }
        }
    }
}
