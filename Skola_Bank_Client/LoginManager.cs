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
            // since 2 accounts can have the same first and last name, the social security number is essential in order to identify the account, sorry babis 
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
                ConsumerLogin();

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

        static void ConsumerLogin()
        {
            Consumer activeConsumer = GetConsumerInfo();
            activeConsumer.LoggedinMenu();
        }

        static Consumer GetConsumerInfo()
        {
            string recievedCredentials = "";

            recievedCredentials = SocketComm.RecvMsg();
            string[] splitRecievedCredentials = recievedCredentials.Split('|');

            string firstName = splitRecievedCredentials[0];
            string lastName = splitRecievedCredentials[1];
            string socialSecurityNumber = splitRecievedCredentials[2];

            DepositStorage<Deposit> deposits = GetDeposits(splitRecievedCredentials);

            Consumer activeConsumer = new Consumer(firstName, lastName, socialSecurityNumber, deposits);
            activeConsumer.DisplayInfo();
            return activeConsumer;
        }

        static DepositStorage<Deposit> GetDeposits(string[] splitRecievedCredentials)
        {
            DepositStorage<Deposit> deposits = new DepositStorage<Deposit>();
            // this is where deposits start in the recieved string, will be incremented
            int depositArrCurrentValue = 3;

            while(true)
            {
                if (splitRecievedCredentials[depositArrCurrentValue] == "depositEnd")
                    break;

                string name = splitRecievedCredentials[depositArrCurrentValue];
                int id = Convert.ToInt32(splitRecievedCredentials[depositArrCurrentValue + 1]);
                double balance = Convert.ToDouble(splitRecievedCredentials[depositArrCurrentValue + 2]);
                deposits.Add(new Deposit(name, id, balance));
                depositArrCurrentValue += 3;
            }

            return deposits;

        }

    }
}
