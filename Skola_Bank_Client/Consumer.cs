using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skola_Bank_Client
{
    internal class Consumer : User, IConsumer
    {
        List<Deposit> deposits;
        public Consumer(string firstName, string lastName, string socialSecurityNumber, List<Deposit> deposits) : base(firstName, lastName, socialSecurityNumber)
        {
            this.deposits = deposits;
        }

        public override void LoggedinMenu()
        {
            MenuManager menuManager = InitMenuManager();
            while(true)
            {
                try
                {
                    int selectedOption = menuManager.MainMenu();
                    ForwardSelectedOption(selectedOption);
                }
                catch(ReturnToMenu)
                {
                    break;
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

                case 3:
                    throw new ReturnToMenu();

            }
        }
    }
}
