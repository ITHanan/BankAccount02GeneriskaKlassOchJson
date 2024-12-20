﻿

using Figgle;
using Spectre.Console;
using System.Text.Json;

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

            DisplayCustomerAccount(bankDB, customerAdmin);

            Console.WriteLine("Enter customer name:");
            string name = Console.ReadLine()!;
            Console.WriteLine("Enter customer address:");
            string address = Console.ReadLine()!;

            var IsCustomerExists = bankDB.AllCustomersDatafromBankDB.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase))!;

            if (IsCustomerExists != null)
            {
                Console.WriteLine("The customer alredy exists. ");

            }

            Customer newCustomer = new(customerAdmin.GetAll().Count + 1, name, address)
            {

                AccountIDs = new List<int>()
            };

            customerAdmin.AddTo(newCustomer);
            bankDB.AllCustomersDatafromBankDB = customerAdmin.GetAll();
            SaveAllData(bankDB);

            Console.WriteLine($"The Customer {name} added successfully.");

        }

        private static void DisplayCustomerAccount(BankDB bankDB, BankGeneriskAdministration<Customer> customerAdmin)
        {
            // Display a stylish header using Figgle
            Console.WriteLine(FiggleFonts.Standard.Render("All Customers"));

            // Create a table using Spectre.Console
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("[bold yellow]Customer ID[/]");
            table.AddColumn("[bold green]Name[/]");
            table.AddColumn("[bold blue]Address[/]");
            table.AddColumn("[bold cyan]Account IDs[/]");

            // Populate the table with customer details
            foreach (var customer in bankDB.AllCustomersDatafromBankDB)
            {
                customerAdmin.AddTo(customer);
                table.AddRow(
                    customer.Id.ToString(),
                    customer.Name,
                    customer.Address,
                    string.Join(", ", customer.AccountIDs) // Display account IDs as a comma-separated list
                );
            }

            // Render the table
            AnsiConsole.Write(table);
        }

        public void AddnewAccount(BankDB bankDB)
        {

            var accountsAdmin = new BankGeneriskAdministration<BankAccount>();

            foreach (var a in bankDB.AllAccountsDatafromBankDB)
            {

                accountsAdmin.AddTo(a);

            }

            DisplayAvalableAccount(accountsAdmin);

            Console.WriteLine("Enter customer ID:");
            int customerId = int.Parse(Console.ReadLine()!);

            var customer = bankDB.AllCustomersDatafromBankDB.FirstOrDefault(customre => customre.Id == customerId);

            if (customer == null)
            {

                Console.WriteLine("customer not found.");
                return;

            }



            Console.WriteLine("Select the account type:");
            Console.WriteLine("1. Saving account");
            Console.WriteLine("2. Personal account");
            Console.WriteLine("3. Investment account");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                return;
            }

            string accountType;
            switch (choice)
            {
                case 1:
                    accountType = "Saving account";
                    break;
                case 2:
                    accountType = "Personal account";
                    break;
                case 3:
                    accountType = "Investment account";
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid account type.");
                    return;
            }

            Console.WriteLine($"You have selected: {accountType}");
            // Proceed with the rest of the logic using the selected accountType











            //Console.WriteLine("Enter account Type(Saving account, personale account, Investment account)");

            //string accountType = Console.ReadLine()!;

            //if (!new[] { "Saving account", "Personal account", "Investment account" }.Contains(accountType))
            //{

            //    Console.WriteLine("Invalid account type");
            //    return;

            //}



            Console.WriteLine("Enter initial balance:");

            decimal balance = decimal.Parse(Console.ReadLine()!);

            BankAccount newbankAccount = new(accountsAdmin.GetAll().Count + 1, customerId, accountType, balance);



            accountsAdmin.AddTo(newbankAccount);
            bankDB.AllAccountsDatafromBankDB = accountsAdmin.GetAll();
            SaveAllData(bankDB);

            Console.WriteLine($"The Account created successfully.");

        }

        private static void DisplayAvalableAccount(BankGeneriskAdministration<BankAccount> accountsAdmin)
        {
            // Display the available accounts to the user
            // Display a stylish header using Figgle
            Console.WriteLine(FiggleFonts.Standard.Render("Available Accounts"));

            // Create a table using Spectre.Console
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("[bold yellow]ID[/]");
            table.AddColumn("[bold green]Type[/]");
            table.AddColumn("[bold blue]Customer ID[/]");
            table.AddColumn("[bold cyan]Balance[/]");

            // Populate the table with account details
            foreach (var account in accountsAdmin.GetAll())
            {
                table.AddRow(
                    account.Id.ToString(),
                    account.AccountType,
                    account.CustomerId.ToString(),
                    account.Balance.ToString("C")
                );
            }

            // Render the table
            AnsiConsole.Write(table);
        }

        public void UpdateCustomerDetaile(BankDB bankDB)
        {
            try
            {
                // Display ASCII art header
                AnsiConsole.Write(new FigletText("Update Customer"));

                var customerAdmin = new BankGeneriskAdministration<Customer>();

                // Populate administration object with existing customers
                foreach (var c in bankDB.AllCustomersDatafromBankDB)
                {
                    customerAdmin.AddTo(c);
                }

                DisplayCustomerAccount(bankDB, customerAdmin);




                // Prompt for Customer ID
                AnsiConsole.Markup("[bold blue]Enter customer ID to update:[/] ");
                if (!int.TryParse(Console.ReadLine(), out int customerId) || customerId <= 0)
                {
                    AnsiConsole.MarkupLine("[bold red]Invalid customer ID. Please try again.[/]");
                    return;
                }

                // Find the customer
                var customer = customerAdmin.GetAll().FirstOrDefault(c => c.Id == customerId);
                if (customer == null)
                {
                    AnsiConsole.MarkupLine("[bold red]Customer not found.[/]");
                    return;
                }

                // Display current customer details in a styled table
                var table = new Table();
                table.AddColumn("[bold yellow]Customer ID[/]");
                table.AddColumn("[bold yellow]Name[/]");
                table.AddColumn("[bold yellow]Address[/]");
                table.AddRow(customer.Id.ToString(), customer.Name, customer.Address);
                AnsiConsole.Write(table);

                // Update Name
                Console.WriteLine();
                AnsiConsole.Markup("[bold blue]Do you want to update the name? (y/n):[/] ");
                string updateNameChoice = Console.ReadLine()!.ToLower();

                if (updateNameChoice == "y")
                {
                    AnsiConsole.Markup("[bold blue]Enter new name:[/] ");
                    string newName = Console.ReadLine()!;
                    if (!string.IsNullOrWhiteSpace(newName))
                    {
                        customer.Name = newName;
                        AnsiConsole.MarkupLine("[bold green]Name updated successfully![/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[bold red]Invalid name. No changes made.[/]");
                    }
                }

                // Update Address
                Console.WriteLine();
                AnsiConsole.Markup("[bold blue]Do you want to update the address? (y/n):[/] ");
                string updateAddressChoice = Console.ReadLine()!.ToLower();

                if (updateAddressChoice == "y")
                {
                    AnsiConsole.Markup("[bold blue]Enter new address:[/] ");
                    string newAddress = Console.ReadLine()!;
                    if (!string.IsNullOrWhiteSpace(newAddress))
                    {
                        customer.Address = newAddress;
                        AnsiConsole.MarkupLine("[bold green]Address updated successfully![/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[bold red]Invalid address. No changes made.[/]");
                    }
                }

                // Update the database
                customerAdmin.Updater(customer);
                bankDB.AllCustomersDatafromBankDB = customerAdmin.GetAll();
                SaveAllData(bankDB);

                // Success message with styling
                AnsiConsole.MarkupLine($"[bold green]Customer details updated successfully![/]");
                var successTable = new Table();
                successTable.AddColumn("[bold yellow]New Name[/]");
                successTable.AddColumn("[bold yellow]New Address[/]");
                successTable.AddRow(customer.Name, customer.Address);
                AnsiConsole.Write(successTable);
            }
            catch (FormatException ex)
            {
                AnsiConsole.MarkupLine($"[bold red]Input format error: {ex.Message}[/]");
            }
            catch (InvalidOperationException ex)
            {
                AnsiConsole.MarkupLine($"[bold red]Operation error: {ex.Message}[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]An unexpected error occurred: {ex.Message}[/]");
            }
        }


        public void UpdateAccountDetaile(BankDB bankDB)
        {
            try
            {
                // Display ASCII art header
                AnsiConsole.Write(new FigletText("Update Account"));

                var accountsAdmin = new BankGeneriskAdministration<BankAccount>();

                // Populate administration object with existing accounts
                foreach (var a in bankDB.AllAccountsDatafromBankDB)
                {
                    accountsAdmin.AddTo(a);
                }

                DisplayAvalableAccount(accountsAdmin);


                // Prompt for Account ID
                AnsiConsole.Markup("[bold blue]Enter account ID to update:[/] ");
                if (!int.TryParse(Console.ReadLine(), out int accountId) || accountId <= 0)
                {
                    AnsiConsole.MarkupLine("[bold red]Invalid account ID. Please try again.[/]");
                    return;
                }

                // Find the account
                var account = accountsAdmin.GetAll().FirstOrDefault(a => a.Id == accountId);
                if (account == null)
                {
                    AnsiConsole.MarkupLine("[bold red]Account not found.[/]");
                    return;
                }

                // Display current account details in a styled table
                var table = new Table();
                table.AddColumn("[bold yellow]Account ID[/]");
                table.AddColumn("[bold yellow]Customer ID[/]");
                table.AddColumn("[bold yellow]Account Type[/]");
                table.AddColumn("[bold yellow]Balance[/]");
                table.AddRow(account.Id.ToString(), account.CustomerId.ToString(), account.AccountType, account.Balance.ToString("C"));
                AnsiConsole.Write(table);

                // Update Account Type
                Console.WriteLine();
                AnsiConsole.Markup("[bold blue]Do you want to update the account type? (y/n):[/] ");
                string updateTypeChoice = Console.ReadLine()!.ToLower();

                if (updateTypeChoice == "y")
                {
                    AnsiConsole.Markup("[bold blue]Enter new account type:[/] ");
                    string newType = Console.ReadLine()!;
                    if (!string.IsNullOrWhiteSpace(newType))
                    {
                        account.AccountType = newType;
                        AnsiConsole.MarkupLine("[bold green]Account type updated successfully![/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[bold red]Invalid account type. No changes made.[/]");
                    }
                }

                // Update Balance
                Console.WriteLine();
                AnsiConsole.Markup("[bold blue]Do you want to update the balance? (y/n):[/] ");
                string updateBalanceChoice = Console.ReadLine()!.ToLower();

                if (updateBalanceChoice == "y")
                {
                    AnsiConsole.Markup("[bold blue]Enter new balance:[/] ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal newBalance))
                    {
                        account.Balance = newBalance;
                        AnsiConsole.MarkupLine("[bold green]Balance updated successfully![/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[bold red]Invalid balance. No changes made.[/]");
                    }
                }

                // Update the database
                accountsAdmin.Updater(account);
                bankDB.AllAccountsDatafromBankDB = accountsAdmin.GetAll();
                SaveAllData(bankDB);

                // Success message with styling
                AnsiConsole.MarkupLine($"[bold green]Account details updated successfully![/]");
                var successTable = new Table();
                successTable.AddColumn("[bold yellow]New Account Type[/]");
                successTable.AddColumn("[bold yellow]New Balance[/]");
                successTable.AddRow(account.AccountType, account.Balance.ToString("C"));
                AnsiConsole.Write(successTable);
            }
            catch (FormatException ex)
            {
                AnsiConsole.MarkupLine($"[bold red]Input format error: {ex.Message}[/]");
            }
            catch (InvalidOperationException ex)
            {
                AnsiConsole.MarkupLine($"[bold red]Operation error: {ex.Message}[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]An unexpected error occurred: {ex.Message}[/]");
            }
        }




        public void RemoveCustomer(BankDB bankDB)
        {
            try
            {
                // Display ASCII art header
                AnsiConsole.Write(new FigletText("Remove Customer"));

                var customerAdmin = new BankGeneriskAdministration<Customer>();

                // Populate the administration object with existing customers
                foreach (var customer in bankDB.AllCustomersDatafromBankDB)
                {
                    customerAdmin.AddTo(customer);
                }

                DisplayCustomerAccount(bankDB, customerAdmin);


                // Ask the user to enter the customer ID to delete
                AnsiConsole.Markup("[bold blue]Enter the customer ID that you want to delete:[/] ");
                if (!int.TryParse(Console.ReadLine(), out int Id) || Id <= 0)
                {
                    AnsiConsole.MarkupLine("[bold red]Invalid customer ID. Please try again.[/]");
                    return;
                }



                // Check if the customer was removed
                var customerToRemove = customerAdmin.GetByID(Id);
                if (customerToRemove == null)
                {
                    AnsiConsole.MarkupLine($"[bold red]Customer with ID {Id} not found.[/]");
                    return;

                }

                // Use the RemoveThis method to attempt to remove the customer
                customerAdmin.RemoveThis(Id);

                // Remove the customer and update the database
                bankDB.AllCustomersDatafromBankDB = customerAdmin.GetAll();
                SaveAllData(bankDB);

                // Success message
                AnsiConsole.MarkupLine($"[bold green]Customer with ID {Id} removed successfully![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]An unexpected error occurred: {ex.Message}[/]");
            }
        }


        public void RemoveAccount(BankDB bankDB)
        {
            try
            {
                // Display ASCII art header
                AnsiConsole.Write(new FigletText("Remove Account"));

                var accountsAdmin = new BankGeneriskAdministration<BankAccount>();

                // Populate the administration object with existing accounts
                foreach (var account in bankDB.AllAccountsDatafromBankDB)
                {
                    accountsAdmin.AddTo(account);
                }

                DisplayAvalableAccount(accountsAdmin);


                // Ask the user to enter the account ID to delete
                AnsiConsole.Markup("[bold blue]Enter the account ID that you want to delete:[/] ");
                if (!int.TryParse(Console.ReadLine(), out int Id) || Id <= 0)
                {
                    AnsiConsole.MarkupLine("[bold red]Invalid account ID. Please try again.[/]");
                    return;
                }

                // Check if the account exists
                var accountToRemove = accountsAdmin.GetByID(Id);
                if (accountToRemove == null)
                {
                    AnsiConsole.MarkupLine($"[bold red]Account with ID {Id} not found.[/]");
                    return;
                }

                // Confirm deletion
                var confirm = AnsiConsole.Confirm($"[bold red]Are you sure you want to delete account ID {Id}?[/]");
                if (!confirm)
                {
                    AnsiConsole.MarkupLine("[bold yellow]Account deletion canceled.[/]");
                    return;
                }

                // Use the RemoveThis method to attempt to remove the account
                accountsAdmin.RemoveThis(Id);

                // Update the bank's database with the modified account list
                bankDB.AllAccountsDatafromBankDB = accountsAdmin.GetAll();
                SaveAllData(bankDB);

                // Success message
                AnsiConsole.MarkupLine($"[bold green]Account with ID {Id} removed successfully![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]An unexpected error occurred: {ex.Message}[/]");
            }
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

            var accountsAdmin = new BankGeneriskAdministration<BankAccount>();

            // Populate administration object with existing accounts
            foreach (var a in bankDB.AllAccountsDatafromBankDB)
            {
                accountsAdmin.AddTo(a);
            }

            DisplayAvalableAccount(accountsAdmin);
            // Prompt the user for the Account ID
            var accountId = int.Parse(AnsiConsole.Ask<string>("[green]Enter the account ID:[/]"));

            // Find the account based on the provided ID
            var account = bankDB.AllAccountsDatafromBankDB.FirstOrDefault(a => a.Id == accountId);
            if (account == null)
            {
                AnsiConsole.MarkupLine("[red]Account not found.[/]");
                return;
            }

            // Display account details in a structured way
            AnsiConsole.MarkupLine($"[yellow]Account ID:[/] {account.Id}");
            AnsiConsole.MarkupLine($"[yellow]Customer ID:[/] {account.CustomerId}");
            AnsiConsole.MarkupLine($"[yellow]Account Type:[/] {account.AccountType}");
            AnsiConsole.MarkupLine($"[yellow]Balance:[/] {account.Balance:C}");

            // Table for transaction history
            var transactionTable = new Table();
            transactionTable.Border(TableBorder.Rounded);
            transactionTable.AddColumn("[yellow]Date[/]");
            transactionTable.AddColumn("[yellow]Transaction Type[/]");
            transactionTable.AddColumn("[yellow]Amount[/]");

            // Add each transaction as a row in the table
            foreach (var transaction in account.Transactions)
            {
                transactionTable.AddRow(
                    transaction.Date.ToShortDateString(),
                    transaction.Type,
                    transaction.Amount.ToString("C")
                );
            }

            // Display the transactions table
            AnsiConsole.MarkupLine("\n[yellow]Transactions:[/]");
            AnsiConsole.Write(transactionTable);

            // Save changes
            SaveAllData(bankDB);
        }




        public void SaveAllDataandExit(BankDB bankDB)
        {
            SaveAllData(bankDB);
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
