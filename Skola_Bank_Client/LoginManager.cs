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
            bool loggingIn = true;
            while (loggingIn)
            {
                string fullName = GetFirstAndLastName();
                SocketComm.SendMsg(fullName);
                ServerValidateCredentials();
            }

            SocketComm.ServerTrueOrFalseResponse(); // if the inserted user is a admin, get true
        }

        static string GetFirstAndLastName()
        {
            Console.Write("Please enter your first name: ");
            string firstName = Console.ReadLine();
            Console.Write("Please enter your last name: ");
            string lastName = Console.ReadLine();
            return $"{firstName}|{lastName}";
        }
        static void ServerValidateCredentials()
        {

            try
            {
                SocketComm.ServerTrueOrFalseResponse();
            }
            catch (ServerFalseResponseException)
            {
                Console.WriteLine("The entered names were incorrect or does not have an account, please try again");
            }
        }
    }
}
