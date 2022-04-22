using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Skola_Bank_Client
{
    internal class LoginManager
    {
        static public void Login()
        {
            SocketComm.SendMsg("login");
            try
            {
                BasicLogin();
                CheckIfAdmin();
            }
            catch (ReturnToMenu)
            {
                return;
            }
        }

        static void BasicLogin()
        {
            bool nameValidated = false;
            while (!nameValidated)
            {
                string credentials = GetUserCredentials();
                SocketComm.SendMsg(credentials);
                nameValidated = ServerValidateCredentials();
            }
        }

        static string GetUserCredentials()
        {
            string firstName = GetInput("Please enter your first name: ");
            string lastName = GetInput("Please enter your last name: ");
            string socialSecurityNumber = GetInput("Please enter your social security number: ");
            return $"{firstName}|{lastName}|{socialSecurityNumber}";
        }

        static string GetInput(string question)
        {
            Console.Write(question);
            string input = Console.ReadLine();
            CheckIfInputIsEmpty(input);
            return input;

        }
        static void CheckIfInputIsEmpty(string input)
        {
            if (input.Length == 0)
                throw new ReturnToMenu();
        }
        static bool ServerValidateCredentials()
        {
            if (SocketComm.ServerTrueOrFalseResponse())
                return true;
            else
            {
                Console.WriteLine("The inserted names were either incorrect or does not have an account, please try again");
                return false;
            }

        }

        static void CheckIfAdmin()
        {
            if (SocketComm.ServerTrueOrFalseResponse())  // if the inserted user is a admin, get true
                AdminLogin();
            else
                Console.WriteLine("Successfully logged in");  // TODO add login to basic user

        }

        static void AdminLogin()
        {
            string password = " ";
            while (password != "")
            { 
                Console.Write("Please enter your password: ");
                password = Console.ReadLine();
                if (password == "")
                    continue;
                SocketComm.SendMsg(password);
                if (SocketComm.ServerTrueOrFalseResponse())
                { 
                    Console.WriteLine("Logged in as admin");    // TODO here
                    Thread.Sleep(2000);
                    return;
                }
                else
                {
                    Console.WriteLine("login as admin failed");
                }
                        
            }
        }
    }
}
