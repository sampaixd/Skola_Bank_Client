using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skola_Bank_Client
{
    internal class LoginManager
    {
        static public void Login()
        {
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
                string fullName = GetFirstAndLastName();
                SocketComm.SendMsg(fullName);
                nameValidated = ServerValidateCredentials();
            }
        }

        static string GetFirstAndLastName()
        {
            string firstName = GetInput("Please enter your first name: ");
            string lastName = GetInput("Please enter your last name: ");
            return $"{firstName}|{lastName}";
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
                Console.WriteLine("Temp");  // TODO add login to basic user

        }

        static void AdminLogin()
        {
            Console.Write("Please enter your password: ");
            string password = Console.ReadLine();
            SocketComm.SendMsg(password);
        }
    }
}
