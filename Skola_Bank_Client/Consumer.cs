using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Skola_Bank_Client
{
    internal class Consumer : User//, IConsumer TODO fix language version to be compatible with private interface methods
    {
        List<Deposit> deposits;
        public Consumer(string firstName, string lastName, string socialSecurityNumber, List<Deposit> deposits) : base(firstName, lastName, socialSecurityNumber)
        {
            this.deposits = deposits;
        }

        public override void LoggedinMenu()
        {
            MenuManager menuManager = InitMenuManager();
            while (true)
            {
                try
                {
                    int selectedOption = menuManager.MainMenu();
                    ForwardSelectedOption(selectedOption);
                }
                catch (ReturnToMenu)
                {
                    break;
                }
            }
        }

        MenuManager InitMenuManager()
        {
            string title = $"Welcome {firstName}! Please select what you wish to do";
            string[] options = { "Perform transaction", "Manage deposits", "Change user information", "Logout" };
            return new MenuManager(title, options);
        }

        void ForwardSelectedOption(int selectedOption)
        {
            switch(selectedOption)
            {
                case 0:
                    PerformTransaction();
                    break;
                case 1:
                    ManageDeposits();
                    break;

                case 2:
                    ChangeUserInformation();
                    break;

                case 3:
                    throw new ReturnToMenu();

            }
        }

        void PerformTransaction()
        {
            SocketComm.SendMsg("transaction");
            MenuManager selectTransactionMenu = new MenuManager("What sort of transaction do you wish to do?", new string[] { "perform transaction within personal deposits", "perform transaction with another consumer" });
            int chosenOption = selectTransactionMenu.MainMenu();
            if (chosenOption == 0)
                LocalTransaction();
            else
                OnlineTransaction();
        }

        void LocalTransaction()
        {
            SocketComm.SendMsg("local");
            MenuManager selectDepositMenu = new MenuManager("Please select the deposit you wish to move money from", ExtractDepositsInfo());
            int givingDepositId = selectDepositMenu.MainMenu();

            selectDepositMenu.Title = "Please select the deposit you wish to move money to";
            int recievingDepositId = selectDepositMenu.MainMenu();

            double transactionAmmount = GetTransactionAmmount(deposits[givingDepositId].Balance);

            SocketComm.SendMsg($"{givingDepositId}|{recievingDepositId}|{transactionAmmount}");

            deposits[givingDepositId].Balance = deposits[givingDepositId].Balance - transactionAmmount;
            deposits[recievingDepositId].Balance = deposits[recievingDepositId].Balance + transactionAmmount;

            Console.WriteLine("Transaction performed, returning to menu...");
            Thread.Sleep(2000);

        }

        void OnlineTransaction()
        {
            throw new NotImplementedException();
        }

        string[] ExtractDepositsInfo()
        {
            string[] extractedDeposits = new string[deposits.Count];
            int currentDeposit = 0;
            foreach (Deposit deposit in deposits)
            {
                extractedDeposits[currentDeposit] = $"{deposit.Name} - {deposit.Balance} kr";
            }
            return extractedDeposits;
        }

        // runs checks on the desired transaction ammount to avoid a negative or too large ammount to be transacted
        double GetTransactionAmmount(double givingDepositBalance)
        {
            while (true)
            {
                double transactionAmmount = ParseDouble("Please enter the ammount of money you wish to exchange: ");
                if (transactionAmmount > givingDepositBalance)
                    Console.WriteLine("The desired ammount is larger than the giving deposits balance, please enter a smaller ammount");

                else if (transactionAmmount < 0)
                    Console.WriteLine("Cannot transact a negative ammount, please enter a ammount larger than or equal to zero");
                else
                    return transactionAmmount;
            }
        }

        double ParseDouble(string question)
        {
            while(true)
            {
                try
                {
                    Console.Write(question);
                    double parsedDouble = double.Parse(Console.ReadLine());
                    return parsedDouble;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        

        void ManageDeposits()   // TODO fix this later
        {
            SocketComm.SendMsg("addDeposit");
            SocketComm.SendMsg($"deposit {deposits.Count()}|{deposits.Count()}|500");
            Console.WriteLine("Deposit added");
            Console.WriteLine("returning to menu...");
            Thread.Sleep(2000);
        }

        protected override void ChangeUserInformation()
        {
            throw new NotImplementedException();
        }

        protected override void ChangeUsername()
        {
            throw new NotImplementedException();
        }

    }
}
