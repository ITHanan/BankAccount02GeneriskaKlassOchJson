

using System.Text.Json.Serialization;

namespace BankAccount02GeneriskaKlassOchJson
{
    public class BankDB
    {

        [JsonPropertyName("Accounts")]
        public List<BankAccount> AllAccountsDatafromBankDB { get; set; } = new List<BankAccount>(); 

        [JsonPropertyName("Customers")]
        public List<Customer> AllCustomersDatafromBankDB { get; set; } = new List<Customer>();


    }
}
