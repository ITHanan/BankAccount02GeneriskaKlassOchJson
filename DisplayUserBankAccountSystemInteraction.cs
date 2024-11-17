

using Figgle;
using Spectre.Console;
using System.Text.Json;

namespace BankAccount02GeneriskaKlassOchJson
{
    public class DisplayUserBankAccountSystemInteraction
    {
        BankSystem bankSystem = new BankSystem();

        public void Run()
        {

            // Figgle ASCII text
            var title = FiggleFonts.Standard.Render("Bank App");
            AnsiConsole.Write(new Markup($"[bold green]{title}[/]"));

            // Beskrivning med Spectre.Console
            AnsiConsole.Markup("[yellow]Välkommen till Bankapplikationen![/]\n");
            AnsiConsole.Markup("[bold]Välj ett alternativ från menyn nedan:[/]\n");


            string dataJsonFilePath = "BankAccountData.json";
            string allDataAsJson = File.ReadAllText(dataJsonFilePath);
            BankDB bankDB = JsonSerializer.Deserialize<BankDB>(allDataAsJson)!;

            bool running = true;

            
            while (running)
            {
                string userChoice = DisplayMenu(); // Use the new Spectre.Console menu

                switch (userChoice)
                {
                    case "1. Add Customer":
                        bankSystem.AddNewCustomer(bankDB);
                        break;
                    case "2. Add Account":
                        bankSystem.AddnewAccount(bankDB);
                        break;
                    case "3. Deposit Money":
                        bankSystem.Deposit(bankDB);
                        break;
                    case "4. Withdraw Money":
                        bankSystem.Withdraw(bankDB);
                        break;
                    case "5. View Account Details":
                        bankSystem.ViewAccountDetails(bankDB);
                        break;
                    case "6. Update Customer Detaile":
                        bankSystem.UpdateCustomerDetaile(bankDB);
                        break;
                    case "7. Save All Data and Exit":
                        bankSystem.SaveAllDataandExit(bankDB);
                        running = false;
                        break;
                    default:
                        AnsiConsole.MarkupLine("[red]Invalid choice! Please try again.[/]");
                        break;
                }
            }
        }

        private static string DisplayMenu()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Select an option from the menu:[/]")
                    .PageSize(6)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(new[]
                    {
                        "1. Add Customer",
                        "2. Add Account",
                        "3. Deposit Money",
                        "4. Withdraw Money",
                        "5. View Account Details",
                        "6. Update Customer Detaile",
                        "7. Save All Data and Exit"
                    }));
        }
    }
}
