﻿
using Spectre.Console;
using Figgle;

namespace BankAccount02GeneriskaKlassOchJson
    
{
    public class Program
    {
        static void Main(string[] args)
        {

            

            BankSystem bankSystem = new BankSystem();


            DisplayUserBankAccountSystemInteraction userBankAccountSystemInteraction = new DisplayUserBankAccountSystemInteraction();
            userBankAccountSystemInteraction.Run();
        }
    }
}
