using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skola_Bank_Client
{
    internal class DepositStorage<T>
    {
        T[] depositArray;
        int stackPointer = 0;
        int maxAmmount = 10;
        public DepositStorage()
        { depositArray = new T [maxAmmount]; }

        public void Add(T newItem)
        {
            if (stackPointer >= maxAmmount)
                throw new MaxDepositsException();
            depositArray [stackPointer++] = newItem;
        }

        public void Remove(int arrayPosition)
        {
            if (stackPointer - 1 > arrayPosition)
                throw new MissingMemberException();

            for (int i = arrayPosition + 1; i < stackPointer; i++)
            {
                depositArray[i - 1] = depositArray[i];
            }
            depositArray[stackPointer] = default;
            stackPointer--;
        }
    }
}
