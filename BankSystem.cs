

using System.Data;
using System.Text.Json;
using System.Xml.Linq;

namespace BankAccount02GeneriskaKlassOchJson
{
    public class BankSystem
    {
        BankGeneriskAdministration<Customer> customerAdmin = new BankGeneriskAdministration<Customer>();
        BankGeneriskAdministration<BankAccount> accountsAdmin = new BankGeneriskAdministration<BankAccount>();


        public void AddNewCustomer(BankDB bankDB)
        {
            var customerAdmin = new BankGeneriskAdministration<Customer>();

            foreach (var c in bankDB.AllCustomersDatafromBankDB) 
            {

                customerAdmin.AddTo(c);

            }

            Console.WriteLine("Enter customer name:");
            string name = Console.ReadLine()!;
            Console.WriteLine("Enter customer address:");
            string address = Console.ReadLine()!;


            Customer newCustomer = new (bankDB.AllCustomersDatafromBankDB.Count+1,name,address)
            {
                Id = customerAdmin.GetAll().Count + 1,
                Name = name,
                Address = address,
                AccountIDs = new List<int>()
            };

            customerAdmin.AddTo(newCustomer);
            bankDB.AllCustomersDatafromBankDB = customerAdmin.GetAll();
            SaveAllData(bankDB);

            Console.WriteLine($"The Customer {name} added successfully.");

        }




        public void AddnewAccount(BankDB bankDB) 
        {

            var accountsAdmin = new BankGeneriskAdministration<BankAccount>();

            foreach (var a in bankDB.AllAccountsDatafromBankDB)
            {

                accountsAdmin.AddTo(a);
            
            }

            Console.WriteLine("Enter customer ID:");
            int customerId = int.Parse(Console.ReadLine()!);

            var customer = bankDB.AllAccountsDatafromBankDB.FirstOrDefault(customre  => customre.Id == customerId);

            if (customer == null) 
            {

                Console.WriteLine("customer not found.");
                return;
            
            }


            Console.WriteLine("Enter account Type(Saving account, personale account, Investment account)");

            string accountType = Console.ReadLine()!;

            Console.WriteLine("Enter initial balance:");

            decimal balance = decimal.Parse(Console.ReadLine()!);

            BankAccount newbankAccount = new(bankDB.AllAccountsDatafromBankDB.Count + 1, customerId, accountType, balance) 
            { 
            
              Id = bankDB.AllAccountsDatafromBankDB.Count + 1,
              CustomerId = customerId,
              AccountType = accountType,
              Balance = balance
        
            };


            accountsAdmin.AddTo(newbankAccount);
            bankDB.AllAccountsDatafromBankDB = accountsAdmin.GetAll();
            SaveAllData(bankDB);

            Console.WriteLine($"The Account created successfully.");

        }

        public void Deposit(BankDB bankDB)
        {
            Console.WriteLine("Enter account ID:");
            int accountId = int.Parse(Console.ReadLine()!);

            var account = bankDB.AllAccountsDatafromBankDB.FirstOrDefault(a => a.Id == accountId);
            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }

            Console.WriteLine("Enter amount to deposit:");
            decimal amount = decimal.Parse(Console.ReadLine()!);

            account.Balance += amount;

            account.Transactions.Add(new Transaction
            {
                TransactionId = account.Transactions.Count + 1,
                Amount = amount,
                Date = DateTime.Now,
                Type = "Deposit"
            });

            Console.WriteLine($"Successfully deposited {amount}. New balance: {account.Balance}");

            SaveAllData(bankDB);

        }

        public void Withdraw(BankDB bankDB)
        {
            Console.WriteLine("Enter account ID:");
            int accountId = int.Parse(Console.ReadLine()!);

            var account = bankDB.AllAccountsDatafromBankDB.FirstOrDefault(a => a.Id == accountId);
            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }

            Console.WriteLine("Enter amount to withdraw:");
            decimal amount = decimal.Parse(Console.ReadLine()!);

            if (amount > account.Balance)
            {
                Console.WriteLine("Insufficient balance.");
                return;
            }

            account.Balance -= amount;

            account.Transactions.Add(new Transaction
            {
                TransactionId = account.Transactions.Count + 1,
                Amount = -amount,
                Date = DateTime.Now,
                Type = "Withdrawal"
            });

            Console.WriteLine($"Successfully withdrew {amount}. New balance: {account.Balance}");
            SaveAllData(bankDB);

        }

        public void ViewAccountDetails(BankDB bankDB)
        {
            Console.WriteLine("Enter account ID:");
            int accountId = int.Parse(Console.ReadLine()!);

            var account = bankDB.AllAccountsDatafromBankDB.FirstOrDefault(a => a.Id == accountId);
            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }

            Console.WriteLine($"Account ID: {account.Id}");
            Console.WriteLine($"Customer ID: {account.CustomerId}");
            Console.WriteLine($"Account Type: {account.AccountType}");
            Console.WriteLine($"Balance: {account.Balance}");
            Console.WriteLine("Transactions:");
            foreach (var transaction in account.Transactions)
            {
                Console.WriteLine($"  {transaction.Date}: {transaction.Type} of {transaction.Amount}");
            }

            SaveAllData(bankDB);

        }


        public void SaveAllDataandExit(BankDB bankDB)
        {
            SaveAllData(bankDB);
            MirrorChangesToProjectRoot("BankAccountData.json");
            Console.WriteLine("Do you want to complete? (J/N)");
            string continueChoice = Console.ReadLine()!;
            if (continueChoice.ToUpper() == "N")
            {
                Environment.Exit(0);
            }

        }





        public void SaveAllData(BankDB bankDB)
        {
            string dataJsonFilePath = "BankAccountData.json";

            string updatedBankDB = JsonSerializer.Serialize(bankDB, new JsonSerializerOptions { WriteIndented = true });

            
            File.WriteAllText(dataJsonFilePath, updatedBankDB);

            MirrorChangesToProjectRoot("BankAccountData.json");

        }

        static void MirrorChangesToProjectRoot(string fileName)
        {
            // Get the path to the output directory
            string outputDir = AppDomain.CurrentDomain.BaseDirectory;

            // Get the path to the project root directory
            string projectRootDir = Path.Combine(outputDir, "../../../");

            // Define paths for the source (output directory) and destination (project root)
            string sourceFilePath = Path.Combine(outputDir, fileName);
            string destFilePath = Path.Combine(projectRootDir, fileName);

            // Copy the file if it exists
            if (File.Exists(sourceFilePath))
            {
                File.Copy(sourceFilePath, destFilePath, true); // true to overwrite
                Console.WriteLine($"{fileName} has been mirrored to the project root.");
            }
            else
            {
                Console.WriteLine($"Source file {fileName} not found.");
            }
        }
    }
}
