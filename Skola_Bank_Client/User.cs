using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skola_Bank_Client
{
    internal abstract class User
    {

        protected string firstName;
        protected string lastName;
        protected string socialSecurityNumber;
        public User(string firstName, string lastName, string socialSecurityNumber)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.socialSecurityNumber = socialSecurityNumber;
        }
        public abstract void LoggedinMenu();
        protected abstract void ChangeUserInformation();
        protected abstract void ChangeUsername();

    }
}
