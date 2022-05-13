using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Skola_Bank_Client
{
    internal class Consumer : User, IConsumer
    {
        DepositStorage<Deposit> deposits;
        public Consumer(string firstName, string lastName, string socialSecurityNumber, DepositStorage<Deposit> deposits) : base(firstName, lastName, socialSecurityNumber)
        {
            this.deposits = deposits;
        }

        public override void LoggedinMenu()
        {
            MenuManager menuManager = InitMenuManager();
            while (true)
            {
                int selectedOption = 0;
                // if esc was pressed while in the menu
                try
                {
                    selectedOption = menuManager.MainMenu();
                }
                catch (ReturnToMenu)
                {
                    SocketComm.SendMsg("logout");
                    break;
                }
                // if esc was pressed somewhere else, it does not log you out
                try
                {
                    ForwardSelectedOption(selectedOption);
                }
                catch(ReturnToMenu)
                {
                    SocketComm.SendMsg("back");
                }
            }
        }

        MenuManager InitMenuManager()
        {
            string title = $"Welcome {firstName}! Please select what you wish to do";
            string[] options = { "Perform transaction", "Manage deposits", "Delete account", "Logout" };
            return new MenuManager(title, options);
        }

        void ForwardSelectedOption(int selectedOption)
        {
            switch(selectedOption)
            {
                case 0:
                    SocketComm.SendMsg("transaction");
                    LocalTransaction();
                    //PerformTransaction(); scrapped idea of also being able to perform transactions between users
                    break;
                case 1:
                    ManageDeposits();
                    break;

                case 2:
                    DeleteAccount();
                    break;

                case 3:
                    throw new ReturnToMenu();

            }
        }

        public void PerformTransaction()
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

            double transactionAmmount = GetTransactionAmmount(deposits.Deposit[givingDepositId].Balance);

            SocketComm.SendMsg($"{givingDepositId}|{recievingDepositId}|{transactionAmmount}");

            deposits.Deposit[givingDepositId].Balance = deposits.Deposit[givingDepositId].Balance - transactionAmmount;
            deposits.Deposit[recievingDepositId].Balance = deposits.Deposit[recievingDepositId].Balance + transactionAmmount;

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
            for (int i = 0; i < deposits.Count; i++)
                extractedDeposits[i] = $"{deposits.Deposit[i].Name} - {deposits.Deposit[i].Balance} kr";
           
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

        

        public void ManageDeposits()
        {
            MenuManager manageDepositMenu = new MenuManager("Please select the desired option", new string[] {"Add deposit", "delete deposit"});
            int selectedOption = manageDepositMenu.MainMenu();
            if (selectedOption == 0)
                AddDeposit();
            else
                DeleteDeposit();
            
            Thread.Sleep(2000);
        }

        void AddDeposit()
        {
            try
            {
                deposits.Add(new Deposit(deposits.Count, 500));
            }
            catch(MaxDepositsException)
            {
                Console.WriteLine("max deposits reached, please delete a deposit and try again");
                return;
            }
            SocketComm.SendMsg("addDeposit");
            SocketComm.SendMsg($"deposit {deposits.Count - 1}|{deposits.Count - 1}|500");
            Console.WriteLine("Deposit added");
            Console.WriteLine("returning to menu...");
        }

        void DeleteDeposit()
        {
            SocketComm.SendMsg("deleteDeposit");
            MenuManager depositsMenu = new MenuManager("Please select the deposit you wish to delete", ExtractDepositsInfo());
            int deletedDepositId = depositsMenu.MainMenu();
            SocketComm.SendMsg($"{deletedDepositId}");
            deposits.Remove(deletedDepositId);
            Console.WriteLine("Deposit deleted, returing to previous menu...");
            Thread.Sleep(2000);
            Console.Clear();
        }

        protected override void ChangeUserInformation()
        {
            throw new NotImplementedException();
        }

        protected override void ChangeUsername()
        {
            throw new NotImplementedException();
        }

        void DeleteAccount()
        {
            MenuManager deleteAccount = new MenuManager("are you sure you want to delete your account? once deleted it cannot be reverted", new string[] {"yes", "no"});
            int selectedOption = deleteAccount.MainMenu();

            if (selectedOption == 0)
            {
                Console.WriteLine("Account deleted, returning to menu...");
                SocketComm.SendMsg("deleteAccount");
                Thread.Sleep(2000);
                throw new DeletedAccount();
            }
                
        }

        public void DisplayInfo()
        {
            Console.WriteLine(firstName);
            Console.WriteLine(lastName);
            Console.WriteLine(socialSecurityNumber);
            for (int i = 0; i < deposits.Count; i++)
                Console.WriteLine($"{deposits.Deposit[i].Name} - {deposits.Deposit[i].Id} - {deposits.Deposit[i].Balance}");
        }

    }
}
