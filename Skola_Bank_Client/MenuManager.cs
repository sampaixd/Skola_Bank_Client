using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skola_Bank_Client
{
    internal class MenuManager  // class used for navigating and viewing the menu
    {
        string title;
        string[] options;
        int currentlySelectedOption;
        public MenuManager(string title, string[] options)
        {
            this.title = title;
            this.options = options;
            this.currentlySelectedOption = 0;
        }
        public int MainMenu()
        {
            while (true)
            {
                MainMenuUI();
                int UpdatedSelectedOption = NavigateMainMenu();
                if (UpdatedSelectedOption == -2)     // enter
                {
                    return currentlySelectedOption;
                }
                else if (UpdatedSelectedOption == -1)    // escape
                {
                    return -1;
                }
                else    // up, down or invalid input
                {
                    currentlySelectedOption = UpdatedSelectedOption;
                }
                Console.Clear();
            }
        }
        public void MainMenuUI()
        {
            Console.WriteLine(title + "\n");
            for (int i = 0; i < options.Length; i++)
            {
                DisplayOption(i == currentlySelectedOption, options[i]);
            }
        }

        void DisplayOption(bool isSelected, string option)
        {
            if (isSelected)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.WriteLine(option);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        public int NavigateMainMenu()
        {
            Console.ForegroundColor = ConsoleColor.Black;   // makes the pressed key invisible, looks alot better
            ConsoleKeyInfo pressedKey = Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.White;
            switch (pressedKey.Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    return SubtractCurrentOption();


                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    return IncrementCurrentOption();


                case ConsoleKey.Enter:
                    return -2;


                case ConsoleKey.Escape:
                    return -1;
                default:
                    return currentlySelectedOption;

            }
        }

        int SubtractCurrentOption()
        {
            Console.WriteLine(currentlySelectedOption);
            if (currentlySelectedOption <= 0)   // the currently selected option will loop back to the bottom if it is already at the top
                return options.Length - 1;
            return --currentlySelectedOption;
        }

        int IncrementCurrentOption()
        {
            if (currentlySelectedOption >= options.Length - 1)   // same as SubtractCurrentOption but reversed to work the other way around
                return 0;
            return ++currentlySelectedOption;
        }

        public string Title { set { title = value; } }
    }
}
