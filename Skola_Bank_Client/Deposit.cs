using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skola_Bank_Client
{
    internal class Deposit
    {
        
                string name;
                int id;
                double balance;
                public Deposit(string name, int id, double balance)
                {
                    this.name = name;
                    this.id = id;
                    this.balance = balance;
                }
                public Deposit(int id, double balance)
                {
                    this.name = $"Deposit {id}";
                    this.id = id;
                    this.balance = balance;
                }

                public string Name { get { return name; } set { name = value; } }
                public int Id { get { return id; } }
                public double Balance { get { return balance; } set { balance = value; } }
            }
}
