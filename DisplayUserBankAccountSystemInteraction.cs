

using System.Text.Json;

namespace BankAccount02GeneriskaKlassOchJson
{
    public class DisplayUserBankAccountSystemInteraction
    {

        BankSystem bankSystem = new BankSystem();

        public void Run()
        {

            BankSystem bank= new BankSystem();
            string dataJsonFilePath = "BankAccountData.json";
            string alldatasomJSONType = File.ReadAllText(dataJsonFilePath);
            BankDB bankDB = JsonSerializer.Deserialize<BankDB>(alldatasomJSONType)!;
            BankSystem bankSystem = new BankSystem();



            bool running = true;

            while (running)
            {
                DiplayMenu();
                string userInputSomString = Console.ReadLine()!;
                int userInputInt = Convert.ToInt32(userInputSomString);

                switch (userInputInt)
                {

                    case 1 :
                        bankSystem.AddNewCustomer(bankDB);
                        break;
                    case 2 :
                        bankSystem.AddnewAccount(bankDB);
                        break;
                    case 3 :
                        bankSystem.Deposit(bankDB);
                        break;
                    case 4 :
                        bankSystem.Withdraw(bankDB);
                        break;
                    case 5 :
                        bankSystem.ViewAccountDetails(bankDB);
                        break;
                    case 6 :
                        bankSystem.SaveAllDataandExit(bankDB);
                        return;

                }

            }
        }

        private static void DiplayMenu()
        {
            Console.WriteLine("1. Add Customer");
            Console.WriteLine("2. Add Account");
            Console.WriteLine("3. Deposit Money");
            Console.WriteLine("4. Withdraw Money");
            Console.WriteLine("5. View Account Details");
            Console.WriteLine("6. Save All Data and Exit");
        }
    }
}
