﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;

namespace Skola_Bank_Client
{
    internal class Program
    {
        static MenuManager mainMenu = new MenuManager(new string[] { "Create account", "Login", "Exit application" });
        static void Main(string[] args)
        {
            TcpInformation.Init();
            while (connected)
            {
                int chosenOption = mainMenu.MainMenu();
                NavigateToSelectedOption(chosenOption);
            }


        }

        
        static void NavigateToSelectedOption(int chosenOption)
        {
            switch(chosenOption)
            {
                case 0:
                    CreateUser();
                    break;
                case 1:
                    break;
                case 2:
                    break;
                default:
                    throw new InvalidArgumentException();

            }
        }

        static void CreateUser()
        {
            string userCredentials = "";
            Console.WriteLine("press enter without having entered any info if you wish to cancel the process of creating a user");
            try
            {
                userCredentials = GetUserCredentials();  
            }
            catch (ReturnToMenu)
            {
                return;
            }
            SocketComm.SendMsg()
        }

        static string GetUserCredentials()
        {
            string name = GetFirstAndLastName();
            string socialSecurityNumber = GetSocialSecurityNumber();
            return $"{name}|{socialSecurityNumber}";
        }

        static string GetFirstAndLastName()
        {
            string firstName = GetFistName();
            string lastName = GetLastName();
            return $"{firstName}|{lastName}";
        }

        static string GetFistName()
        {
            Console.Write("Please enter your first name: ");
            string firstName = Console.ReadLine();
            CheckIfInputIsEmpty(firstName);
            return firstName;
        }

        static string GetLastName()
        {
            Console.Write("Please enter your last name: ");
            string lastName = Console.ReadLine();
            CheckIfInputIsEmpty(lastName);
            return lastName;
        }

        static string GetSocialSecurityNumber()
        {
            string socialSecurityNumber;
            while(true)
            { 
                try
                {
                    Console.Write("Please enter your social security number (YYYYMMDDXXXX): ");
                    socialSecurityNumber = Console.ReadLine();

                    CheckIfInputIsEmpty(socialSecurityNumber);
                    ValidateSocialSecurityNumber(socialSecurityNumber);
                    return socialSecurityNumber;
                }
                catch (InvalidSocialSecurityNumberException)
                {
                    Console.WriteLine("The entered social security number is invalid, please enter your social security number again");
                }
            }
        }

        static void CheckIfInputIsEmpty(string input)
        {
            if (input.Length == 0)
                throw new ReturnToMenu();
        }
        static void ValidateSocialSecurityNumber(string socialSecurityNumber)   // parses year and date of the social security number and makes sure that they
        {                                                                       // are 
            if (socialSecurityNumber.Length != 12)
                throw new InvalidSocialSecurityNumberException();

            char[] socialSecurityChars = socialSecurityNumber.ToCharArray();
            ValidateYear(socialSecurityChars);
            ValidateDate(socialSecurityChars);
        }

        static void ValidateYear(char[] socialSecurityNumber)
        {
            int year = int.Parse(new string(socialSecurityNumber, 0, 4));

            if (year > 2022 || year < 1850)
                throw new InvalidSocialSecurityNumberException();
        }
        static void ValidateDate(char[] socialSecurityNumber)
        {
            int month = int.Parse(new string(socialSecurityNumber, 4, 2));
            int day = int.Parse(new string(socialSecurityNumber, 6, 2));

            if (month > 12 || month < 1 || day > 31 || day < 1)
                throw new InvalidSocialSecurityNumberException();
        }
    }
}
