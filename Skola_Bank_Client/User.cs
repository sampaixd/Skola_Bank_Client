using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skola_Bank_Client
{
    internal abstract class User
    {
            
        string username;
        string socialSecurityNumber;
        public User(string username, string socialSecurityNumber)
        {
            this.username = username;
            this.socialSecurityNumber = socialSecurityNumber;
        }

        protected abstract void Login();
        protected abstract void LoggedinMenu();
        protected abstract void ChangeUserInformation();
        protected abstract void ChangePassword();
        protected abstract void ChangeUsername();
        protected abstract void ChangeSocialSecurityNumber();

    }
}
